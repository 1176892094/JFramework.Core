using System;
using System.Net;
using System.Net.Sockets;
using JFramework.Udp;
using UnityEngine;

namespace JFramework.Net
{
    public class NetworkDiscovery : MonoBehaviour
    {
        [SerializeField] private string address = "";

        [SerializeField] private ushort port = 47777;

        [SerializeField] private int duration = 1;

        [SerializeField] private string version;

        private UdpClient udpClient;

        private UdpClient udpServer;

        public void StartDiscovery()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                Debug.LogError("网络发现不支持WebGL");
                return;
            }

            StopDiscovery();
            switch (NetworkManager.Mode)
            {
                case EntryMode.Server or EntryMode.Host:
                    udpServer = new UdpClient(port)
                    {
                        EnableBroadcast = true,
                        MulticastLoopback = false
                    };
                    BeginMulticastLock();
                    ServerReceive();
                    break;
                case EntryMode.Client:
                    udpClient = new UdpClient(0)
                    {
                        EnableBroadcast = true,
                        MulticastLoopback = false
                    };
                    ClientReceive();
                    InvokeRepeating(nameof(ClientSend), 0, duration);
                    break;
            }
        }

        public void StopDiscovery()
        {
            EndMulticastLock();
            udpServer?.Close();
            udpClient?.Close();
            udpServer = null;
            udpClient = null;
            CancelInvoke();
        }

        private void OnDestroy()
        {
            StopDiscovery();
        }

        private static void BeginMulticastLock()
        {
            var setting = Resources.Load<ScriptableObject>("GlobalSetting");
            if (setting != null)
            {
                var message = setting.GetType().GetMethod("BeginMulticastLock", Service.Depend.Instance);
                message?.Invoke(setting, null);
            }
        }

        private static void EndMulticastLock()
        {
            var setting = Resources.Load<ScriptableObject>("GlobalSetting");
            if (setting != null)
            {
                var message = setting.GetType().GetMethod("EndMulticastLock", Service.Depend.Instance);
                message?.Invoke(setting, null);
            }
        }

        private void ClientSend()
        {
            try
            {
                if (NetworkManager.Client.isConnected)
                {
                    StopDiscovery();
                    return;
                }

                var endPoint = new IPEndPoint(IPAddress.Broadcast, port);
                if (!string.IsNullOrWhiteSpace(address))
                {
                    endPoint = new IPEndPoint(IPAddress.Parse(address), port);
                }

                using var writer = MemoryWriter.Pop();
                writer.WriteString(version);
                writer.Invoke(new RequestMessage());
                ArraySegment<byte> segment = writer;
                udpClient.Send(segment.Array, segment.Count, endPoint);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private async void ServerReceive()
        {
            while (udpServer != null)
            {
                try
                {
                    var result = await udpServer.ReceiveAsync();
                    using var reader = MemoryReader.Pop(new ArraySegment<byte>(result.Buffer));
                    if (version != reader.ReadString())
                    {
                        Debug.LogError("接收到的消息版本不同！");
                        return;
                    }

                    reader.Invoke<RequestMessage>();
                    ServerSend(result.RemoteEndPoint);
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        private void ServerSend(IPEndPoint endPoint)
        {
            try
            {
                using var writer = MemoryWriter.Pop();
                writer.WriteString(version);
                writer.Invoke(new ResponseMessage(new UriBuilder
                {
                    Scheme = "https",
                    Host = Dns.GetHostName(),
                    Port = ((IAddress)NetworkManager.Transport).port
                }.Uri));
                ArraySegment<byte> segment = writer;
                udpServer.Send(segment.Array, segment.Count, endPoint);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private async void ClientReceive()
        {
            while (udpClient != null)
            {
                try
                {
                    var result = await udpClient.ReceiveAsync();
                    using var reader = MemoryReader.Pop(new ArraySegment<byte>(result.Buffer));
                    if (version != reader.ReadString())
                    {
                        Debug.LogError("接收到的消息版本不同息！");
                        return;
                    }

                    var endPoint = result.RemoteEndPoint;
                    var response = reader.Invoke<ResponseMessage>();
                    Service.Event.Invoke(new ServerResponseEvent(new UriBuilder(response.uri)
                    {
                        Host = endPoint.Address.ToString()
                    }.Uri, endPoint));
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }
}