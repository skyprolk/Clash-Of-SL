using CSS.Helpers.Binary;

namespace CSS.Packets.Commands.Client
{
    internal class BoostBarracksCommand : Command
    {
        public BoostBarracksCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.Tick = this.Reader.ReadInt64();
        }

        public long Tick;

        internal override void Process()
        {
           /* var player = level.Avatar;
            var barracks = level.GameObjectManager.GetGameObjectByID(500000010);
            var boost = (Building)barracks;

            if(!boost.IsBoosted)
            {
                boost.BoostBuilding();
            } */
        }
    }
}
