
namespace UCS.Logic.JSONProperty
{
    using System.Collections.Generic;
    using UCS.Logic.JSONProperty.Item;

    internal class Inbox : List<Mail>
    {
        internal ClientAvatar Player;

        /// <summary>
        /// Initializes a new instance of the <see cref="Inbox"/> class.
        /// </summary>
        /// <param name="Player">The player.</param>
        internal Inbox(ClientAvatar Player)
        {
            this.Player = Player;
        }

        internal byte[] ToBytes
        {
            get
            {
                List<byte> Packet = new List<byte>();

                foreach (Mail Entry in this)
                {
                    Packet.AddRange(Entry.ToBytes);
                }

                return Packet.ToArray();
            }
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        internal void Update()
        {
            foreach (Mail Entry in this)
            {
                /* if (Entry.Outdated)
                {
                    this.Remove(Entry);
                } */
            }
        }
    }
}