namespace CSS.Utilities.Blake2B
{
    internal static class Blake2Builder
    {
        private static readonly Blake2BTreeConfig SequentialTreeConfig = new Blake2BTreeConfig
        {
            IntermediateHashSize = 0, LeafSize = 0, FanOut = 1, MaxHeight = 1
        };

        public static ulong[] ConfigB(Blake2BConfig config, Blake2BTreeConfig treeConfig)
        {
            bool isSequential = treeConfig == null;
            if (isSequential)
            {
                treeConfig = Blake2Builder.SequentialTreeConfig;
            }

            ulong[] rawConfig = new ulong[8];

            rawConfig[0] |= (uint) config.OutputSize;

            if (config.Key != null)
            {
                rawConfig[0] |= (uint) config.Key.Length << 8;
            }

            rawConfig[0] |= (uint) treeConfig.FanOut << 16;
            rawConfig[0] |= (uint) treeConfig.MaxHeight << 24;
            rawConfig[0] |= (ulong) (uint) treeConfig.LeafSize << 32;
            rawConfig[2] |= (uint) treeConfig.IntermediateHashSize << 8;

            if (config.Salt != null)
            {
                rawConfig[4] = Blake2BCore.BytesToUInt64(config.Salt, 0);
                rawConfig[5] = Blake2BCore.BytesToUInt64(config.Salt, 8);
            }

            if (config.Personalization != null)
            {
                rawConfig[6] = Blake2BCore.BytesToUInt64(config.Personalization, 0);
                rawConfig[7] = Blake2BCore.BytesToUInt64(config.Personalization, 8);
            }

            return rawConfig;
        }

        public static void ConfigBSetNode(ulong[] rawConfig, byte depth, ulong nodeOffset)
        {
            rawConfig[1] = nodeOffset;
            rawConfig[2] = (rawConfig[2] & ~0xFFul) | depth;
        }
    }
}