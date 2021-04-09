using CSS.Helpers.Binary;

namespace CSS.Packets.Commands.Client
{
    internal class RemoveShieldToAttackCommand : Command
    {
        public RemoveShieldToAttackCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }
    }
}
