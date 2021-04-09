using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Commands.Client
{
    // Packet 570
    internal class TogglePlayerWarStateCommand : Command
    {
        public TogglePlayerWarStateCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
        }

        internal override async void Process()
        {
            Alliance a = ObjectManager.GetAlliance(this.Device.Player.Avatar.AllianceId);
            if (a != null)
            {
                AllianceMemberEntry _AllianceMemberEntry = a.m_vAllianceMembers[this.Device.Player.Avatar.UserId];
                _AllianceMemberEntry.ToggleStatus();
                PlayerWarStatusMessage _PlayerWarStatusMessage = new PlayerWarStatusMessage(this.Device)
                {
                    Status = _AllianceMemberEntry.WarOptInStatus
                };
                _PlayerWarStatusMessage.Send();
            }
        }
    }
}
