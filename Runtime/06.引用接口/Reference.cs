using System;

namespace JFramework
{
    public struct Reference
    {
        public Type assetType { get; }
        public string assetPath { get; }
        public int cachedCount { get; }
        public int unusedCount { get; }
        public int dequeueCount { get; }
        public int enqueueCount { get; }

        public Reference(Type assetType, int cachedCount, int unusedCount, int dequeueCount, int enqueueCount)
        {
            assetPath = string.Empty;
            this.assetType = assetType;
            this.cachedCount = cachedCount;
            this.unusedCount = unusedCount;
            this.dequeueCount = dequeueCount;
            this.enqueueCount = enqueueCount;
        }

        public Reference(Type assetType, string assetPath, int cachedCount, int unusedCount, int dequeueCount, int enqueueCount)
        {
            this.assetType = assetType;
            this.assetPath = assetPath;
            this.cachedCount = cachedCount;
            this.unusedCount = unusedCount;
            this.dequeueCount = dequeueCount;
            this.enqueueCount = enqueueCount;
        }

        public override string ToString()
        {
            return Service.Text.Format("{0}\t\t{1}\t\t{2}\t\t{3}", unusedCount, cachedCount, dequeueCount, enqueueCount);
        }
    }
}