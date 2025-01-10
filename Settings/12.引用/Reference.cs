using System;

namespace JFramework
{
    public struct Reference
    {
        public Type assetType { get; }
        public string assetPath { get; }
        public int caches { get; }
        public int unuseds { get; }
        public int dequeue { get; }
        public int enqueue { get; }

        public Reference(Type assetType, int caches, int unuseds, int dequeue, int enqueue)
        {
            assetPath = string.Empty;
            this.assetType = assetType;
            this.caches = caches;
            this.unuseds = unuseds;
            this.dequeue = dequeue;
            this.enqueue = enqueue;
        }

        public Reference(Type assetType, string assetPath, int caches, int unuseds, int dequeue, int enqueue)
        {
            this.assetType = assetType;
            this.assetPath = assetPath;
            this.caches = caches;
            this.unuseds = unuseds;
            this.dequeue = dequeue;
            this.enqueue = enqueue;
        }

        public override string ToString()
        {
            return Utility.Text.Format("{0}\t\t{1}\t\t{2}\t\t{3}", unuseds, caches, dequeue, enqueue);
        }
    }
}