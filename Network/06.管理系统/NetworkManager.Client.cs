// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-21 23:12:50
// # Recently: 2024-12-22 21:12:48
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
        public static partial class Client
        {
            internal static readonly Dictionary<ushort, MessageDelegate> messages = new Dictionary<ushort, MessageDelegate>();

            internal static readonly Dictionary<ulong, NetworkObject> scenes = new Dictionary<ulong, NetworkObject>();

            internal static readonly Dictionary<uint, NetworkObject> spawns = new Dictionary<uint, NetworkObject>();

            private static StateMode state = StateMode.Disconnect;

            private static double waitTime;

            private static double pingTime;

            private static double sendTime;

            public static bool isActive => state != StateMode.Disconnect;

            public static bool isReady { get; internal set; }

            public static bool isLoadScene { get; internal set; }

            public static NetworkServer connection { get; private set; }

            public static bool isConnected => state == StateMode.Connected;

            internal static void Start(EntryMode mode)
            {
                if (mode == EntryMode.Host)
                {
                    Register(EntryMode.Host);
                    state = StateMode.Connected;
                    connection = new NetworkServer();
                    Server.Connect(new NetworkClient(Server.hostId));
                    Ready();
                    return;
                }

                Register(EntryMode.Client);
                state = StateMode.Connect;
                connection = new NetworkServer();
                Transport.StartClient();
            }

            internal static void Start(Uri uri)
            {
                Register(EntryMode.Client);
                state = StateMode.Connect;
                connection = new NetworkServer();
                Transport.StartClient(uri);
            }

            internal static void Stop()
            {
                if (!isActive) return;
                var objects = spawns.Values.Where(@object => @object != null).ToList();
                foreach (var @object in objects)
                {
                    @object.OnStopClient();
                    if (@object.sceneId != 0)
                    {
                        @object.gameObject.SetActive(false);
                        @object.Reset();
                    }
                    else
                    {
                        Destroy(@object.gameObject);
                    }
                }

                state = StateMode.Disconnect;
                if (Transport != null)
                {
                    Transport.StopClient();
                }

                sendTime = 0;
                waitTime = 0;
                pingTime = 0;
                isReady = false;
                spawns.Clear();
                scenes.Clear();
                messages.Clear();
                connection = null;
                isLoadScene = false;
                Service.Event.Invoke(new ClientDisconnectEvent());
            }

            private static void Pong()
            {
                if (waitTime + 2 <= Time.unscaledTimeAsDouble)
                {
                    waitTime = Time.unscaledTimeAsDouble;
                    connection.Send(new PongMessage(waitTime), Channel.Unreliable);
                }
            }

            public static void Ready()
            {
                if (connection == null)
                {
                    Log.Error("没有连接到有效的服务器！");
                    return;
                }

                if (isReady)
                {
                    Log.Error("客户端已经准备就绪！");
                    return;
                }

                isReady = true;
                connection.isReady = true;
                connection.Send(new ReadyMessage());
            }

            private static void Load(string sceneName)
            {
                if (string.IsNullOrWhiteSpace(sceneName))
                {
                    Log.Error("客户端不能加载空场景！");
                    return;
                }

                if (isLoadScene && NetworkManager.sceneName == sceneName)
                {
                    Log.Error(Utility.Text.Format("客户端正在加载 {0} 场景", sceneName));
                    return;
                }

                Service.Event.Invoke(new ClientLoadSceneEvent(sceneName));
                if (Server.isActive) return;
                isLoadScene = true;
                NetworkManager.sceneName = sceneName;

                Service.Asset.LoadScene(sceneName);
            }

            internal static void LoadSceneComplete(string sceneName)
            {
                isLoadScene = false;
                if (isConnected && !isReady)
                {
                    Ready();
                }

                Service.Event.Invoke(new ClientLoadCompleteEvent(sceneName));
            }
        }

        public static partial class Client
        {
            private static void Register(EntryMode mode)
            {
                if (mode == EntryMode.Client)
                {
                    Transport.OnClientConnect -= OnClientConnect;
                    Transport.OnClientDisconnect -= OnClientDisconnect;
                    Transport.OnClientError -= OnClientError;
                    Transport.OnClientReceive -= OnClientReceive;
                    Transport.OnClientConnect += OnClientConnect;
                    Transport.OnClientDisconnect += OnClientDisconnect;
                    Transport.OnClientError += OnClientError;
                    Transport.OnClientReceive += OnClientReceive;
                }

                Register<PingMessage>(PingMessage);
                Register<NotReadyMessage>(NotReadyMessage);
                Register<EntityMessage>(EntityMessage);
                Register<ClientRpcMessage>(ClientRpcMessage);

                Register<SceneMessage>(SceneMessage);
                Register<SpawnMessage>(SpawnMessage);
                Register<DespawnMessage>(DespawnMessage);
            }

            public static void Register<T>(Action<T> handle) where T : struct, IMessage
            {
                messages[Utility.Hash<T>.Id] = (client, reader, channel) =>
                {
                    try
                    {
                        var message = reader.Invoke<T>();
                        handle?.Invoke(message);
                    }
                    catch (Exception e)
                    {
                        Log.Error(Utility.Text.Format("{0} 调用失败。传输通道: {1}\n{2}", typeof(T).Name, channel, e));
                        client.Disconnect();
                    }
                };
            }

            private static void PingMessage(PingMessage message)
            {
                if (Server.isActive)
                {
                    return;
                }

                if (pingTime <= 0)
                {
                    pingTime = Time.unscaledTimeAsDouble - message.clientTime;
                }
                else
                {
                    var delta = Time.unscaledTimeAsDouble - message.clientTime - pingTime;
                    pingTime += 2.0 / (6 + 1) * delta;
                }

                Service.Event.Invoke(new PingUpdateEvent(pingTime));
            }

            private static void NotReadyMessage(NotReadyMessage message)
            {
                isReady = false;
                Service.Event.Invoke(new ClientNotReadyEvent());
            }

            private static void EntityMessage(EntityMessage message)
            {
                if (Server.isActive)
                {
                    return;
                }

                if (!spawns.TryGetValue(message.objectId, out var @object))
                {
                    Log.Warn(Utility.Text.Format("无法同步网络对象: {0}", message.objectId));
                    return;
                }

                if (@object == null)
                {
                    Log.Warn(Utility.Text.Format("无法同步网络对象: {0}", message.objectId));
                    return;
                }

                using var reader = MemoryReader.Pop(message.segment);
                @object.ClientDeserialize(reader, false);
            }

            private static void ClientRpcMessage(ClientRpcMessage message)
            {
                if (spawns.TryGetValue(message.objectId, out var @object))
                {
                    using var reader = MemoryReader.Pop(message.segment);
                    @object.InvokeMessage(message.componentId, message.methodHash, InvokeMode.ClientRpc, reader);
                }
            }

            private static void SceneMessage(SceneMessage message)
            {
                if (!isConnected)
                {
                    Log.Warn("客户端没有通过验证，无法加载场景。");
                    return;
                }

                Load(message.sceneName);
            }

            private static void SpawnMessage(SpawnMessage message)
            {
                if (Server.isActive)
                {
                    if (Server.spawns.TryGetValue(message.objectId, out var @object))
                    {
                        spawns[message.objectId] = @object;
                        @object.gameObject.SetActive(true);
                        if (message.isOwner)
                        {
                            @object.entityMode |= EntityMode.Owner;
                        }
                        else
                        {
                            @object.entityMode &= ~EntityMode.Owner;
                        }

                        @object.entityMode |= EntityMode.Client;
                        @object.OnStartClient();
                        @object.OnNotifyAuthority();
                    }

                    return;
                }

                scenes.Clear();
                var objects = Resources.FindObjectsOfTypeAll<NetworkObject>();
                foreach (var @object in objects)
                {
                    if (IsSceneObject(@object))
                    {
                        if (scenes.TryGetValue(@object.sceneId, out var obj))
                        {
                            Utility.Text.Format("客户端场景对象重复。网络对象: {0} {1}", @object.name, obj.name);
                            continue;
                        }

                        scenes.Add(@object.sceneId, @object);
                    }
                }

                SpawnObject(message);
            }

            private static void DespawnMessage(DespawnMessage message)
            {
                if (!spawns.TryGetValue(message.objectId, out var @object))
                {
                    return;
                }

                @object.OnStopClient();
                @object.entityMode &= ~EntityMode.Owner;
                @object.OnNotifyAuthority();
                spawns.Remove(message.objectId);

                if (Server.isActive)
                {
                    return;
                }

                if (@object.assetId.Equals(@object.name, StringComparison.OrdinalIgnoreCase))
                {
                    Service.Pool.Hide(@object.gameObject);
                    @object.Reset();
                    return;
                }

                Destroy(@object.gameObject);
            }
        }

        public static partial class Client
        {
            private static void OnClientConnect()
            {
                if (connection == null)
                {
                    Log.Error("没有连接到有效的服务器！");
                    return;
                }

                state = StateMode.Connected;
                Service.Event.Invoke(new ClientConnectEvent());
                Pong();
                Ready();
            }

            private static void OnClientDisconnect()
            {
                Stop();
            }

            private static void OnClientError(int error, string message)
            {
                var reason = error switch
                {
                    1 => "DnsResolve",
                    2 => "Timeout",
                    3 => "Congestion",
                    4 => "InvalidReceive",
                    5 => "InvalidSend",
                    6 => "ConnectionClosed",
                    _ => "Unexpected",
                };
                Log.Warn(Utility.Text.Format("错误代码: {0} => {1}", reason, message));
            }

            internal static void OnClientReceive(ArraySegment<byte> segment, int channel)
            {
                if (connection == null)
                {
                    Log.Error("没有连接到有效的服务器！");
                    return;
                }

                if (!connection.reader.AddBatch(segment))
                {
                    Log.Warn("无法处理来自服务器的消息。");
                    connection.Disconnect();
                    return;
                }

                while (!isLoadScene && connection.reader.GetMessage(out var newSeg, out var remoteTime))
                {
                    using var reader = MemoryReader.Pop(newSeg);
                    if (reader.residue < sizeof(ushort))
                    {
                        Log.Warn("无法处理来自服务器的消息。没有头部。");
                        connection.Disconnect();
                        return;
                    }

                    var message = reader.ReadUShort();
                    if (!messages.TryGetValue(message, out var action))
                    {
                        Log.Warn(Utility.Text.Format("无法处理来自服务器的消息。未知的消息{0}", message));
                        connection.Disconnect();
                        return;
                    }

                    connection.remoteTime = remoteTime;
                    action.Invoke(null, reader, channel);
                }

                if (!isLoadScene && connection.reader.Count > 0)
                {
                    Log.Warn(Utility.Text.Format("无法处理来自服务器的消息。残留消息: {0}", connection.reader.Count));
                }
            }
        }

        public static partial class Client
        {
            private static async void SpawnObject(SpawnMessage message)
            {
                if (spawns.TryGetValue(message.objectId, out var @object))
                {
                    Spawn(message, @object);
                    return;
                }

                if (message.sceneId == 0)
                {
                    GameObject prefab;
                    if (message.isPool)
                    {
                        prefab = await Service.Pool.Show(message.assetId);
                    }
                    else
                    {
                        prefab = await Service.Asset.Load<GameObject>(message.assetId);
                    }

                    if (!prefab.TryGetComponent(out @object))
                    {
                        Log.Error(Utility.Text.Format("无法注册网络对象 {0}。没有 NetworkObject 组件。", prefab.name));
                        return;
                    }

                    if (@object.sceneId != 0)
                    {
                        Log.Error(Utility.Text.Format("无法注册网络对象 {0}。因为该预置体为场景对象。", @object.name));
                        return;
                    }

                    if (@object.GetComponentsInChildren<NetworkObject>().Length > 1)
                    {
                        Log.Error(Utility.Text.Format("无法注册网络对象 {0}。持有多个 NetworkObject 组件。", @object.name));
                        return;
                    }
                }
                else
                {
                    if (!scenes.TryGetValue(message.sceneId, out @object))
                    {
                        Log.Error(Utility.Text.Format("无法注册网络对象 {0}。场景标识无效。", message.sceneId));
                        return;
                    }

                    scenes.Remove(message.sceneId);
                }

                Spawn(message, @object);
            }

            private static void Spawn(SpawnMessage message, NetworkObject @object)
            {
                if (!@object.gameObject.activeSelf)
                {
                    @object.gameObject.SetActive(true);
                }

                @object.objectId = message.objectId;
                if (message.isOwner)
                {
                    @object.entityMode |= EntityMode.Owner;
                }
                else
                {
                    @object.entityMode &= ~EntityMode.Owner;
                }

                @object.entityMode |= EntityMode.Client;
                var transform = @object.transform;
                transform.localPosition = message.position;
                transform.localRotation = message.rotation;
                transform.localScale = message.localScale;

                if (message.segment.Count > 0)
                {
                    using var reader = MemoryReader.Pop(message.segment);
                    @object.ClientDeserialize(reader, true);
                }

                spawns[message.objectId] = @object;
                @object.OnNotifyAuthority();
                @object.OnStartClient();
            }
        }

        public static partial class Client
        {
            internal static void EarlyUpdate()
            {
                if (Transport != null)
                {
                    Transport.ClientEarlyUpdate();
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

                if (connection != null)
                {
                    if (Mode == EntryMode.Host)
                    {
                        connection.Update();
                    }
                    else
                    {
                        if (isConnected)
                        {
                            Pong();
                            connection.Update();
                        }
                    }
                }

                if (Transport != null)
                {
                    Transport.ClientAfterUpdate();
                }
            }

            private static void Broadcast()
            {
                if (Server.isActive)
                {
                    return;
                }

                if (!connection.isReady)
                {
                    return;
                }

                foreach (var @object in spawns.Values)
                {
                    using var writer = MemoryWriter.Pop();
                    @object.ClientSerialize(writer);
                    if (writer.position > 0)
                    {
                        connection.Send(new EntityMessage(@object.objectId, writer));
                        @object.ClearDirty();
                    }
                }
            }
        }
    }
}