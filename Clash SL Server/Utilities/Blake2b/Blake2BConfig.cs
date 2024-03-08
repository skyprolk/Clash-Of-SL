namespace CSS.Utilities.Blake2B
{
    public sealed class Blake2BConfig
    {
        public Blake2BConfig()
        {
            this.OutputSize = 24;
        }

        public byte[] Key
        {
            get;
            set;
        }

        public int OutputSize
        {
            get;
            set;
        }

        public int OutputSizeInBits
        {
            get
            {
                return this.OutputSize * 8;
            }

            set
            {
                this.OutputSize = value / 8;
            }
        }

        public byte[] Personalization
        {
            get;
            set;
        }

        public byte[] Salt
        {
            get;
            set;
        }
    }
}