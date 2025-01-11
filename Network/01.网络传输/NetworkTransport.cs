// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-11-29 13:11:20
// # Recently: 2024-12-22 20:12:20
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using JFramework.Udp;
using UnityEngine;

namespace JFramework.Net
{
    public sealed class NetworkTransport : Transport
    {
        [SerializeField] private string _address = "localhost";
        [SerializeField] private ushort _port = 20974;
        public int maxUnit = 1200;
        public uint timeout = 10000;
        public uint interval = 10;
        public uint deadLink = 40;
        public uint fastResend = 2;
        public uint sendWindow = 1024 * 4;
        public uint receiveWindow = 1024 * 4;

        private Client client;
        private Server server;

        public override ushort port
        {
            get => _port;
            set => _port = value;
        }

        public override string address
        {
            get => _address;
            set => _address = value;
        }

        private void Awake()
        {
            Log.Info = Debug.Log;
            Log.Warn = Debug.LogWarning;
            Log.Error = Debug.LogError;
            var setting = new Setting(maxUnit, timeout, interval, deadLink, fastResend, sendWindow, receiveWindow);
            client = new Client(setting, ClientConnect, ClientDisconnect, ClientError, ClientReceive);
            server = new Server(setting, ServerConnect, ServerDisconnect, ServerError, ServerReceive);
            return;

            void ClientConnect()
            {
                OnClientConnect.Invoke();
            }

            void ClientDisconnect()
            {
                OnClientDisconnect.Invoke();
            }

            void ClientError(int error, string message)
            {
                OnClientError?.Invoke(error, message);
            }

            void ClientReceive(ArraySegment<byte> message, int channel)
            {
                OnClientReceive.Invoke(message, channel);
            }

            void ServerConnect(int clientId)
            {
                OnServerConnect.Invoke(clientId);
            }

            void ServerDisconnect(int clientId)
            {
                OnServerDisconnect.Invoke(clientId);
            }

            void ServerError(int clientId, int error, string message)
            {
                OnServerError?.Invoke(clientId, error, message);
            }

            void ServerReceive(int clientId, ArraySegment<byte> message, int channel)
            {
                OnServerReceive.Invoke(clientId, message, channel);
            }
        }

        public override int MessageSize(int channel)
        {
            return channel == Channel.Reliable ? Agent.ReliableSize(maxUnit, receiveWindow) : Agent.UnreliableSize(maxUnit);
        }

        public override void StartServer()
        {
            server.Connect(_port);
        }

        public override void StopServer()
        {
            server.StopServer();
        }

        public override void StopClient(int clientId)
        {
            server.Disconnect(clientId);
        }

        public override void SendToClient(int clientId, ArraySegment<byte> segment, int channel = Channel.Reliable)
        {
            server.Send(clientId, segment, channel);
        }

        public override void StartClient()
        {
            client.Connect(_address, _port);
        }

        public override void StartClient(Uri uri)
        {
            client.Connect(uri.Host, (ushort)(uri.IsDefaultPort ? _port : uri.Port));
        }

        public override void StopClient()
        {
            client.Disconnect();
        }

        public override void SendToServer(ArraySegment<byte> segment, int channel = Channel.Reliable)
        {
            client.Send(segment, channel);
        }

        public override void ClientEarlyUpdate()
        {
            client.EarlyUpdate();
        }

        public override void ClientAfterUpdate()
        {
            client.AfterUpdate();
        }

        public override void ServerEarlyUpdate()
        {
            server.EarlyUpdate();
        }

        public override void ServerAfterUpdate()
        {
            server.AfterUpdate();
        }
    }
}