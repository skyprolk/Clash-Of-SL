using CSS.Helpers.Binary;
using CSS.Logic;

namespace CSS.Packets.Commands.Client
{
    // Packet 531
    internal class CancelHeroUpgradeCommand : Command
    {
        internal int m_vBuildingId;

        public CancelHeroUpgradeCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.m_vBuildingId = this.Reader.ReadInt32();
            this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            var go = this.Device.Player.GameObjectManager.GetGameObjectByID(m_vBuildingId);
            if (go?.ClassId == 0)
            {
                var b = (Building) go;
                var hbc = b.GetHeroBaseComponent();
                hbc?.CancelUpgrade();
            }
        }
    }
}