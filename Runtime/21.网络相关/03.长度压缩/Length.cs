// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-24 02:12:57
// # Recently: 2025-01-08 17:01:24
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

namespace JFramework.Net
{
    public static class Length
    {
        public static int GetLength(ulong length)
        {
            if (length == 0)
            {
                return 1;
            }

            var result = 0;
            while (length > 0)
            {
                result++;
                length >>= 7;
            }

            return result;
        }

        public static void Compress(MemoryWriter writer, ulong length)
        {
            while (length >= 0x80)
            {
                writer.Write((byte)((length & 0x7F) | 0x80));
                length >>= 7;
            }

            writer.Write((byte)length);
        }

        public static ulong Decompress(MemoryReader reader)
        {
            var shift = 0;
            var length = 0UL;
            while (true)
            {
                var bit = reader.Read<byte>();
                length |= (ulong)(bit & 0x7F) << shift;
                if ((bit & 0x80) == 0)
                {
                    break;
                }

                shift += 7;
            }

            return length;
        }
    }
}