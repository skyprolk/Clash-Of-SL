using System;
using System.IO;
using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Messages.Client
{
    // Packet 10117
    internal class ReportPlayerMessage : Message
    {
        public ReportPlayerMessage(Device device, Reader reader) : base(device, reader)
        {
        }

        public long ReportedPlayerID { get; set; }

        public int Tick { get; set; }

        internal override void Decode()
        {
            this.Reader.ReadInt32();
            this.ReportedPlayerID = this.Reader.ReadInt64();
            this.Reader.ReadInt32();
        }

        internal override async void Process()
        {
            try
            {
                Level ReportedPlayer = await ResourcesManager.GetPlayer(ReportedPlayerID);
                ReportedPlayer.Avatar.ReportedTimes++;
                if (ReportedPlayer.Avatar.ReportedTimes >= 3)
                {
                    AvatarChatBanMessage _AvatarChatBanMessage = new AvatarChatBanMessage(ReportedPlayer.Client);
                    //_AvatarChatBanMessage.SetBanPeriod(86400); // A Day
                    _AvatarChatBanMessage.SetBanPeriod(1800); // 30 Minutes
                    _AvatarChatBanMessage.Send();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
