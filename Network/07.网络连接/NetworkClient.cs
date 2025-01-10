// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-21 23:12:50
// # Recently: 2024-12-22 21:12:45
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JFramework.Common;
using UnityEngine;

namespace JFramework.Net
{
    [Serializable]
    public sealed class NetworkClient
    {
        private Dictionary<int, WriterBatch> batches = new Dictionary<int, WriterBatch>();
        internal ReaderBatch reader = new ReaderBatch();
        public int clientId;
        public bool isReady;
        internal double remoteTime;

        public NetworkClient(int clientId)
        {
            this.clientId = clientId;
        }

        internal void Update()
        {
            foreach (var batch in batches)
            {
                using var writer = MemoryWriter.Pop();
                while (batch.Value.GetBatch(writer))
                {
                    Transport.Instance.SendToClient(clientId, writer, batch.Key);
                    writer.Reset();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Send<T>(T message, int channel = Channel.Reliable) where T : struct, IMessage
        {
            using var writer = MemoryWriter.Pop();
            writer.WriteUShort(Service.Hash<T>.Id);
            writer.Invoke(message);

            if (writer.position > Transport.Instance.MessageSize(channel))
            {
                Debug.LogError(Service.Text.Format("发送消息大小过大！消息大小: {0}", writer.position));
                return;
            }

            AddMessage(writer, channel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void AddMessage(MemoryWriter writer, int channel)
        {
            if (!batches.TryGetValue(channel, out var batch))
            {
                batch = new WriterBatch(Transport.Instance.MessageSize(channel));
                batches[channel] = batch;
            }

            batch.AddMessage(writer, Time.unscaledTimeAsDouble);

            if (clientId == NetworkManager.Server.hostId)
            {
                using var target = MemoryWriter.Pop();
                if (batch.GetBatch(target))
                {
                    NetworkManager.Client.OnClientReceive(target, Channel.Reliable);
                }
            }
        }

        public void Disconnect()
        {
            isReady = false;
            Transport.Instance.StopClient(clientId);
        }
    }
}