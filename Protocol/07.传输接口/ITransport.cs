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
using JFramework.Udp;

namespace JFramework.Common
{
    public interface ITransport : IAddress
    {
        /// <summary>
        /// 客户端连接事件
        /// </summary>
        Action OnClientConnect { get; set; }

        /// <summary>
        /// 客户端断开事件
        /// </summary>
        Action OnClientDisconnect { get; set; }

        /// <summary>
        /// 客户端错误事件
        /// </summary>
        Action<int, string> OnClientError { get; set; }

        /// <summary>
        /// 客户端接收事件
        /// </summary>
        Action<ArraySegment<byte>, int> OnClientReceive { get; set; }

        /// <summary>
        /// 客户端连接到服务器的事件
        /// </summary>
        Action<int> OnServerConnect { get; set; }

        /// <summary>
        /// 客户端从服务器断开的事件
        /// </summary>
        Action<int> OnServerDisconnect { get; set; }

        /// <summary>
        /// 服务器错误事件
        /// </summary>
        Action<int, int, string> OnServerError { get; set; }

        /// <summary>
        /// 服务器接收客户端消息的事件
        /// </summary>
        Action<int, ArraySegment<byte>, int> OnServerReceive { get; set; }

        /// <summary>
        /// 获取最大网络消息大小
        /// </summary>
        /// <param name="channel">传输通道</param>
        /// <returns></returns>
        int MessageSize(int channel);

        /// <summary>
        /// 服务器传输信息给客户端
        /// </summary>
        void SendToClient(int clientId, ArraySegment<byte> segment, int channel = Channel.Reliable);

        /// <summary>
        /// 客户端向服务器传输信息
        /// </summary>
        /// <param name="segment">传入发送的数据</param>
        /// <param name="channel">传入通道</param>
        void SendToServer(ArraySegment<byte> segment, int channel = Channel.Reliable);

        /// <summary>
        /// 当服务器连接
        /// </summary>
        void StartServer();

        /// <summary>
        /// 当服务器停止
        /// </summary>
        void StopServer();

        /// <summary>
        /// 服务器断开指定客户端连接
        /// </summary>
        /// <param name="clientId">传入要断开的客户端Id</param>
        void StopClient(int clientId);

        /// <summary>
        /// 根据地址和端口进行连接
        /// </summary>
        void StartClient();

        /// <summary>
        /// 根据Uri连接
        /// </summary>
        /// <param name="uri">传入Uri</param>
        void StartClient(Uri uri);

        /// <summary>
        /// 客户端断开连接
        /// </summary>
        void StopClient();

        /// <summary>
        /// 客户端Update之前
        /// </summary>
        void ClientEarlyUpdate();

        /// <summary>
        /// 客户端Update之后
        /// </summary>
        void ClientAfterUpdate();

        /// <summary>
        /// 服务器Update之前
        /// </summary>
        void ServerEarlyUpdate();

        /// <summary>
        /// 服务器Update之后
        /// </summary>
        void ServerAfterUpdate();
    }
}