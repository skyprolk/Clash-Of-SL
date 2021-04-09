/*
 * Program : Clash Of SL Server
 * Description : A C# Writted 'Clash of SL' Server Emulator !
 *
 * Authors:  Sky Tharusha <Founder at Sky Production>,
 *           And the Official DARK Developement Team
 *
 * Copyright (c) 2021  Sky Production
 * All Rights Reserved.
 */

using System.IO;
using CSS.Helpers;
using CSS.Logic;

namespace CSS.PacketProcessing.Commands
{
    internal class RequestAllianceUnitsCommand : Command
    {
        #region Public Constructors

        public RequestAllianceUnitsCommand(CoCSharpPacketReader br)
        {
            Unknown1 = br.ReadUInt32WithEndian();
            FlagHasRequestMessage = br.ReadByte();
            if (FlagHasRequestMessage == 0x01)
                Message = br.ReadScString();
            else
                Message = "I need reinforcements !";
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
            /*
            var player = level.GetPlayerAvatar();

            var cm = new TroopRequestStreamEntry();
            cm.SetId((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            cm.SetSenderId(player.GetId());
            cm.SetHomeId(player.GetId());
            cm.SetSenderLeagueId(player.GetLeagueId());
            cm.SetSenderName(player.GetAvatarName());
            cm.SetSenderRole(player.GetAllianceRole());
            cm.SetMessage(Message);

            var all = ObjectManager.GetAlliance(player.GetAllianceId());
            all.AddChatMessage(cm);

            foreach (var onlinePlayer in ResourcesManager.GetOnlinePlayers())
                if (onlinePlayer.GetPlayerAvatar().GetAllianceId() == player.GetAllianceId())
                {
                    var p = new AllianceStreamEntryMessage(onlinePlayer.GetClient());
                    p.SetStreamEntry(cm);
                    PacketManager.ProcessOutgoingPacket(p);
                }
            */
        }

        #endregion Public Methods

        #region Public Properties

        public byte FlagHasRequestMessage { get; set; }
        public string Message { get; set; }
        public int MessageLength { get; set; }
        public uint Unknown1 { get; set; }

        #endregion Public Properties
    }
}