// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:40
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;

namespace JFramework.Net
{
    internal class ReaderBatch
    {
        private readonly MemoryReader reader = new MemoryReader();
        private readonly Queue<MemoryWriter> writers = new Queue<MemoryWriter>();
        private double remoteTime;
        public int Count => writers.Count;


        public bool AddBatch(ArraySegment<byte> segment)
        {
            if (segment.Count < sizeof(double))
            {
                return false;
            }

            var writer = MemoryWriter.Pop();
            writer.WriteBytes(segment.Array, segment.Offset, segment.Count);
            if (writers.Count == 0)
            {
                reader.Reset(writer);
                remoteTime = reader.Read<double>();
            }

            writers.Enqueue(writer);
            return true;
        }

        public bool GetMessage(out ArraySegment<byte> segment, out double newTime)
        {
            newTime = 0;
            segment = default;
            if (writers.Count == 0)
            {
                return false;
            }

            if (reader.buffer.Count == 0)
            {
                return false;
            }

            if (reader.residue == 0)
            {
                var writer = writers.Dequeue();
                MemoryWriter.Push(writer);
                if (writers.Count > 0)
                {
                    writer = writers.Peek();
                    reader.Reset(writer);
                    remoteTime = reader.Read<double>();
                }
                else
                {
                    return false;
                }
            }

            newTime = remoteTime;
            if (reader.residue == 0)
            {
                return false;
            }

            var size = (int)NetworkCompress.Decompress(reader);

            if (reader.residue < size)
            {
                return false;
            }

            segment = reader.ReadArraySegment(size);
            return true;
        }
    }
}