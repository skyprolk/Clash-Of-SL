using CSS.Helpers.Binary;

namespace CSS.Packets.Commands.Client
{
    // Packet 4294967295
    internal class RemoveMultiBuildingsCommand : Command
    {
        public RemoveMultiBuildingsCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }
    }
}