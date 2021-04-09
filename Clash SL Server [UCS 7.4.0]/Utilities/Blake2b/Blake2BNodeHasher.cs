namespace UCS.Utilities.Blake2b
{
    /*public class Blake2BNodeHasher : NodeHasher
	{
		ulong[] rawConfig;
		byte[] key;
		Blake2BCore core = new Blake2BCore();

		public override void Init(int depth, long nodeOffset)
		{
			throw new NotImplementedException();
		}

		public override byte[] Finish(bool isEndOfLayer)
		{
			throw new NotImplementedException();
		}

		public override void Update(byte[] data, int start, int count)
		{
			throw new NotImplementedException();
		}

		public Blake2BNodeHasher(Blake2BConfig config, Blake2BTreeConfig treeConfig)
		{
			if (config == null)
				config = DefaultConfig;
			rawConfig = Blake2IvBuilder.ConfigB(config, null);
			if (config.Key != null && config.Key.Length != 0)
			{
				key = new byte[128];
				Array.Copy(config.Key, key, config.Key.Length);
			}
			Init(0, 0);
		}
	}*/
}