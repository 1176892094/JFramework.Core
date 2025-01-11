// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-05 20:12:40
// # Recently: 2024-12-22 20:12:19
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework.Net
{
    public enum EntryMode : byte
    {
        None = 0,
        Host = 1,
        Server = 2,
        Client = 3,
    }

    internal enum OpCodes : byte
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

    public enum RoomMode : byte
    {
        Public,
        Private,
        Locked,
    }

    public enum SyncMode : byte
    {
        Server,
        Client
    }

    internal enum StateMode : byte
    {
        Connect = 0,
        Connected = 1,
        Disconnect = 2,
    }

    internal enum InvokeMode : byte
    {
        ServerRpc,
        ClientRpc,
    }

    [Flags]
    internal enum EntityMode : byte
    {
        None = 0,
        Owner = 1 << 0,
        Client = 1 << 1,
        Server = 1 << 2,
    }

    [Flags]
    internal enum EntityState : byte
    {
        None = 0,
        Spawn = 1 << 0,
        Destroy = 1 << 1,
        Authority = 1 << 2,
    }
    
    [Flags]
    internal enum TransformOption
    {
        None,
        Position = 1 << 0,
        Rotation = 1 << 1,
        Scale = 1 << 2,
    }
}