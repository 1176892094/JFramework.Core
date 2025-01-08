// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-08 19:01:30
// # Recently: 2025-01-08 20:01:58
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace JFramework.Udp
{
    internal static class Common
    {
        private static readonly RNGCryptoServiceProvider cryptoRandom = new RNGCryptoServiceProvider();
        private static readonly byte[] cryptoRandomBuffer = new byte[4];

        internal static uint GenerateCookie()
        {
            cryptoRandom.GetBytes(cryptoRandomBuffer);
            return BitConverter.ToUInt32(cryptoRandomBuffer, 0);
        }

        internal static bool ParseReliable(byte value, out Reliable header)
        {
            if (Enum.IsDefined(typeof(Reliable), value))
            {
                header = (Reliable)value;
                return true;
            }

            header = Reliable.Ping;
            return false;
        }

        internal static bool ParseUnreliable(byte value, out Unreliable header)
        {
            if (Enum.IsDefined(typeof(Unreliable), value))
            {
                header = (Unreliable)value;
                return true;
            }

            header = Unreliable.Disconnect;
            return false;
        }

        internal static void SetBuffer(Socket socket, int buffer = 1024 * 1024 * 7)
        {
            socket.Blocking = false;
            var sendBuffer = socket.SendBufferSize;
            var receiveBuffer = socket.ReceiveBufferSize;
            try
            {
                socket.SendBufferSize = buffer;
                socket.ReceiveBufferSize = buffer;
            }
            catch (SocketException)
            {
                Log.Info($"发送缓存: {buffer} => {sendBuffer} : {sendBuffer / buffer:F}");
                Log.Info($"接收缓存: {buffer} => {receiveBuffer} : {receiveBuffer / buffer:F}");
            }
        }
    }
}