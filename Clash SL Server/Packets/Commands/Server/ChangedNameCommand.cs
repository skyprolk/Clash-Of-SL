using CSS.Helpers.List;

namespace CSS.Packets.Commands.Server
{
    internal class ChangedNameCommand : Command
    {
        public ChangedNameCommand(Device client) : base(client)
        {
            this.Identifier = 3;
        }

        internal override void Encode()
        {
            this.Data.AddString(Name);
            this.Data.AddLong(ID);
        }

        public string Name { get; set; }
        public long ID { get; set; }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetId(long id)
        {
            ID = id;
        }
    }
}
