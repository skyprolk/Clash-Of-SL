using System;
using System.IO;
using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Logic.StreamEntry;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Messages.Client
{
    // Packet 14317
    internal class JoinRequestAllianceMessage : Message
    {
        public JoinRequestAllianceMessage(Device device, Reader reader) : base(device, reader)
        {
        }

        public string Message;

        public long ID;

        internal override void Decode()
        {
            this.ID      = this.Reader.ReadInt64();
            this.Message = this.Reader.ReadString();
        }


        internal override async void Process()
        {
            try
            {
                if (Message.Length > 0 && Message.Length < 100)
                {
                    ClientAvatar player = this.Device.Player.Avatar;
                    Alliance all = ObjectManager.GetAlliance(ID);

                    InvitationStreamEntry cm = new InvitationStreamEntry {ID = all.m_vChatMessages.Count + 1};
                    cm.SetSender(player);
                    cm.SetMessage(Message);
                    cm.SetState(1);
                    all.AddChatMessage(cm);

                    foreach (AllianceMemberEntry op in all.GetAllianceMembers())
                    {
                        Level playera = await ResourcesManager.GetPlayer(op.AvatarId);
                        if (playera.Client != null)
                        {
                            new AllianceStreamEntryMessage(playera.Client) {StreamEntry = cm}.Send();
                        }
                    }
                }
                else
                {
                    ResourcesManager.DisconnectClient(this.Device);
                }
            } catch (Exception) { }
        }
    }
}