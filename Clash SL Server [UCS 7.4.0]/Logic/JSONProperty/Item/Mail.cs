namespace UCS.Logic.JSONProperty.Item
{
    using System.Collections.Generic;

    internal class Mail
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mail"/> class.
        /// </summary>
        internal Mail()
        {
            // Mail.
        }

        internal byte[] ToBytes
        {
            get
            {
                List<byte> Packet = new List<byte>();



                return Packet.ToArray();
            }
        }
    }
}
