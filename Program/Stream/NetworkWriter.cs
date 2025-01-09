// *********************************************************************************
// # Project: JFramework.Lobby
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-08-28 20:08:49
// # Recently: 2024-12-23 00:12:04
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace JFramework.Net
{
    [Serializable]
    public class NetworkWriter : IDisposable
    {
        internal readonly UTF8Encoding encoding = new UTF8Encoding(false, true);

        internal byte[] buffer = new byte[1500];

        public int position;

        void IDisposable.Dispose()
        {
            NetworkPool<NetworkWriter>.Enqueue(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe void Write<T>(T value) where T : unmanaged
        {
            AddCapacity(position + sizeof(T));
            fixed (byte* ptr = &buffer[position])
            {
                *(T*)ptr = value;
            }

            position += sizeof(T);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Reset()
        {
            position = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NetworkWriter Pop()
        {
            var writer = NetworkPool<NetworkWriter>.Dequeue();
            writer.Reset();
            return writer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Push(NetworkWriter writer)
        {
            NetworkPool<NetworkWriter>.Enqueue(writer);
        }

        public override string ToString()
        {
            return BitConverter.ToString(buffer, 0, position);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void AddCapacity(int length)
        {
            if (buffer.Length < length)
            {
                Array.Resize(ref buffer, Math.Max(length, buffer.Length * 2));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ArraySegment<byte>(NetworkWriter writer)
        {
            return new ArraySegment<byte>(writer.buffer, 0, writer.position);
        }

        public void WriteBytes(byte[] segment, int offset, int count)
        {
            AddCapacity(position + count);
            Buffer.BlockCopy(segment, offset, buffer, position, count);
            position += count;
        }
    }
}