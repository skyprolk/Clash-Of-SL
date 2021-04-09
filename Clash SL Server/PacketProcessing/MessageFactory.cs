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
using System.Collections.Generic;
using System.IO;
using CSS.Helpers;
using CSS.PacketProcessing.Messages.Client;

namespace CSS.PacketProcessing
{
    internal static class MessageFactory
    {

        static readonly Dictionary<int, Type> m_vMessages;

        static MessageFactory()
        {
            m_vMessages = new Dictionary<int, Type>();
            m_vMessages.Add(10100, typeof(HandshakeRequest));
            m_vMessages.Add(10101, typeof(LoginMessage));
            m_vMessages.Add(10105, typeof(AskForFriendListMessage));
            m_vMessages.Add(10108, typeof(KeepAliveMessage)); 
            m_vMessages.Add(10117, typeof(ReportPlayerMessage)); 
            m_vMessages.Add(10113, typeof(GetDeviceTokenMessage));
            m_vMessages.Add(10212, typeof(ChangeAvatarNameMessage)); 
            m_vMessages.Add(10905, typeof(NewsSeenMessage));
            m_vMessages.Add(14101, typeof(GoHomeMessage));
            m_vMessages.Add(14102, typeof(ExecuteCommandsMessage));
            m_vMessages.Add(14113, typeof(VisitHomeMessage));
            m_vMessages.Add(14134, typeof(AttackNpcMessage));
            m_vMessages.Add(14201, typeof(FacebookLinkMessage));
            m_vMessages.Add(14316, typeof(EditClanSettingsMessage));
            m_vMessages.Add(14301, typeof(CreateAllianceMessage));
            m_vMessages.Add(14302, typeof(AskForAllianceDataMessage));
            m_vMessages.Add(14303, typeof(AskForJoinableAlliancesListMessage));
            m_vMessages.Add(14305, typeof(JoinAllianceMessage));
            m_vMessages.Add(14306, typeof(PromoteAllianceMemberMessage));
            m_vMessages.Add(14308, typeof(LeaveAllianceMessage));
            m_vMessages.Add(14315, typeof(ChatToAllianceStreamMessage));
            m_vMessages.Add(14317, typeof(JoinRequestAllianceMessage));
            m_vMessages.Add(14321, typeof(TakeDecisionJoinRequestMessage));
            m_vMessages.Add(14322, typeof(AllianceInviteMessage));
            m_vMessages.Add(14324, typeof(SearchAlliancesMessage));
            m_vMessages.Add(14325, typeof(AskForAvatarProfileMessage));
            m_vMessages.Add(14331, typeof(AskForAllianceWarDataMessage));
            m_vMessages.Add(14336, typeof(AskForAllianceWarHistoryMessage));
            m_vMessages.Add(14341, typeof(AskForBookmarkMessage));
            m_vMessages.Add(14343, typeof(AddToBookmarkMessage));
            m_vMessages.Add(14344, typeof(RemoveFromBookmarkMessage));
            m_vMessages.Add(14715, typeof(SendGlobalChatLineMessage));
            m_vMessages.Add(14401, typeof(TopGlobalAlliancesMessage));
            m_vMessages.Add(14402, typeof(TopLocalAlliancesMessage));
            m_vMessages.Add(14403, typeof(TopGlobalPlayersMessage));
            m_vMessages.Add(14404, typeof(TopLocalPlayersMessage));
            m_vMessages.Add(14406, typeof(TopPreviousGlobalPlayersMessage));
            m_vMessages.Add(14503, typeof(TopLeaguePlayersMessage));
            m_vMessages.Add(14600, typeof(RequestAvatarNameChange));
        }

        public static object Read(Client c, CoCSharpPacketReader br, int packetType)
        {
            if (m_vMessages.ContainsKey(packetType))
                return Activator.CreateInstance(m_vMessages[packetType], c, br);
            return null;
        }

    }
}
