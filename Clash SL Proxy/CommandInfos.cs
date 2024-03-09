using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP
{
    internal class CommandInfos
    {
        private static readonly Dictionary<int, string> Commands = new Dictionary<int, string>
        {
            {1, "Join_Alliance_Reply"},
            {502, "Upgrade_Building"},
            {504, "Speed_Up_Building_Upgrade"},
            {506, "Collect_Resources"},
            {507, "Remove_Obstacle"},
            {508, "Train_Units"},
            {513, "Speed_Up_Unit_Production"},
            {518, "Buy_Resources"},
            {523, "Claim_Achievement"},
            {534, "Remove_Shield"},
            {542, "Share_Replay"},
            {571, "Toggle_Clan_Chat_Filter"},
            {600, "Place_Unit"},
            {700, "Search_Opponent"},
        };

        public static string GetPacketName(int CommandID)
        {
            string packetname = "";
            if (Commands.ContainsKey(CommandID))
            {
                packetname = Commands[CommandID] + "(" + CommandID + ")";
            }
            else
            {
                packetname = "Unknown Command (" + CommandID + ")";
            }
            return packetname;
        }
    }
}
