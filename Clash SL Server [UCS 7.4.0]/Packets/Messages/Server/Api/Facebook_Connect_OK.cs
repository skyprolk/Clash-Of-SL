namespace CSS.Packets.Messages.Server
{
    internal class Facebook_Connect_OK : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Facebook_Connect_OK"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public Facebook_Connect_OK(Device Device) : base(Device)
        {
            this.Identifier = 24201;
        }

        /// <summary>
        /// Encodes this message.
        /// </summary>
        internal override void Encode()
        {
            this.Data.Add(1);
        }
    }
}
