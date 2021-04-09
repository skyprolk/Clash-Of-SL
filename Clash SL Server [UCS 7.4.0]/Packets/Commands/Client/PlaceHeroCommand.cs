using CSS.Helpers.Binary;

namespace CSS.Packets.Commands.Client
{
    // Packet 605
    internal class PlaceHeroCommand : Command
    {
        public PlaceHeroCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }


        internal override void Decode()
        {
            this.X = this.Reader.ReadInt32();
            this.Y = this.Reader.ReadInt32();
            this.HeroID = this.Reader.ReadInt32();
            this.Tick = this.Reader.ReadInt32();
        }

        public int X;

        public int Y;

        public int Tick;

        public int HeroID;
    }
}