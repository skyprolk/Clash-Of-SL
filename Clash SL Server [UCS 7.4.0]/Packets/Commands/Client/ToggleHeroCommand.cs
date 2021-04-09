using CSS.Helpers.Binary;

namespace CSS.Packets.Commands.Client
{
    // Packet 500000004
    internal class ToggleHeroCommand : Command
    {
        public ToggleHeroCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }
    }
}