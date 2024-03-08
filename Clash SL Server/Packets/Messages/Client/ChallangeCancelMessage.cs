using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Logic.StreamEntry;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Messages.Client
{
    internal class ChallangeCancelMessage : Message
    {
        public ChallangeCancelMessage(Device device, Reader reader) : base(device, reader)
        {
        }

        internal async void Process()
        {
            try
            {
                Alliance a = ObjectManager.GetAlliance(this.Device.Player.Avatar.AllianceId);
                StreamEntry s = a.m_vChatMessages.Find(c => c.SenderID == this.Device.Player.Avatar.AllianceId && c.GetStreamEntryType() == 12);

                if (s != null)
                {
                    a.m_vChatMessages.RemoveAll(t => t == s);
                    foreach (AllianceMemberEntry op in a.GetAllianceMembers())
                    {
                        Level player = await ResourcesManager.GetPlayer(op.AvatarId);
                        if (player.Client != null)
                        {
                            new AllianceStreamEntryRemovedMessage(Device, s.ID).Send();
                        }
                    }
                }
                else
                {
                    new OutOfSyncMessage(this.Device).Send();
                }
            } catch (Exception) { }
        }

    }
}
