using System;
using System.Collections.Generic;
using System.IO;
using CSS.Core;
using CSS.Helpers;
using CSS.Packets.Messages.Client;
using CSS.Packets.Messages.Client;

namespace CSS.Packets
{
    internal class MessageFactory
    {
        public static Dictionary<int, Type> Messages;

        public MessageFactory()
        {
            Messages = new Dictionary<int, Type>
            {
                {10100, typeof(SessionRequest)},
                {10101, typeof(LoginMessage)},
                {10105, typeof(AskForFriendListMessage)},
                {10108, typeof(KeepAliveMessage)},
                {10117, typeof(ReportPlayerMessage)},
                {10118, typeof(AccountSwitchMessage)},
                {10113, typeof(GetDeviceTokenMessage)},
                {10212, typeof(ChangeAvatarNameMessage)},
                {10502, typeof(AddClashFriendMessage)},
                {10905, typeof(NewsSeenMessage)},
                {14101, typeof(GoHomeMessage)},
                {14102, typeof(ExecuteCommandsMessage)},
                {14106, typeof(RetributionAttackerMessage)},
                {14110, typeof(ChallangeWatchLiveMessage)},
                {14111, typeof(ChallangeVisitMessage)},
                {14113, typeof(VisitHomeMessage)},
                {14114, typeof(ReplayRequestMessage)},
                {14120, typeof(ChallangeAttackMessage)},
                {14125, typeof(ChallangeCancelMessage)},
                {14134, typeof(AttackNpcMessage)},
                {14201, typeof(Bind_Facebook_Message)},
                {14316, typeof(EditClanSettingsMessage)},
                {14301, typeof(CreateAllianceMessage)},
                {14302, typeof(AskForAllianceDataMessage)},
                {14303, typeof(AskForJoinableAlliancesListMessage)},
                {14305, typeof(JoinAllianceMessage)},
                {14306, typeof(PromoteAllianceMemberMessage)},
                {14308, typeof(LeaveAllianceMessage)},
                {14310, typeof(DonateAllianceUnitMessage)},
                {14315, typeof(ChatToAllianceStreamMessage)},
                {14317, typeof(JoinRequestAllianceMessage)},
                {14321, typeof(TakeDecisionJoinRequestMessage)},
                {14322, typeof(AllianceInviteMessage)},
                {14324, typeof(SearchAlliancesMessage)},
                {14325, typeof(AskForAvatarProfileMessage)},
                {14331, typeof(AskForAllianceWarDataMessage)},
                {14336, typeof(AskForAllianceWarHistoryMessage)},
                {14341, typeof(AskForBookmarkMessage)},
                {14343, typeof(AddToBookmarkMessage)},
                {14344, typeof(RemoveFromBookmarkMessage)},
                {14715, typeof(SendGlobalChatLineMessage)},
                {14401, typeof(TopGlobalAlliancesMessage)},
                {14402, typeof(TopLocalAlliancesMessage)},
                {14403, typeof(TopGlobalPlayersMessage)},
                {14404, typeof(TopLocalPlayersMessage)},
                {14406, typeof(TopPreviousGlobalPlayersMessage)},
                {14503, typeof(TopLeaguePlayersMessage)},
                {14600, typeof(RequestAvatarNameChange)},
                {15001, typeof(AllianceWarAttackAvatarMessage)}
            };

            // 25006 = Live Clan Battle Notification, 

            //Messages.Add(10513, typeof(UnknownFacebookMessage));
            //Messages.Add(14262, typeof(BindGoogleAccount));
        }
    }
}
