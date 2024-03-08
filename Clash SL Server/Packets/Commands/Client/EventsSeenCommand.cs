using CSS.Helpers.Binary;

namespace CSS.Packets.Commands.Client
{
    internal class EventsSeenCommand : Command
    {
        public EventsSeenCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public int Tick;

        public int UnknownID;

        internal override void Process()
        {
            this.UnknownID = this.Reader.ReadInt32();
            this.Tick = this.Reader.ReadInt32();
        }
    }
}
