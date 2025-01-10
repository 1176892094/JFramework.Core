// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-09 16:01:50
// # Recently: 2025-01-10 16:01:56
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Runtime.CompilerServices;

namespace JFramework
{
    public static partial class Utility
    {
        public static class Unsafe
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe byte[] Write<T>(T value) where T : unmanaged
            {
                var buffer = new byte[sizeof(T)];
                fixed (byte* ptr = buffer)
                {
                    *(T*)ptr = value;
                }

                return buffer;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe T Read<T>(byte[] buffer) where T : unmanaged
            {
                T value;
                fixed (byte* ptr = buffer)
                {
                    value = *(T*)ptr;
                }

                return value;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe void Write<T>(byte[] buffer, T value, ref int position) where T : unmanaged
            {
                fixed (byte* ptr = &buffer[position])
                {
                    *(T*)ptr = value;
                }

                position += sizeof(T);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe T Read<T>(byte[] buffer, int offset, ref int position) where T : unmanaged
            {
                T value;
                fixed (byte* ptr = &buffer[offset + position])
                {
                    value = *(T*)ptr;
                }

                position += sizeof(T);
                return value;
            }
        }
    }
}