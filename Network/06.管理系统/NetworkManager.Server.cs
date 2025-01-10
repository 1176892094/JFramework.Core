// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-21 23:12:50
// # Recently: 2024-12-22 21:12:49
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JFramework.Net
{
    using MessageDelegate = Action<NetworkClient, MemoryReader, int>;

    public partial class NetworkManager
    {
        public static partial class Server
        {
            internal static readonly Dictionary<ushort, MessageDelegate> messages = new Dictionary<ushort, MessageDelegate>();

            internal static readonly Dictionary<int, NetworkClient> clients = new Dictionary<int, NetworkClient>();

            internal static readonly Dictionary<uint, NetworkObject> spawns = new Dictionary<uint, NetworkObject>();

            private static StateMode state = StateMode.Disconnect;

            private static List<NetworkClient> copies = new List<NetworkClient>();

            private static uint objectId;

            private static double sendTime;

            public static bool isActive => state != StateMode.Disconnect;

            public static bool isReady => clients.Values.All(client => client.isReady);

            public static bool isLoadScene { get; internal set; }

            public static int hostId { get; private set; } = 0;

            public static int connections => clients.Count;

            internal static void Start(EntryMode mode)
            {
                switch (mode)
                {
                    case EntryMode.Host:
                        Transport.StartServer();
                        break;
                    case EntryMode.Server:
                        Transport.StartServer();
                        break;
                }

                Register();
                clients.Clear();
                state = StateMode.Connected;
                SpawnObjects();
            }

            internal static void Stop()
            {
                if (!isActive) return;
                state = StateMode.Disconnect;
                copies = clients.Values.ToList();
                foreach (var client in copies)
                {
                    client.Disconnect();
                    if (client.clientId != hostId)
                    {
                        OnServerDisconnect(client.clientId);
                    }
                }

                if (Transport != null)
                {
                    Transport.StopServer();
                }

                sendTime = 0;
                objectId = 0;
                spawns.Clear();
                clients.Clear();
                messages.Clear();
                isLoadScene = false;
            }

            internal static void Connect(NetworkClient client)
            {
                if (!clients.ContainsKey(client.clientId))
                {
                    clients.Add(client.clientId, client);
                    Utility.Event.Invoke(new ServerConnectEvent(client));
                }
            }

            public static void Load(string sceneName)
            {
                if (string.IsNullOrWhiteSpace(sceneName))
                {
                    Log.Error("服务器不能加载空场景！");
                    return;
                }

                if (isLoadScene && NetworkManager.sceneName == sceneName)
                {
                    Log.Error(Utility.Text.Format("服务器正在加载 {0} 场景", sceneName));
                    return;
                }

                foreach (var client in clients.Values)
                {
                    client.isReady = false;
                    client.Send(new NotReadyMessage());
                }

                Utility.Event.Invoke(new ServerLoadSceneEvent(sceneName));
                if (!isActive) return;
                isLoadScene = true;
                NetworkManager.sceneName = sceneName;

                foreach (var client in clients.Values)
                {
                    client.Send(new SceneMessage(sceneName));
                }

                Service.Asset.LoadScene(sceneName);
            }

            internal static void LoadSceneComplete(string sceneName)
            {
                isLoadScene = false;
                SpawnObjects();
                Utility.Event.Invoke(new ServerLoadCompleteEvent(sceneName));
            }
        }

        public static partial class Server
        {
            private static void Register()
            {
                Transport.OnServerConnect -= OnServerConnect;
                Transport.OnServerDisconnect -= OnServerDisconnect;
                Transport.OnServerReceive -= OnServerReceive;
                Transport.OnServerConnect += OnServerConnect;
                Transport.OnServerDisconnect += OnServerDisconnect;
                Transport.OnServerReceive += OnServerReceive;
                Register<PongMessage>(PongMessage);
                Register<ReadyMessage>(ReadyMessage);
                Register<EntityMessage>(EntityMessage);
                Register<ServerRpcMessage>(ServerRpcMessage);
            }

            public static void Register<T>(Action<NetworkClient, T> handle) where T : struct, IMessage
            {
                messages[Utility.Hash<T>.Id] = (client, reader, channel) =>
                {
                    try
                    {
                        var message = reader.Invoke<T>();
                        handle?.Invoke(client, message);
                    }
                    catch (Exception e)
                    {
                        Log.Error(Utility.Text.Format("{0} 调用失败。传输通道: {1}\n{2}", typeof(T).Name, channel, e));
                        client.Disconnect();
                    }
                };
            }

            public static void Register<T>(Action<NetworkClient, T, int> handle) where T : struct, IMessage
            {
                messages[Utility.Hash<T>.Id] = (client, reader, channel) =>
                {
                    try
                    {
                        var message = reader.Invoke<T>();
                        handle?.Invoke(client, message, channel);
                    }
                    catch (Exception e)
                    {
                        Log.Error(Utility.Text.Format("{0} 调用失败。传输通道: {1}\n{2}", typeof(T).Name, channel, e));
                        client.Disconnect();
                    }
                };
            }

            internal static void PongMessage(NetworkClient client, PongMessage message)
            {
                client.Send(new PingMessage(message.clientTime), Channel.Unreliable);
            }

            internal static void ReadyMessage(NetworkClient client, ReadyMessage message)
            {
                client.isReady = true;
                foreach (var @object in spawns.Values.Where(@object => @object.gameObject.activeSelf))
                {
                    SpawnToClient(client, @object);
                }

                Utility.Event.Invoke(new ServerReadyEvent(client));
            }

            internal static void EntityMessage(NetworkClient client, EntityMessage message)
            {
                if (!spawns.TryGetValue(message.objectId, out var @object))
                {
                    Log.Warn(Utility.Text.Format("无法为客户端 {0} 同步网络对象: {1}", client.clientId, message.objectId));
                    return;
                }

                if (@object == null)
                {
                    Log.Warn(Utility.Text.Format("无法为客户端 {0} 同步网络对象: {1}", client.clientId, message.objectId));
                    return;
                }

                if (@object.connection != client)
                {
                    Log.Warn(Utility.Text.Format("无法为客户端 {0} 同步网络对象: {1}", client.clientId, message.objectId));
                    return;
                }

                using var reader = MemoryReader.Pop(message.segment);
                if (!@object.ServerDeserialize(reader))
                {
                    Log.Warn(Utility.Text.Format("无法为客户端 {0} 反序列化网络对象: {1}", client.clientId, message.objectId));
                    client.Disconnect();
                }
            }

            internal static void ServerRpcMessage(NetworkClient client, ServerRpcMessage message, int channel)
            {
                if (!client.isReady)
                {
                    if (channel != Channel.Reliable) return;
                    Log.Warn(Utility.Text.Format("无法为客户端 {0} 进行远程调用，未准备就绪。", client.clientId));
                    return;
                }

                if (!spawns.TryGetValue(message.objectId, out var @object))
                {
                    Log.Warn(Utility.Text.Format("无法为客户端 {0} 进行远程调用，未找到对象 {1}。", client.clientId, message.objectId));
                    return;
                }

                if (NetworkDelegate.RequireReady(message.methodHash) && @object.connection != client)
                {
                    Log.Warn(Utility.Text.Format("无法为客户端 {0} 进行远程调用，未通过验证 {1}。", client.clientId, message.objectId));
                    return;
                }

                using var reader = MemoryReader.Pop(message.segment);
                @object.InvokeMessage(message.componentId, message.methodHash, InvokeMode.ServerRpc, reader, client);
            }
        }

        public partial class Server
        {
            private static void OnServerConnect(int clientId)
            {
                if (clientId == 0)
                {
                    Log.Warn(Utility.Text.Format("无法为客户端 {0} 建立通信连接。", clientId));
                    Transport.StopClient(clientId);
                }
                else if (clients.ContainsKey(clientId))
                {
                    Transport.StopClient(clientId);
                }
                else if (clients.Count >= Instance.connection)
                {
                    Transport.StopClient(clientId);
                }
                else
                {
                    Connect(new NetworkClient(clientId));
                }
            }

            internal static void OnServerDisconnect(int clientId)
            {
                if (clients.TryGetValue(clientId, out var client))
                {
                    var objects = spawns.Values.Where(@object => @object.connection == client).ToList();
                    foreach (var @object in objects)
                    {
                        Destroy(@object);
                    }

                    clients.Remove(client.clientId);
                    Utility.Event.Invoke(new ServerDisconnectEvent(client));
                }
            }

            internal static void OnServerReceive(int clientId, ArraySegment<byte> segment, int channel)
            {
                if (!clients.TryGetValue(clientId, out var client))
                {
                    Log.Warn(Utility.Text.Format("无法为客户端 {0} 进行处理消息。未知客户端。", clientId));
                    return;
                }

                if (!client.reader.AddBatch(segment))
                {
                    Log.Warn(Utility.Text.Format("无法为客户端 {0} 进行处理消息。", clientId));
                    client.Disconnect();
                    return;
                }

                while (!isLoadScene && client.reader.GetMessage(out var newSeg, out var remoteTime))
                {
                    using var reader = MemoryReader.Pop(newSeg);
                    if (reader.residue < sizeof(ushort))
                    {
                        Log.Warn(Utility.Text.Format("无法为客户端 {0} 进行处理消息。没有头部。", clientId));
                        client.Disconnect();
                        return;
                    }

                    var message = reader.ReadUShort();
                    if (!messages.TryGetValue(message, out var action))
                    {
                        Log.Warn(Utility.Text.Format("无法为客户端 {0} 进行处理消息。未知的消息{1}。", clientId, message));
                        client.Disconnect();
                        return;
                    }

                    client.remoteTime = remoteTime;
                    action.Invoke(client, reader, channel);
                }

                if (!isLoadScene && client.reader.Count > 0)
                {
                    Log.Warn(Utility.Text.Format("无法为客户端 {0} 进行处理消息。残留消息: {1}。", clientId, client.reader.Count));
                }
            }
        }

        public partial class Server
        {
            internal static void SpawnObjects()
            {
                var objects = Resources.FindObjectsOfTypeAll<NetworkObject>();
                foreach (var @object in objects)
                {
                    if (IsSceneObject(@object) && @object.objectId == 0)
                    {
                        @object.gameObject.SetActive(true);
                        var parent = @object.transform.parent;
                        if (parent == null || parent.gameObject.activeInHierarchy)
                        {
                            Spawn(@object.gameObject, @object.connection);
                        }
                    }
                }
            }

            public static void Spawn(GameObject obj, NetworkClient client = null)
            {
                if (!isActive)
                {
                    Log.Error("服务器不是活跃的。", obj);
                    return;
                }

                if (!obj.TryGetComponent(out NetworkObject @object))
                {
                    Log.Error(Utility.Text.Format("网络对象 {0} 没有 NetworkObject 组件", obj), obj);
                    return;
                }

                if (spawns.ContainsKey(@object.objectId))
                {
                    Log.Warn(Utility.Text.Format("网络对象 {0} 已经生成。", @object), @object);
                    return;
                }

                @object.connection = client;

                if (Mode == EntryMode.Host && client?.clientId == hostId)
                {
                    @object.entityMode |= EntityMode.Owner;
                }

                if ((@object.entityMode & EntityMode.Server) == 0 && @object.objectId == 0)
                {
                    @object.objectId = ++objectId;
                    @object.entityMode |= EntityMode.Server;
                    if (Client.isActive)
                    {
                        @object.entityMode |= EntityMode.Client;
                    }
                    else
                    {
                        @object.entityMode &= ~EntityMode.Owner;
                    }

                    spawns[@object.objectId] = @object;
                    @object.OnStartServer();
                }

                SpawnToClients(@object);
            }

            private static void SpawnToClients(NetworkObject @object)
            {
                foreach (var client in clients.Values.Where(client => client.isReady))
                {
                    SpawnToClient(client, @object);
                }
            }

            private static void SpawnToClient(NetworkClient client, NetworkObject @object)
            {
                using MemoryWriter writer = MemoryWriter.Pop(), observer = MemoryWriter.Pop();
                var isOwner = @object.connection == client;
                var transform = @object.transform;

                ArraySegment<byte> segment = default;
                if (@object.entities.Length != 0)
                {
                    @object.ServerSerialize(true, writer, observer);
                    segment = isOwner ? writer : observer;
                }

                var message = new SpawnMessage
                {
                    isOwner = isOwner,
                    isPool = @object.assetId.Equals(@object.name, StringComparison.OrdinalIgnoreCase),
                    assetId = @object.assetId,
                    sceneId = @object.sceneId,
                    objectId = @object.objectId,
                    position = transform.localPosition,
                    rotation = transform.localRotation,
                    localScale = transform.localScale,
                    segment = segment
                };

                client.Send(message);
            }

            public static void Despawn(GameObject obj)
            {
                if (!obj.TryGetComponent(out NetworkObject @object))
                {
                    return;
                }

                spawns.Remove(@object.objectId);
                foreach (var client in clients.Values)
                {
                    client.Send(new DespawnMessage(@object.objectId));
                }

                @object.OnStopServer();
                if (@object.assetId.Equals(@object.name, StringComparison.OrdinalIgnoreCase))
                {
                    Service.Pool.Hide(@object.gameObject);
                    @object.Reset();
                    return;
                }

                @object.entityState |= EntityState.Destroy;
                Destroy(@object.gameObject);
            }
        }

        public partial class Server
        {
            internal static void EarlyUpdate()
            {
                if (Transport != null)
                {
                    Transport.ServerEarlyUpdate();
                }
            }

            internal static void AfterUpdate()
            {
                if (isActive)
                {
                    if (Tick(ref sendTime))
                    {
                        Broadcast();
                    }
                }

                if (Transport != null)
                {
                    Transport.ServerAfterUpdate();
                }
            }

            private static void Broadcast()
            {
                copies.Clear();
                copies.AddRange(clients.Values);
                foreach (var client in copies)
                {
                    if (client.isReady)
                    {
                        foreach (var @object in spawns.Values)
                        {
                            if (@object == null)
                            {
                                Log.Warn(Utility.Text.Format("在客户端 {0} 找到了空的网络对象。", client.clientId));
                                return;
                            }

                            var serialize = @object.Synchronization(Time.frameCount);
                            if (@object.connection == client)
                            {
                                if (serialize.owner.position > 0)
                                {
                                    client.Send(new EntityMessage(@object.objectId, serialize.owner));
                                }
                            }
                            else
                            {
                                if (serialize.observer.position > 0)
                                {
                                    client.Send(new EntityMessage(@object.objectId, serialize.observer));
                                }
                            }
                        }
                    }

                    client.Update();
                }
            }
        }
    }
}