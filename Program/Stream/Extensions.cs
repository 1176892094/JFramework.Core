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
using System.IO;

namespace JFramework.Net
{
    public static class Extensions
    {
        public static void WriteByte(this NetworkWriter writer, byte value)
        {
            writer.Write(value);
        }

        public static byte ReadByte(this NetworkReader reader)
        {
            return reader.Read<byte>();
        }

        public static void WriteBool(this NetworkWriter writer, bool value)
        {
            writer.Write((byte)(value ? 1 : 0));
        }

        public static bool ReadBool(this NetworkReader reader)
        {
            return reader.Read<byte>() != 0;
        }

        public static void WriteUShort(this NetworkWriter writer, ushort value)
        {
            writer.Write(value);
        }

        public static ushort ReadUShort(this NetworkReader reader)
        {
            return reader.Read<ushort>();
        }

        public static void WriteInt(this NetworkWriter writer, int value)
        {
            writer.Write(value);
        }

        public static int ReadInt(this NetworkReader reader)
        {
            return reader.Read<int>();
        }

        public static void WriteUInt(this NetworkWriter writer, uint value)
        {
            writer.Write(value);
        }

        public static uint ReadUInt(this NetworkReader reader)
        {
            return reader.Read<uint>();
        }

        public static void WriteString(this NetworkWriter writer, string value)
        {
            if (value == null)
            {
                writer.WriteUShort(0);
                return;
            }

            writer.AddCapacity(writer.position + 2 + writer.encoding.GetMaxByteCount(value.Length));
            var count = writer.encoding.GetBytes(value, 0, value.Length, writer.buffer, writer.position + 2);
            if (count > ushort.MaxValue - 1)
            {
                throw new EndOfStreamException("写入字符串过长!");
            }

            writer.WriteUShort(checked((ushort)(count + 1)));
            writer.position += count;
        }

        public static string ReadString(this NetworkReader reader)
        {
            var count = reader.ReadUShort();
            if (count == 0)
            {
                return null;
            }

            count = (ushort)(count - 1);
            if (count > ushort.MaxValue - 1)
            {
                throw new EndOfStreamException("读取字符串过长!");
            }

            var segment = reader.ReadArraySegment(count);
            return reader.encoding.GetString(segment.Array, segment.Offset, segment.Count);
        }

        public static void WriteArraySegment(this NetworkWriter writer, ArraySegment<byte> value)
        {
            if (value == default)
            {
                writer.WriteUInt(0);
                return;
            }

            writer.WriteUInt(checked((uint)value.Count) + 1);
            writer.WriteBytes(value.Array, value.Offset, value.Count);
        }


        public static ArraySegment<byte> ReadArraySegment(this NetworkReader reader)
        {
            var count = reader.ReadUInt();
            return count == 0 ? default : reader.ReadArraySegment(checked((int)(count - 1)));
        }
    }
}