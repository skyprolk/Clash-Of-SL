using CSS.Helpers.Binary;
using CSS.Logic;

namespace CSS.Packets.Commands.Client
{
    internal class PlaceAllianceTroopsCommand : Command
    {
        public PlaceAllianceTroopsCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
        }

        internal override async void Process()
        {
            ClientAvatar _Player = this.Device.Player.Avatar;

            if (_Player != null)
            {
                _Player.AllianceUnits.Clear();
                _Player.SetAllianceCastleUsedCapacity(0);
            }
        }
    }
}
