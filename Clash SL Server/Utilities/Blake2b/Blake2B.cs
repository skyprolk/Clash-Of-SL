namespace UCS.Utilities.Blake2b
{
    public static class Blake2B
    {
        public static Hasher Create()
        {
            return Create(new Blake2BConfig());
        }

        public static Hasher Create(Blake2BConfig config)
        {
            return new Blake2BHasher(config);
        }

        public static byte[] ComputeHash(byte[] data, int start, int count)
        {
            return ComputeHash(data, start, count, null);
        }

        public static byte[] ComputeHash(byte[] data)
        {
            return ComputeHash(data, 0, data.Length, null);
        }

        public static byte[] ComputeHash(byte[] data, Blake2BConfig config)
        {
            return ComputeHash(data, 0, data.Length, config);
        }

        public static byte[] ComputeHash(byte[] data, int start, int count, Blake2BConfig config)
        {
            var hasher = Create(config);
            hasher.Update(data, start, count);
            return hasher.Finish();
        }
    }
}