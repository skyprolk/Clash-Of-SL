using CSS.Helpers.Binary;
using CSS.Logic;

namespace CSS.Packets.Commands.Client
{
    // Packet 504
    internal class SpeedUpConstructionCommand : Command
    {
        internal int m_vBuildingId;

        public SpeedUpConstructionCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.m_vBuildingId = this.Reader.ReadInt32();
            this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            var go = this.Device.Player.GameObjectManager.GetGameObjectByID(this.m_vBuildingId);
            if (go != null)
            {
                if (go.ClassId == 0 || go.ClassId == 4)
                {
                    ((ConstructionItem) go).SpeedUpConstruction();
                }
            }
        }
    }
}