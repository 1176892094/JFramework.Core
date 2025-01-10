// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-11-29 13:11:20
// # Recently: 2024-12-22 21:12:51
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace JFramework.Net
{
    public partial class NetworkManager : MonoBehaviour, IEvent<SceneCompleteEvent>
    {
        public static NetworkManager Instance;

        [SerializeField] private Transport transport;

        [SerializeField, Range(30, 120)] private int sendRate = 30;

        public int connection = 100;

        private static string sceneName { get; set; }

        public static Transport Transport
        {
            get => Instance.transport;
            set => Instance.transport = value;
        }

        public static EntryMode Mode
        {
            get
            {
                if (!Application.isPlaying)
                {
                    return EntryMode.None;
                }

                if (Server.isActive)
                {
                    return Client.isActive ? EntryMode.Host : EntryMode.Server;
                }

                return Client.isActive ? EntryMode.Client : EntryMode.None;
            }
        }

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Application.runInBackground = true;
        }

        private void OnEnable()
        {
            Service.Event.Listen(this);
        }

        private void OnDisable()
        {
            Service.Event.Remove(this);
        }

        private void OnApplicationQuit()
        {
            if (Client.isConnected)
            {
                StopClient();
            }

            if (Server.isActive)
            {
                StopServer();
            }
        }

        public void Execute(SceneCompleteEvent message)
        {
            switch (Mode)
            {
                case EntryMode.Host:
                    Server.LoadSceneComplete(sceneName);
                    Client.LoadSceneComplete(sceneName);
                    break;
                case EntryMode.Server:
                    Server.LoadSceneComplete(sceneName);
                    break;
                case EntryMode.Client:
                    Client.LoadSceneComplete(sceneName);
                    break;
            }
        }

        public static void StartServer()
        {
            if (Server.isActive)
            {
                Log.Warn("服务器已经连接！");
                return;
            }

            Server.Start(EntryMode.Server);
        }

        public static void StopServer()
        {
            if (!Server.isActive)
            {
                Log.Warn("服务器已经停止！");
                return;
            }

            Server.Stop();
        }

        public static void StartClient()
        {
            if (Client.isActive)
            {
                Log.Warn("客户端已经连接！");
                return;
            }

            Client.Start(EntryMode.Client);
        }

        public static void StartClient(Uri uri)
        {
            if (Client.isActive)
            {
                Log.Warn("客户端已经连接！");
                return;
            }

            Client.Start(uri);
        }

        public static void StopClient()
        {
            if (!Client.isActive)
            {
                Log.Warn("客户端已经停止！");
                return;
            }

            if (Mode == EntryMode.Host)
            {
                Server.OnServerDisconnect(Server.hostId);
            }

            Client.Stop();
        }

        public static void StartHost(EntryMode mode = EntryMode.Host)
        {
            if (Server.isActive || Client.isActive)
            {
                Log.Warn("客户端或服务器已经连接！");
                return;
            }

            Server.Start(mode);
            Client.Start(EntryMode.Host);
        }

        public static void StopHost()
        {
            StopClient();
            StopServer();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NetworkObject GetNetworkObject(uint objectId)
        {
            if (Server.isActive)
            {
                Server.spawns.TryGetValue(objectId, out var @object);
                return @object;
            }

            if (Client.isActive)
            {
                Client.spawns.TryGetValue(objectId, out var @object);
                return @object;
            }

            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Tick(ref double sendTime)
        {
            var duration = 1.0 / Instance.sendRate;
            if (sendTime + duration <= Time.unscaledTimeAsDouble)
            {
                sendTime = (long)(Time.unscaledTimeAsDouble / duration) * duration;
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsSceneObject(NetworkObject @object)
        {
            if (@object.sceneId == 0)
            {
                return false;
            }

            if (@object.gameObject.hideFlags == HideFlags.NotEditable)
            {
                return false;
            }

            return @object.gameObject.hideFlags != HideFlags.HideAndDontSave;
        }
    }
}