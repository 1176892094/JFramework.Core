﻿// *********************************************************************************
// # Project: JFramework.Lobby
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-08-28 20:08:49
// # Recently: 2024-12-23 00:12:20
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace JFramework.Net
{
    public class Process
    {
        private readonly Dictionary<string, Room> rooms = new Dictionary<string, Room>();
        private readonly Dictionary<int, Room> clients = new Dictionary<int, Room>();
        private readonly HashSet<int> connections = new HashSet<int>();
        private readonly Random random = new Random();
        private readonly Transport transport;

        public Process(Transport transport)
        {
            this.transport = transport;
        }

        public List<Room> roomInfo => rooms.Values.ToList();

        public void ServerError(int clientId, int error, string message)
        {
            string reason;
            switch (error)
            {
                case 1:
                    //        reason = "DnsResolve";
                    break;
                case 2:
                    //        reason = "Timeout";
                    break;
                case 3:
                    reason = "Congestion";
                    Debug.LogWarning($"客户端: {clientId} 错误代码: {reason}\n{message}");
                    break;
                case 4:
                    reason = "InvalidReceive";
                    Debug.LogWarning($"客户端: {clientId} 错误代码: {reason}\n{message}");
                    break;
                case 5:
                    reason = "InvalidSend";
                    Debug.LogWarning($"客户端: {clientId} 错误代码: {reason}\n{message}");
                    break;
                case 6:
                    reason = "ConnectionClosed";
                    Debug.LogWarning($"客户端: {clientId} 错误代码: {reason}\n{message}");
                    break;
                default:
                    reason = "Unexpected";
                    Debug.LogWarning($"客户端: {clientId} 错误代码: {reason}\n{message}");
                    break;
            }
        }

        public void ServerConnect(int clientId)
        {
            connections.Add(clientId);
            using var writer = NetworkWriter.Pop();
            writer.WriteByte((byte)OpCodes.Connect);
            transport.SendToClient(clientId, writer);
        }

        public void ServerDisconnect(int clientId)
        {
            var copies = rooms.Values.ToList();
            foreach (var room in copies)
            {
                if (room.clientId == clientId) // 主机断开
                {
                    using var writer = NetworkWriter.Pop();
                    writer.WriteByte((byte)OpCodes.LeaveRoom);
                    foreach (var client in room.clients)
                    {
                        transport.SendToClient(client, writer);
                        clients.Remove(client);
                    }

                    room.clients.Clear();
                    rooms.Remove(room.roomId);
                    clients.Remove(clientId);
                    return;
                }

                if (room.clients.Remove(clientId)) // 客户端断开
                {
                    using var writer = NetworkWriter.Pop();
                    writer.WriteByte((byte)OpCodes.KickRoom);
                    writer.WriteInt(clientId);
                    transport.SendToClient(room.clientId, writer);
                    clients.Remove(clientId);
                    break;
                }
            }
        }

        public void ServerReceive(int clientId, ArraySegment<byte> segment, int channel)
        {
            try
            {
                using var reader = NetworkReader.Pop(segment);
                var opcode = (OpCodes)reader.ReadByte();
                if (opcode == OpCodes.Connected)
                {
                    if (connections.Contains(clientId))
                    {
                        var serverKey = reader.ReadString();
                        if (serverKey == Program.Setting.ServerKey)
                        {
                            using var writer = NetworkWriter.Pop();
                            writer.WriteByte((byte)OpCodes.Connected);
                            transport.SendToClient(clientId, writer);
                        }

                        connections.Remove(clientId);
                    }
                }
                else if (opcode == OpCodes.CreateRoom)
                {
                    ServerDisconnect(clientId);
                    string id;
                    do
                    {
                        id = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ", 5).Select(s => s[random.Next(s.Length)]).ToArray());
                    } while (rooms.ContainsKey(id));

                    var room = new Room
                    {
                        roomId = id,
                        clientId = clientId,
                        roomName = reader.ReadString(),
                        roomData = reader.ReadString(),
                        maxCount = reader.ReadInt(),
                        isPublic = reader.ReadBool(),
                        clients = new HashSet<int>(),
                    };

                    rooms.Add(id, room);
                    clients.Add(clientId, room);
                    Debug.Log($"客户端 {clientId} 创建游戏房间。房间名：{room.roomName} 房间数：{rooms.Count} 连接数：{clients.Count}");

                    using var writer = NetworkWriter.Pop();
                    writer.WriteByte((byte)OpCodes.CreateRoom);
                    writer.WriteString(room.roomId);
                    transport.SendToClient(clientId, writer);
                }
                else if (opcode == OpCodes.JoinRoom)
                {
                    ServerDisconnect(clientId);
                    var roomId = reader.ReadString();
                    if (rooms.TryGetValue(roomId, out var room) && room.clients.Count + 1 < room.maxCount)
                    {
                        room.clients.Add(clientId);
                        clients.Add(clientId, room);
                        Debug.Log($"客户端 {clientId} 加入游戏房间。房间名：{room.roomName} 房间数：{rooms.Count} 连接数：{clients.Count}");

                        using var writer = NetworkWriter.Pop();
                        writer.WriteByte((byte)OpCodes.JoinRoom);
                        writer.WriteInt(clientId);
                        transport.SendToClient(clientId, writer);
                        transport.SendToClient(room.clientId, writer);
                    }
                    else
                    {
                        using var writer = NetworkWriter.Pop();
                        writer.WriteByte((byte)OpCodes.LeaveRoom);
                        transport.SendToClient(clientId, writer);
                    }
                }
                else if (opcode == OpCodes.UpdateRoom)
                {
                    if (clients.TryGetValue(clientId, out var room))
                    {
                        room.roomName = reader.ReadString();
                        room.roomData = reader.ReadString();
                        room.isPublic = reader.ReadBool();
                        room.maxCount = reader.ReadInt();
                    }
                }
                else if (opcode == OpCodes.LeaveRoom)
                {
                    ServerDisconnect(clientId);
                }
                else if (opcode == OpCodes.UpdateData)
                {
                    var message = reader.ReadArraySegment();
                    var targetId = reader.ReadInt();
                    if (clients.TryGetValue(clientId, out var room) && room != null)
                    {
                        if (message.Count > transport.MessageSize(channel))
                        {
                            Debug.Log($"接收消息大小过大！消息大小：{message.Count}");
                            ServerDisconnect(clientId);
                            return;
                        }

                        if (room.clientId == clientId)
                        {
                            if (room.clients.Contains(targetId))
                            {
                                using var writer = NetworkWriter.Pop();
                                writer.WriteByte((byte)OpCodes.UpdateData);
                                writer.WriteArraySegment(message);
                                transport.SendToClient(targetId, writer, channel);
                            }
                        }
                        else
                        {
                            using var writer = NetworkWriter.Pop();
                            writer.WriteByte((byte)OpCodes.UpdateData);
                            writer.WriteArraySegment(message);
                            writer.WriteInt(clientId);
                            transport.SendToClient(room.clientId, writer, channel);
                        }
                    }
                }
                else if (opcode == OpCodes.KickRoom)
                {
                    var targetId = reader.ReadInt();
                    var copies = rooms.Values.ToList();
                    foreach (var room in copies)
                    {
                        if (room.clientId == targetId) // 踢掉的是主机
                        {
                            using var writer = NetworkWriter.Pop();
                            writer.WriteByte((byte)OpCodes.LeaveRoom);
                            foreach (var client in room.clients)
                            {
                                transport.SendToClient(client, writer);
                                clients.Remove(client);
                            }

                            room.clients.Clear();
                            rooms.Remove(room.roomId);
                            clients.Remove(targetId);
                            return;
                        }

                        if (room.clientId == clientId) // 踢掉的是客户端
                        {
                            if (room.clients.Remove(targetId))
                            {
                                using var writer = NetworkWriter.Pop();
                                writer.WriteByte((byte)OpCodes.KickRoom);
                                writer.WriteInt(targetId);
                                transport.SendToClient(room.clientId, writer);
                                clients.Remove(targetId);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
                transport.StopClient(clientId);
            }
        }

        private enum OpCodes
        {
            Connect = 1,
            Connected = 2,
            JoinRoom = 3,
            CreateRoom = 4,
            UpdateRoom = 5,
            LeaveRoom = 6,
            UpdateData = 7,
            KickRoom = 8,
        }
    }
}