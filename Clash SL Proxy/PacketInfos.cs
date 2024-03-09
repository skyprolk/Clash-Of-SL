using System.Collections.Generic;

namespace CSP
{
    internal class PacketInfos
    {
        private static readonly Dictionary<int, string> Packets = new Dictionary<int, string>
        {
            {10101, "Login"},
            {10105, "Ask_For_Friendlist"},
            {10108, "Keep_Alive"},
            {10117, "Report_Player"},
            {10212, "Change_Name"},
            {10905, "News_Seen"},
            {14102, "Executes_Command_Message"},
            {10100, "Login_Message"},
            {10103, "Create_Account" },
            {10116, "Reset_Account"},
            {10118, "Account_Switched"},
            {10502, "Add_Friend"},
            {10505, "Ask_For_FriendList"},
            {14101, "Go_Home"},
            {14113, "Visit_Home"},
            {14114, "Watch_Replay"},
            {14134, "Attack_Npc"},
            {14201, "Bind_Facebook_Account"},
            {14211, "Unbind_Facebook_Account"},
            {14302, "Ask_For_Alliance_Data"},
            {14303, "Ask_For_Alliance_List"},
            {14325, "Player_Profile"},
            {14341, "Ask_For_Bookmarks"},
            {14343, "Add_Alliance_Bookmark"},
            {14715, "Send_Global_Chat_Message"},
            {20104, "Login_OK"},
            {20103, "Login_Failed"},
            {20106, "Friend_Request_Sent"},
            {20100, "Handshake_Success"},
            {20108, "Keep_Alive_OK"},
            {20112, "Friend_Request_Failed"},
            {20505, "Friend_List_Data"},
            {24101, "Own_Home_Data"},
            {24107, "Enemy_Home_Data"},
            {24113, "Visited_Home_Data"},
            {24104, "Out_Of_Sync"},
            {24301, "Alliance_Data"},
            {24304, "Alliane_List_Data"},
            {24311, "Alliance_Stream"},
            {24334, "Avatar_Profile_Data"},
            {24335, "War_Map_Data"},
            {24411, "Avatar_Stream"},
        };

        public static string GetPacketName(int packetid)
        {
            string packetname = "";
            if (Packets.ContainsKey(packetid))
            {
                packetname = Packets[packetid] + "(" + packetid + ")";
            }
            else
            {
                packetname = "Unknown Packet (" + packetid + ")";
            }
            return packetname;
        }
    }
}
