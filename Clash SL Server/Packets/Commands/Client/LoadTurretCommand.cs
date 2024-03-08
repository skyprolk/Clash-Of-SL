using CSS.Helpers.Binary;
using CSS.Logic;

namespace CSS.Packets.Commands.Client
{
    // Packet 525
    internal class LoadTurretCommand : Command
    {
        public LoadTurretCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.m_vUnknown1 = this.Reader.ReadUInt32();
            this.m_vBuildingId = this.Reader.ReadInt32();
            this.m_vUnknown2 = this.Reader.ReadUInt32();
        }

        internal override void Process()
        {
            var go = this.Device.Player.GameObjectManager.GetGameObjectByID(m_vBuildingId);
            if (go?.GetComponent(1, true) != null)
                ((CombatComponent) go.GetComponent(1, true)).FillAmmo();
        }

        public int m_vBuildingId;
        public uint m_vUnknown1;
        public uint m_vUnknown2;
    }
}