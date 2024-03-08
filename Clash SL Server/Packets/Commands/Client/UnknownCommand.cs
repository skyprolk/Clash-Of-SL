using CSS.Helpers.Binary;

namespace CSS.Packets.Commands.Client
{
    // Packet 3072
    internal class UnknownCommand : Command
    {
        public UnknownCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
            //Unknown1 = br.ReadInt32();
            //Tick = br.ReadInt32();
        }

        public static int Tick;
        public static int Unknown1;
    }
}