using CSS.Helpers.List;


namespace CSS.Packets.Messages.Server
{
    internal class RequestConfirmChangeNameMessage : Message
    {
        public RequestConfirmChangeNameMessage(Device client, string name) : base(client)
        {
            this.Identifier = 20300;
            this.Name = name;
        }

        public string Name;

        internal override void Encode()
        {
            this.Data.AddLong(0);
            this.Data.AddString(this.Name);
        }
    }
}
