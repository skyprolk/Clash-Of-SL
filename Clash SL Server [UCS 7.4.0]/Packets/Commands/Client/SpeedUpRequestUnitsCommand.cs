using CSS.Helpers.Binary;

namespace CSS.Packets.Commands.Client
{
    internal class SpeedUpRequestUnitsCommand : Command
    {
        public SpeedUpRequestUnitsCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }


        public int Tick;

        internal override void Decode()
        {
            this.Tick = this.Reader.ReadInt32();
        }
    }
}
