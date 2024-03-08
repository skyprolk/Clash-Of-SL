using System;
using System.IO;
using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers;
using CSS.Logic;
using CSS.Packets.Messages.Server;
using CSS.Helpers.Binary;
namespace CSS.Packets.Messages.Client
{
    // Packet 14113
    internal class VisitHomeMessage : Message
    {
        public VisitHomeMessage(Device device, Reader reader) : base(device, reader)
        {
        }

        internal long AvatarId;

        internal override void Decode()
        {
            this.AvatarId = this.Reader.ReadInt64();
        }

        internal override async void Process()
        {
            try
            {
                Level targetLevel = await ResourcesManager.GetPlayer(AvatarId);
                targetLevel.Tick();
                new VisitedHomeDataMessage(Device, targetLevel, this.Device.Player).Send();


                if (this.Device.Player.Avatar.AllianceId > 0)
                {
                    Alliance alliance = ObjectManager.GetAlliance(this.Device.Player.Avatar.AllianceId);
                    if (alliance != null)
                    {
                        new AllianceStreamMessage(Device, alliance).Send();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
