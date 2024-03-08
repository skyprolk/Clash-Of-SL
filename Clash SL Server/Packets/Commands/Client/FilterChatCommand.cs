using CSS.Helpers.Binary;

namespace CSS.Packets.Commands.Client
{
    // Packet 571
    internal class FilterChatCommand : Command
    {
        public FilterChatCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }
    }
}