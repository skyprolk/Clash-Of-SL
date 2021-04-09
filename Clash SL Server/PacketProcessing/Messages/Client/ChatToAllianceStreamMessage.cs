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

using System;
using System.IO;
using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers;
using CSS.Logic;
using CSS.Logic.StreamEntry;
using CSS.PacketProcessing.Messages.Server;

namespace CSS.PacketProcessing.Messages.Client
{
    internal class ChatToAllianceStreamMessage : Message
    {
        #region Private Fields

        string m_vChatMessage;

        #endregion Private Fields

        #region Public Constructors

        public ChatToAllianceStreamMessage(PacketProcessing.Client client, CoCSharpPacketReader br) : base(client, br)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Decode()
        {
            using (var br = new BinaryReader(new MemoryStream(GetData())))
            {
                m_vChatMessage = br.ReadScString();
            }
        }

        public override void Process(Level level)
        {
            if (m_vChatMessage.Length > 0)
            {
                if (m_vChatMessage[0] == '/')
                {
                    var obj = GameOpCommandFactory.Parse(m_vChatMessage);
                    if (obj != null)
                    {
                        var player = "";
                        if (level != null)
                            player += " (" + level.GetPlayerAvatar().GetId() + ", " +
                                      level.GetPlayerAvatar().GetAvatarName() + ")";
                        //Debugger.WriteLine("\t" + obj.GetType().Name + player);
                        ((GameOpCommand) obj).Execute(level);
                    }
                }
                else
                {
                    var avatar = level.GetPlayerAvatar();
                    var allianceId = avatar.GetAllianceId();
                    if (allianceId > 0)
                    {
                        var cm = new ChatStreamEntry();
                        cm.SetId((int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                        cm.SetAvatar(avatar);
                        cm.SetMessage(m_vChatMessage);

                        var alliance = ObjectManager.GetAlliance(allianceId);
                        if (alliance != null)
                        {
                            alliance.AddChatMessage(cm);

                            foreach (var onlinePlayer in ResourcesManager.GetOnlinePlayers())
                            {
                                if (onlinePlayer.GetPlayerAvatar().GetAllianceId() == allianceId)
                                {
                                    var p = new AllianceStreamEntryMessage(onlinePlayer.GetClient());
                                    p.SetStreamEntry(cm);
                                    PacketManager.ProcessOutgoingPacket(p);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion Public Methods
    }
}