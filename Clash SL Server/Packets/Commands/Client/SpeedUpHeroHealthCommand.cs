using CSS.Helpers.Binary;

namespace CSS.Packets.Commands.Client
{
    // Packet 530
    internal class SpeedUpHeroHealthCommand : Command
    {
        //int m_vBuildingId;

        public SpeedUpHeroHealthCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
            /*
            m_vBuildingId = br.ReadInt32();
            br.ReadInt32();
            */
        }

        internal override void Decode()
        {
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
        }
    }
}