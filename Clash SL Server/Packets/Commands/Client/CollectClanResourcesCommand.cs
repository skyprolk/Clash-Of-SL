using CSS.Helpers.Binary;

namespace CSS.Packets.Commands.Client
{
    internal class CollectClanResourcesCommand : Command
    {
        public CollectClanResourcesCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.Tick = this.Reader.ReadInt32();
        }

        public int Tick;

        internal override void Process()
        {
            // Database change is needed for the Player
        }
    }
}
