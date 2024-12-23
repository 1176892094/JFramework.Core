// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2024-12-24 01:12:40
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.IO;

namespace JFramework
{
    public static partial class Service
    {
        public static class Xor
        {
            private const int LENGTH = 16;

            public static byte[] Encrypt(byte[] data)
            {
                var key = new byte[LENGTH];
                Random.NextBytes(key);

                using var ms = new MemoryStream();
                ms.Write(key, 0, key.Length);

                var buffer = new byte[1024];

                for (var i = 0; i < data.Length; i += buffer.Length)
                {
                    var length = Math.Min(buffer.Length, data.Length - i);
                    for (var j = 0; j < length; j++)
                    {
                        buffer[j] = (byte)(data[i + j] ^ key[(i + j) % key.Length]);
                    }

                    ms.Write(buffer, 0, length);
                }

                return ms.ToArray();
            }

            public static byte[] Decrypt(byte[] data)
            {
                var key = new byte[LENGTH];
                Buffer.BlockCopy(data, 0, key, 0, key.Length);

                using var ms = new MemoryStream();

                var buffer = new byte[1024];

                for (var i = LENGTH; i < data.Length; i += buffer.Length)
                {
                    var length = Math.Min(buffer.Length, data.Length - i);
                    for (var j = 0; j < length; j++)
                    {
                        buffer[j] = (byte)(data[i + j] ^ key[(i + j - LENGTH) % key.Length]);
                    }

                    ms.Write(buffer, 0, length);
                }

                return ms.ToArray();
            }
        }
    }
}