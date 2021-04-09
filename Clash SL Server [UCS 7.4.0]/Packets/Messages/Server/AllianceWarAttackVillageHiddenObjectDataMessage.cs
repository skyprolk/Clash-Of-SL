namespace CSS.Packets.Messages.Server
{
    // Packet 24326
    internal class AllianceWarAttackVillageHiddenObjectDataMessage : Message
    {
        public AllianceWarAttackVillageHiddenObjectDataMessage(Device client) : base(client)
        {
            this.Identifier = 24326;
        }
    }
}