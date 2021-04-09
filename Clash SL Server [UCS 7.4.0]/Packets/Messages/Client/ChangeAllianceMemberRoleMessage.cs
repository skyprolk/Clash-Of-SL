using CSS.Helpers.Binary;

namespace CSS.Packets.Messages.Client
{
    // Packet 14306
    internal class ChangeAllianceMemberRoleMessage : Message
    {
        public ChangeAllianceMemberRoleMessage(Device device, Reader reader) : base(device, reader)
        {
            
        }
        public static int PacketID = 14306;
    }
}