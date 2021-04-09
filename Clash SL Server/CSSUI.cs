using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSS.Core;
using CSS.Core.Network;
using CSS.Logic;
using CSS.Logic.AvatarStreamEntry;
using CSS.PacketProcessing.Messages.Server;

namespace css
{
    public partial class CSSUI : MetroForm
    {
        
        private Level level;

        public static IPAddress LocalNetworkIP
        {
            get
            {
                try
                {
                    using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                    {
                        socket.Connect("10.0.2.4", 65530);
                        return (socket.LocalEndPoint as IPEndPoint).Address;
                    }
                }
                catch
                {
                    return IPAddress.Parse("127.0.0.1");
                }
            }
        }
        public CSSUI()
        {
            InitializeComponent();
        }

       
        private void cssUI_Load(object sender, EventArgs e)
        {
            //var publicIP = new WebClient().DownloadString("http://bot.whatismyipaddress.com/");
            metroTextBox1.Text = ""+LocalNetworkIP;
            metroTextBox2.Text = "";
            onlinepltxt.Text = Convert.ToString(ResourcesManager.GetOnlinePlayers().Count);
            metroLabel5.Text = Convert.ToString(ResourcesManager.GetConnectedClients().Count);
            onlinepltxt.TextChanged += Onlinepltxt_TextChanged;
            
        }

        private void Onlinepltxt_TextChanged(object sender, EventArgs e)
        {
            onlinepltxt.Text += ResourcesManager.GetOnlinePlayers().Count;
        }
        
        private void restarbtn_Click(object sender, EventArgs e)
        {

            Process.Start("restarter.bat");
        }
        
        private void metroButton2_Click(object sender, EventArgs e)
        {

            var mail = new AllianceMailStreamEntry();
            mail.SetId((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            mail.SetSenderId(0);
            mail.SetSenderAvatarId(0);
            mail.SetSenderName(metroTextBox5.Text);
            mail.SetIsNew(0);
            mail.SetAllianceId(0);
            mail.SetAllianceBadgeData(1728059989);
            mail.SetAllianceName(metroTextBox6.Text);
            mail.SetMessage(metroTextBox4.Text);
            mail.SetSenderLevel(500);
            mail.SetSenderLeagueId(22);

            foreach (var onlinePlayer in ResourcesManager.GetOnlinePlayers())
            {
                 var p = new AvatarStreamEntryMessage(onlinePlayer.GetClient());
                p.SetAvatarStreamEntry(mail);
                  PacketManager.ProcessOutgoingPacket(p);
                
            }
        }

        private void metroTabPage5_Click(object sender, EventArgs e)
        {

        }

        private void metroLink1_Click(object sender, EventArgs e)
        {
            Process.Start("http://facebook.com/skyprolk");
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            try
            {
                metroLabel16.Text = "";
                PlayerNameBox.Clear();
                txtPlayerGems.Clear();
                lvlbox.Clear();
                StatusComboBox.Clear();
                PlayerScoreBox.Clear();
                PlayerRankBox.Clear();

                PlayerNameBox.Text = ResourcesManager.GetPlayer(long.Parse(PlayerID.Text)).GetPlayerAvatar().GetAvatarName();
                PlayerRankBox.Text += ResourcesManager.GetPlayer(long.Parse(PlayerID.Text)).GetAccountPrivileges();
                PlayerScoreBox.Text += ResourcesManager.GetPlayer(long.Parse(PlayerID.Text)).GetPlayerAvatar().GetScore();

                lvlbox.Text += ResourcesManager.GetPlayer(long.Parse(PlayerID.Text)).GetPlayerAvatar().GetAvatarLevel();
                metroLabel16.Text += ResourcesManager.GetPlayer(long.Parse(PlayerID.Text)).GetPlayerAvatar().GetAllianceId();
                txtPlayerGems.Text += ResourcesManager.GetPlayer(long.Parse(PlayerID.Text)).GetPlayerAvatar().GetDiamonds();
                if (ResourcesManager.GetPlayer(long.Parse(PlayerID.Text)).GetAccountStatus() == 0)
                {
                    StatusComboBox.Text = "Normal";
                }
                else if (ResourcesManager.GetPlayer(long.Parse(PlayerID.Text)).GetAccountStatus() == 99)
                {
                    StatusComboBox.Text = "Banned";
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Can't Load Player Profile, This Player ID Not Found : " + PlayerID.Text,"Message",MessageBoxButtons.OK,MessageBoxIcon.Information );
 
            }
            
        }

        
        private void metroButton3_Click(object sender, EventArgs e)
        {
            byte pre = Convert.ToByte(PlayerRankBox.Text);

            ResourcesManager.GetPlayer(long.Parse(PlayerID.Text)).SetAccountPrivileges(pre);
            ResourcesManager.GetPlayer(long.Parse(PlayerID.Text)).GetPlayerAvatar().SetScore(int.Parse(PlayerScoreBox.Text));
            ResourcesManager.GetPlayer(long.Parse(PlayerID.Text)).GetPlayerAvatar().SetName(PlayerNameBox.Text);
            ResourcesManager.GetPlayer(long.Parse(PlayerID.Text)).GetPlayerAvatar().SetAvatarLevel(int.Parse(lvlbox.Text));
            ResourcesManager.GetPlayer(long.Parse(PlayerID.Text)).GetPlayerAvatar().SetDiamonds(int.Parse(txtPlayerGems.Text));
            MessageBox.Show("Player Profile Updated.","Message",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        

        private void metroButton4_Click(object sender, EventArgs e)
        {
            foreach(var onlineplayer in ResourcesManager.GetOnlinePlayers())
            {
            var p = new GlobalChatLineMessage(onlineplayer.GetClient());
            p.SetChatMessage(metroTextBox8.Text);
            p.SetPlayerId(0);
            p.SetLeagueId(22);
            p.SetPlayerName(metroTextBox7.Text);
            PacketManager.ProcessOutgoingPacket(p);
            }
            
        }

       
        private void Loadbtn_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            foreach(var account in ResourcesManager.GetOnlinePlayers())
            {
                ListViewItem item = new ListViewItem(account.GetPlayerAvatar().GetAvatarName());
                item.SubItems.Add(Convert.ToString(account.GetPlayerAvatar().GetId()));
                item.SubItems.Add(Convert.ToString(account.GetPlayerAvatar().GetAvatarLevel()));
                item.SubItems.Add(Convert.ToString(account.GetPlayerAvatar().GetScore()));
                item.SubItems.Add(Convert.ToString(account.GetAccountPrivileges()));
                listView1.Items.Add(item);
            }
               
        }
 
    }
}
