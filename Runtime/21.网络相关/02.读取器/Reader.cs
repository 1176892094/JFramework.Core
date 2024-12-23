// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-21 23:12:15
// # Recently: 2024-12-22 20:12:22
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace JFramework.Net
{
    public class MemoryReader : IDisposable
    {
        public readonly UTF8Encoding encoding = new UTF8Encoding(false, true);
        public ArraySegment<byte> buffer;
        public int position;

        public int residue => buffer.Count - position;

        void IDisposable.Dispose()
        {
            Service.Heap.Enqueue(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Read<T>() where T : unmanaged
        {
            return Service.Memory.Read<T>(buffer.Array, buffer.Offset, ref position);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T? ReadNullable<T>() where T : unmanaged
        {
            return Read<byte>() != 0 ? Service.Memory.Read<T>(buffer.Array, buffer.Offset, ref position) : default(T?);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Invoke<T>()
        {
            return Reader<T>.read != null ? Reader<T>.read.Invoke(this) : default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset(ArraySegment<byte> segment)
        {
            buffer = segment;
            position = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MemoryReader Pop(ArraySegment<byte> segment)
        {
            var reader = Service.Heap.Dequeue<MemoryReader>();
            reader.Reset(segment);
            return reader;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Push(MemoryReader reader)
        {
            Service.Heap.Enqueue(reader);
        }

        public override string ToString()
        {
            return BitConverter.ToString(buffer.Array, buffer.Offset, buffer.Count);
        }

        public ArraySegment<byte> ReadArraySegment(int count)
        {
            if (residue < count)
            {
                throw new OverflowException("读取器剩余容量不够!");
            }

            var segment = new ArraySegment<byte>(buffer.Array, buffer.Offset + position, count);
            position += count;
            return segment;
        }
    }
}