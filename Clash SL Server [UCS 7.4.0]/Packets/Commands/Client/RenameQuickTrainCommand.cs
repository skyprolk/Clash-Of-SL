using CSS.Helpers.Binary;

namespace CSS.Packets.Commands.Client
{
    internal class RenameQuickTrainCommand : Command
    {
        public RenameQuickTrainCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.SlotID = this.Reader.ReadInt32();
            this.SlotName = this.Reader.ReadString();
            this.Tick = this.Reader.ReadInt32();
        }

        public int SlotID;
        public string SlotName;
        public int Tick;
    }
}
