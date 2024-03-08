using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.Packets.Messages.Server;

namespace UCS.Packets.Messages.Client
{
    // Packet 14201
    internal class FacebookLinkMessage : Message
    {
        public FacebookLinkMessage(Packets.Client client, PacketReader br) : base(client, br)
        {
        }

        public override void Decode()
        {
            using (PacketReader br = new PacketReader(new MemoryStream(GetData())))
            {
                Unknown = br.ReadBoolean(); // Unknown, maybe if Logged in True if no False
                UserID = br.ReadString(); // Facebook UserID (https://www.facebook.com/ + UserID)
            }
        }

        public string UserID { get; set; }

        public bool Unknown { get; set; }

        public override async void Process(Level level)
        {
            try
            {
                ClientAvatar player = level.GetPlayerAvatar();

                Level l = await ResourcesManager.GetPlayerWithFacebookID(UserID);

                if (l != null)
                {
                    PacketProcessor.Send(new FacebookChooseVillageMessage(Client, l));
                }
                else if(player.GetFacebookID() == null)
                {
                    player.SetFacebookID(UserID);
                    PacketProcessor.Send(new OwnHomeDataMessage(Client, level)); // Until we got the other Message 
                }
            } catch (Exception) { }
        }
    }
}