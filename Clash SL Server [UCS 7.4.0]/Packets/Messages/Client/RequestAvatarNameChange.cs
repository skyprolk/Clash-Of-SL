using System;
using System.IO;
using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Messages.Client
{
    // Packet 14600
    internal class RequestAvatarNameChange : Message
    {
        public RequestAvatarNameChange(Device device, Reader reader) : base(device, reader)
        {
        }

        public string PlayerName { get; set; }

        public byte Unknown1 { get; set; }

        internal override void Decode()
        {
            this.PlayerName = this.Reader.ReadString();
        }

        internal override async void Process()
        {
            try
            {
                long id = this.Device.Player.Avatar.UserId;
                Level l = await ResourcesManager.GetPlayer(id);
                if (l != null)
                {
                    if (PlayerName.Length > 15)
                    {
                        ResourcesManager.DisconnectClient(Device);
                    }
                    else
                    {
                        l.Avatar.SetName(PlayerName);
                        AvatarNameChangeOkMessage p = new AvatarNameChangeOkMessage(l.Client) {AvatarName = PlayerName};
                        p.Send();
                    }
                }
            } catch (Exception) { }
        }
    }
}