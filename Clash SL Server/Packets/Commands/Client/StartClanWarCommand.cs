using System;
using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Logic.StreamEntry;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Commands.Client
{
    internal class StartClanWarCommand : Command
    {
        public StartClanWarCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.Tick = this.Reader.ReadInt32();
        }

        public int Tick;

        internal override async void Process()
        {
            try
            {
                Alliance an = ObjectManager.GetAlliance(this.Device.Player.Avatar.AllianceId);
                if (an != null)
                {
                    if (an.GetAllianceMembers().Count >= 10)
                    {
                        AllianceEventStreamEntry eventStreamEntry = new AllianceEventStreamEntry();
                        eventStreamEntry.ID = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                        eventStreamEntry.SetSender(this.Device.Player.Avatar);
                        eventStreamEntry.EventType = 7;
                        an.AddChatMessage(eventStreamEntry);

                        foreach (AllianceMemberEntry a in an.GetAllianceMembers())
                        {
                            Level l = await ResourcesManager.GetPlayer(a.AvatarId);
                            new AllianceWarMapDataMessage(l.Client).Send();

                            new AllianceStreamEntryMessage(l.Client) { StreamEntry = eventStreamEntry }.Send();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
