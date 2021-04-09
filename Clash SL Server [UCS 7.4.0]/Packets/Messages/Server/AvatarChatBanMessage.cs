using CSS.Helpers.List;

namespace CSS.Packets.Messages.Server
{
    internal class AvatarChatBanMessage : Message
    {
        public int m_vCode = 86400;

        public AvatarChatBanMessage(Device client) : base(client)
        {
            this.Identifier = 20118;
        }

        internal override void Encode()
        {
            this.Data.AddInt(m_vCode);
        }

        public void SetBanPeriod(int code)
        {
            m_vCode = code;
        }
    }
}
