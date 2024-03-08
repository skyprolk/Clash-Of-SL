using CSS.Helpers.Binary;

namespace CSS.Packets.Commands.Client
{
    // Packet 20
    internal class RemainingBuildingsLayoutCommand : Command
    {
        public RemainingBuildingsLayoutCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }
    }
}