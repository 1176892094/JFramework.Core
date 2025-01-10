// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-10 17:01:08
// # Recently: 2025-01-10 17:01:09
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework
{
    public static partial class Service
    {
        public static class Hash<T>
        {
            public static readonly ushort Id = (ushort)Hash.Id(typeof(T).FullName);
        }

        public static class Hash
        {
            private static readonly byte[] buffer = new byte[4];

            public static uint Id()
            {
                Random.NextBytes(buffer);
                return BitConverter.ToUInt32(buffer, 0);
            }

            public static uint Id(string name)
            {
                var result = 23U;
                unchecked
                {
                    foreach (var c in name)
                    {
                        result = result * 31 + c;
                    }

                    return result;
                }
            }
        }
    }
}