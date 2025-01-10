// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-10 01:01:43
// # Recently: 2025-01-10 01:01:43
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using JFramework.Common;
using UnityEngine;

namespace JFramework.Net
{
    public abstract class Transport : MonoBehaviour, ITransport
    {
        public static Transport Instance;
        public abstract string address { get; set; }
        public abstract ushort port { get; set; }
        public Action OnClientConnect { get; set; }
        public Action OnClientDisconnect { get; set; }
        public Action<int, string> OnClientError { get; set; }
        public Action<ArraySegment<byte>, int> OnClientReceive { get; set; }
        public Action<int> OnServerConnect { get; set; }
        public Action<int> OnServerDisconnect { get; set; }
        public Action<int, int, string> OnServerError { get; set; }
        public Action<int, ArraySegment<byte>, int> OnServerReceive { get; set; }
        public abstract int MessageSize(int channel);
        public abstract void SendToClient(int clientId, ArraySegment<byte> segment, int channel = Channel.Reliable);
        public abstract void SendToServer(ArraySegment<byte> segment, int channel =  Channel.Reliable);
        public abstract void StartServer();
        public abstract void StopServer();
        public abstract void StopClient(int clientId);
        public abstract void StartClient();
        public abstract void StartClient(Uri uri);
        public abstract void StopClient();
        public abstract void ClientEarlyUpdate();
        public abstract void ClientAfterUpdate();
        public abstract void ServerEarlyUpdate();
        public abstract void ServerAfterUpdate();
    }
}