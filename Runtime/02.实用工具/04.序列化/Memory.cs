// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:30
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Runtime.CompilerServices;

namespace JFramework
{
    public static partial class Service
    {
        public static class Memory
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe void Write<T>(byte[] buffer, T value, ref int position) where T : unmanaged
            {
                fixed (byte* destination = &buffer[position])
                {
                    var source = stackalloc T[1] { value };
                    MemCpy(destination, source, sizeof(T));
                }

                position += sizeof(T);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe T Read<T>(byte[] buffer, int offset, ref int position) where T : unmanaged
            {
                T value;
                fixed (byte* source = &buffer[offset + position])
                {
                    var destination = stackalloc T[1];
                    MemCpy(destination, source, sizeof(T));
                    value = destination[0];
                }

                position += sizeof(T);
                return value;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe void MemCpy(void* destination, void* source, int size)
            {
                var dest = (byte*)destination;
                var src = (byte*)source;

                while (size >= 8)
                {
                    *(long*)dest = *(long*)src;
                    dest += 8;
                    src += 8;
                    size -= 8;
                }

                while (size >= 4)
                {
                    *(int*)dest = *(int*)src;
                    dest += 4;
                    src += 4;
                    size -= 4;
                }

                while (size >= 2)
                {
                    *(short*)dest = *(short*)src;
                    dest += 2;
                    src += 2;
                    size -= 2;
                }

                while (size > 0)
                {
                    *dest = *src;
                    dest++;
                    src++;
                    size--;
                }
            }
        }
    }
}