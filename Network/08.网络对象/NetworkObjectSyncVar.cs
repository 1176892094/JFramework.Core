// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-21 23:12:45
// # Recently: 2024-12-22 22:12:02
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************


using System;
using System.Collections.Generic;

namespace JFramework.Net
{
    public sealed partial class NetworkObject
    {
        internal void ServerSerialize(bool status, MemoryWriter owner, MemoryWriter observer)
        {
            var components = entities;
            var ownerPair = ServerDirtyMasks(status);
            var ownerMask = ownerPair.Key;
            var observerMask = ownerPair.Value;

            if (ownerMask != 0)
            {
                NetworkLength.Compress(owner, ownerMask);
            }

            if (observerMask != 0)
            {
                NetworkLength.Compress(observer, observerMask);
            }

            if ((ownerMask | observerMask) != 0)
            {
                for (var i = 0; i < components.Length; ++i)
                {
                    var component = components[i];
                    var ownerDirty = IsDirty(ownerMask, i);
                    var observersDirty = IsDirty(observerMask, i);
                    if (ownerDirty || observersDirty)
                    {
                        using var writer = MemoryWriter.Pop();
                        component.Serialize(writer, status);
                        ArraySegment<byte> segment = writer;
                        if (ownerDirty)
                        {
                            owner.WriteBytes(segment.Array, segment.Offset, segment.Count);
                        }

                        if (observersDirty)
                        {
                            observer.WriteBytes(segment.Array, segment.Offset, segment.Count);
                        }
                    }
                }
            }
        }

        internal void ClientSerialize(MemoryWriter writer)
        {
            var components = entities;
            var dirtyMask = ClientDirtyMask();
            if (dirtyMask != 0)
            {
                NetworkLength.Compress(writer, dirtyMask);
                for (var i = 0; i < components.Length; ++i)
                {
                    var component = components[i];

                    if (IsDirty(dirtyMask, i))
                    {
                        component.Serialize(writer, false);
                    }
                }
            }
        }

        internal bool ServerDeserialize(MemoryReader reader)
        {
            var components = entities;
            var mask = NetworkLength.Decompress(reader);

            for (var i = 0; i < components.Length; ++i)
            {
                if (IsDirty(mask, i))
                {
                    var component = components[i];

                    if (component.syncDirection == SyncMode.Client)
                    {
                        if (!component.Deserialize(reader, false))
                        {
                            return false;
                        }

                        component.SetSyncVarDirty(ulong.MaxValue);
                    }
                }
            }

            return true;
        }

        internal void ClientDeserialize(MemoryReader reader, bool status)
        {
            var components = entities;
            var mask = NetworkLength.Decompress(reader);

            for (var i = 0; i < components.Length; ++i)
            {
                if (IsDirty(mask, i))
                {
                    var component = components[i];
                    component.Deserialize(reader, status);
                }
            }
        }

        private KeyValuePair<ulong, ulong> ServerDirtyMasks(bool status)
        {
            ulong ownerMask = 0;
            ulong observerMask = 0;

            var components = entities;
            for (var i = 0; i < components.Length; ++i)
            {
                var component = components[i];
                var dirty = component.IsDirty();
                ulong mask = 1U << i;
                if (status || (component.syncDirection == SyncMode.Server && dirty))
                {
                    ownerMask |= mask;
                }

                if (status || dirty)
                {
                    observerMask |= mask;
                }
            }

            return new KeyValuePair<ulong, ulong>(ownerMask, observerMask);
        }

        private ulong ClientDirtyMask()
        {
            ulong mask = 0;
            var components = entities;
            for (var i = 0; i < components.Length; ++i)
            {
                var component = components[i];
                if ((entityMode & EntityMode.Owner) != 0 && component.syncDirection == SyncMode.Client)
                {
                    if (component.IsDirty()) mask |= 1U << i;
                }
            }

            return mask;
        }
    }
}