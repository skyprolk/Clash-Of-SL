using CSS.Helpers.Binary;

namespace CSS.Packets.Commands.Client
{
    // Packet 524
    internal class ToggleAttackModeCommand : Command
    {
        public ToggleAttackModeCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
            /*
            BuildingId = br.ReadUInt32WithEndian(); //buildingId - 0x1DCD6500;
            Unknown1 = br.ReadByte();
            Unknown2 = br.ReadUInt32WithEndian();
            Unknown3 = br.ReadUInt32WithEndian();
            */
        }

        public uint BuildingId { get; set; }
        public byte Unknown1 { get; set; }
        public uint Unknown2 { get; set; }
        public uint Unknown3 { get; set; }
    }
}