using CSS.Logic.AvatarStreamEntry;

namespace CSS.Packets.Messages.Server
{
    // Packet 24412
    internal class AvatarStreamEntryMessage : Message
    {
        AvatarStreamEntry m_vAvatarStreamEntry;

        public AvatarStreamEntryMessage(Device client) : base(client)
        {
            this.Identifier = 24412;
        }

        internal override void Encode()
        { 
            this.Data.AddRange(m_vAvatarStreamEntry.Encode());
        }

        public void SetAvatarStreamEntry(AvatarStreamEntry entry)
        {
            m_vAvatarStreamEntry = entry;
        }
    }
}