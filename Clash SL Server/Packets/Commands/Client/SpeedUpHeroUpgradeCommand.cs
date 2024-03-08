using CSS.Helpers.Binary;
using CSS.Logic;

namespace CSS.Packets.Commands.Client
{
    // Packet 528
    internal class SpeedUpHeroUpgradeCommand : Command
    {
        public SpeedUpHeroUpgradeCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.m_vBuildingId = this.Reader.ReadInt32();
            this.m_vUnknown1 = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            var go = this.Device.Player.GameObjectManager.GetGameObjectByID(m_vBuildingId);

            if (go != null)
            {
                var b = (Building) go;
                var hbc = b.GetHeroBaseComponent();
                hbc?.SpeedUpUpgrade();
            }
        }

        internal int m_vBuildingId;
        internal int m_vUnknown1;
    }
}