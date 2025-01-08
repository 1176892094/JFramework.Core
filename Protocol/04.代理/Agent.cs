// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-11-29 13:11:20
// # Recently: 2024-12-22 20:12:11
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace JFramework.Udp
{
    public abstract class Agent
    {
        private readonly byte[] kcpSendBuffer;
        private readonly byte[] rawSendBuffer;
        private readonly byte[] receiveBuffer;
        private readonly int unreliableSize;
        private readonly Stopwatch watch = new Stopwatch();
        protected uint cookie;
        private Kcp kcp;
        private uint pingTime;
        private uint receiveTime;
        protected Status status;
        private uint timeout;

        protected Agent(Setting setting, uint cookie)
        {
            Reset(setting);
            this.cookie = cookie;
            unreliableSize = Common.UnreliableSize(setting.MaxUnit);
            var reliableSize = Common.ReliableSize(setting.MaxUnit, setting.ReceiveWindow);
            rawSendBuffer = new byte[setting.MaxUnit];
            receiveBuffer = new byte[1 + reliableSize];
            kcpSendBuffer = new byte[1 + reliableSize];
            status = Status.Disconnect;
        }

        protected void Reset(Setting config)
        {
            cookie = 0;
            pingTime = 0;
            receiveTime = 0;
            status = Status.Disconnect;
            watch.Restart();

            kcp = new Kcp(0, SendReliable);
            kcp.SetMtu((uint)config.MaxUnit - Common.METADATA_SIZE);
            kcp.SetWindowSize(config.SendWindow, config.ReceiveWindow);
            kcp.SetNoDelay(config.NoDelay ? 1U : 0U, config.Interval, config.FastResend, !config.Congestion);
            kcp.dead_link = config.DeadLink;
            timeout = config.Timeout;
        }

        private bool TryReceive(out Reliable header, out ArraySegment<byte> message)
        {
            message = default;
            header = Reliable.Ping;
            var size = kcp.PeekSize();
            if (size <= 0)
            {
                return false;
            }

            if (size > receiveBuffer.Length)
            {
                Logger(Error.InvalidReceive, $"{GetType()}: 网络消息长度溢出 {receiveBuffer.Length} < {size}。");
                Disconnect();
                return false;
            }

            if (kcp.Receive(receiveBuffer, size) < 0)
            {
                Logger(Error.InvalidReceive, $"{GetType()}: 接收网络消息失败。");
                Disconnect();
                return false;
            }

            if (!Common.ParseReliable(receiveBuffer[0], out header))
            {
                Logger(Error.InvalidReceive, $"{GetType()}: 未知的网络消息头部 {header}");
                Disconnect();
                return false;
            }

            message = new ArraySegment<byte>(receiveBuffer, 1, size - 1);
            receiveTime = (uint)watch.ElapsedMilliseconds;
            return true;
        }

        protected void Input(int channel, ArraySegment<byte> segment)
        {
            if (channel == Channel.Reliable)
            {
                if (kcp.Input(segment.Array, segment.Offset, segment.Count) != 0)
                {
                    Log.Warn($"{GetType()}: 发送可靠消息失败。消息大小：{segment.Count - 1}");
                }
            }
            else if (channel == Channel.Unreliable)
            {
                if (segment.Count < 1) return;
                var headerByte = segment.Array[segment.Offset];
                if (!Common.ParseUnreliable(headerByte, out var header))
                {
                    Logger(Error.InvalidReceive, $"{GetType()}: 未知的网络消息头部 {header}");
                    Disconnect();
                    return;
                }

                if (header == Unreliable.Data)
                {
                    if (status == Status.Connected)
                    {
                        segment = new ArraySegment<byte>(segment.Array, segment.Offset + 1, segment.Count - 1);
                        Receive(segment, Channel.Unreliable);
                        receiveTime = (uint)watch.ElapsedMilliseconds;
                    }
                }
                else if (header == Unreliable.Disconnect)
                {
                    Log.Info($"{GetType()}: 接收到断开连接的消息");
                    Disconnect();
                }
            }
        }

        private void SendReliable(byte[] data, int length)
        {
            rawSendBuffer[0] = Channel.Reliable;
            Utils.Encode32U(rawSendBuffer, 1, cookie);
            Buffer.BlockCopy(data, 0, rawSendBuffer, 1 + 4, length);
            var segment = new ArraySegment<byte>(rawSendBuffer, 0, length + 1 + 4);
            Send(segment);
        }

        protected void SendReliable(Reliable header, ArraySegment<byte> segment)
        {
            if (segment.Count > kcpSendBuffer.Length - 1)
            {
                Logger(Error.InvalidSend, $"{GetType()}: 发送可靠消息失败。消息大小：{segment.Count}");
                return;
            }

            kcpSendBuffer[0] = (byte)header;
            if (segment.Count > 0)
            {
                Buffer.BlockCopy(segment.Array, segment.Offset, kcpSendBuffer, 1, segment.Count);
            }

            if (kcp.Send(kcpSendBuffer, 0, 1 + segment.Count) < 0)
            {
                Logger(Error.InvalidSend, $"{GetType()}: 发送可靠消息失败。消息大小：{segment.Count}。");
            }
        }

        private void SendUnreliable(Unreliable header, ArraySegment<byte> segment)
        {
            if (segment.Count > unreliableSize)
            {
                Log.Error($"{GetType()}: 发送不可靠消息失败。消息大小：{segment.Count}");
                return;
            }

            rawSendBuffer[0] = Channel.Unreliable;
            Utils.Encode32U(rawSendBuffer, 1, cookie);
            rawSendBuffer[5] = (byte)header;
            if (segment.Count > 0)
            {
                Buffer.BlockCopy(segment.Array, segment.Offset, rawSendBuffer, 1 + 4 + 1, segment.Count);
            }

            Send(new ArraySegment<byte>(rawSendBuffer, 0, segment.Count + 1 + 4 + 1));
        }

        public void SendData(ArraySegment<byte> data, int channel)
        {
            if (data.Count == 0)
            {
                Logger(Error.InvalidSend, $"{GetType()} 尝试发送空消息。");
                Disconnect();
                return;
            }

            switch (channel)
            {
                case Channel.Reliable:
                    SendReliable(Reliable.Data, data);
                    break;
                case Channel.Unreliable:
                    SendUnreliable(Unreliable.Data, data);
                    break;
                default:
                    Log.Warn("试图在未知的传输通道传输消息!");
                    break;
            }
        }

        public void Disconnect()
        {
            if (status == Status.Disconnect) return;
            try
            {
                for (var i = 0; i < 5; ++i)
                {
                    SendUnreliable(Unreliable.Disconnect, default);
                }
            }
            finally
            {
                status = Status.Disconnect;
                Disconnected();
            }
        }

        public virtual void EarlyUpdate()
        {
            if (kcp.state == -1)
            {
                Logger(Error.Timeout, $"{GetType()}: 网络消息被重传了 {kcp.dead_link} 次而没有得到确认！");
                Disconnect();
            }

            var time = (uint)watch.ElapsedMilliseconds;
            if (time >= receiveTime + timeout)
            {
                Logger(Error.Timeout, $"{GetType()}: 在 {timeout}ms 内没有收到任何消息后的连接超时！");
                Disconnect();
            }

            var total = kcp.receiveQueue.Count + kcp.sendQueue.Count + kcp.receiveBuffer.Count + kcp.sendBuffer.Count;
            if (total >= 10000)
            {
                Logger(Error.Congestion, $"{GetType()}: 断开连接，因为它处理数据的速度不够快！");
                kcp.sendQueue.Clear();
                Disconnect();
            }

            if (time >= pingTime + Common.PING_INTERVAL)
            {
                SendReliable(Reliable.Ping, default);
                pingTime = time;
            }

            try
            {
                if (status == Status.Connect)
                {
                    if (TryReceive(out var header, out _))
                    {
                        if (header == Reliable.Connect)
                        {
                            status = Status.Connected;
                            Connected();
                        }
                        else if (header == Reliable.Data)
                        {
                            Logger(Error.InvalidReceive, $"{GetType()}: 收到未通过验证的网络消息。消息类型：{header}");
                            Disconnect();
                        }
                    }
                }
                else if (status == Status.Connected)
                {
                    while (TryReceive(out var header, out var segment))
                    {
                        if (header == Reliable.Connect)
                        {
                            Log.Warn($"{GetType()}: 收到无效的网络消息。消息类型：{header}");
                            Disconnect();
                        }
                        else if (header == Reliable.Data)
                        {
                            if (segment.Count == 0)
                            {
                                Logger(Error.InvalidReceive, $"{GetType()}: 收到无效的网络消息。消息类型：{header}");
                                Disconnect();
                                return;
                            }

                            Receive(segment, Channel.Reliable);
                        }
                    }
                }
            }
            catch (SocketException e)
            {
                Logger(Error.ConnectionClosed, $"{GetType()}: 网络发生异常，断开连接。\n{e}");
                Disconnect();
            }
            catch (ObjectDisposedException e)
            {
                Logger(Error.ConnectionClosed, $"{GetType()}: 网络发生异常，断开连接。\n{e}");
                Disconnect();
            }
            catch (Exception e)
            {
                Logger(Error.Unexpected, $"{GetType()}:网络发生异常，断开连接。\n{e}");
                Disconnect();
            }
        }

        public virtual void AfterUpdate()
        {
            try
            {
                if (status != Status.Disconnect)
                {
                    kcp.Update((uint)watch.ElapsedMilliseconds);
                }
            }
            catch (SocketException e)
            {
                Logger(Error.ConnectionClosed, $"{GetType()}: 网络发生异常，断开连接。\n{e}");
                Disconnect();
            }
            catch (ObjectDisposedException e)
            {
                Logger(Error.ConnectionClosed, $"{GetType()}: 网络发生异常，断开连接。\n{e}");
                Disconnect();
            }
            catch (Exception e)
            {
                Logger(Error.Unexpected, $"{GetType()}: 网络发生异常，断开连接。\n{e}");
                Disconnect();
            }
        }

        protected abstract void Connected();
        protected abstract void Send(ArraySegment<byte> segment);
        protected abstract void Receive(ArraySegment<byte> message, int channel);
        protected abstract void Logger(Error error, string message);
        protected abstract void Disconnected();
    }
}