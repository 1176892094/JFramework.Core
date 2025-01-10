// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:42
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace JFramework.Net
{
    public class MemoryWriter : IDisposable
    {
        public readonly UTF8Encoding encoding = new UTF8Encoding(false, true);
        public byte[] buffer = new byte[1500];
        public int position;

        void IDisposable.Dispose()
        {
            Utility.Heap.Enqueue(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Write<T>(T value) where T : unmanaged
        {
            AddCapacity(position + sizeof(T));
            Utility.Unsafe.Write(buffer, value, ref position);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteNullable<T>(T? value) where T : unmanaged
        {
            if (!value.HasValue)
            {
                Write((byte)0);
                return;
            }

            Write((byte)1);
            Write(value.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Invoke<T>(T value)
        {
            Writer<T>.write?.Invoke(this, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            position = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MemoryWriter Pop()
        {
            var writer = Utility.Heap.Dequeue<MemoryWriter>();
            writer.Reset();
            return writer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Push(MemoryWriter writer)
        {
            Utility.Heap.Enqueue(writer);
        }

        public override string ToString()
        {
            return BitConverter.ToString(buffer, 0, position);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddCapacity(int length)
        {
            if (buffer.Length < length)
            {
                Array.Resize(ref buffer, Math.Max(length, buffer.Length * 2));
            }
        }

        public void WriteBytes(byte[] segment, int offset, int count)
        {
            AddCapacity(position + count);
            Buffer.BlockCopy(segment, offset, buffer, position, count);
            position += count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ArraySegment<byte>(MemoryWriter writer)
        {
            return new ArraySegment<byte>(writer.buffer, 0, writer.position);
        }
    }
}