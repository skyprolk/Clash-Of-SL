using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using CSS.Core;
using CSS.Core.Network;
using CSS.Core.Web;
using CSS.Packets.Messages.Server;
using System.Timers;
using CSS.Logic.AvatarStreamEntry;
using CSS.Core.Settings;
using CSS.Logic;

namespace CSS
{
    public partial class CSSUI : MaterialForm
    {
        public CSSUI()
        {
            InitializeComponent();
            var sm = MaterialSkinManager.Instance;
            sm.AddFormToManage(this);
            sm.Theme = MaterialSkinManager.Themes.DARK;
            sm.ColorScheme = new ColorScheme(Primary.Blue800, Primary.Blue900, Primary.Grey500, Accent.Blue200, TextShade.WHITE);
        }

        public System.Timers.Timer T = new System.Timers.Timer();

        public int Count = 0;

        private void CSSUI_Load(object sender, EventArgs e)
        {
			IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
			labelIP.Text = Convert.ToString(ipHostInfo.AddressList[0]);
            labelPort.Text = ConfigurationManager.AppSettings["ServerPort"];
            labelOnlinePlayers.Text = Convert.ToString(ResourcesManager.m_vOnlinePlayers.Count);
            labelConnectedPlayers.Text = Convert.ToString(ResourcesManager.GetConnectedClients().Count);
            labelMemoryPlayers.Text = Convert.ToString(ResourcesManager.m_vInMemoryLevels.Count);
            materialLabel14.Text = Convert.ToString(ObjectManager.GetMaxPlayerID() + ObjectManager.GetMaxAllianceID());
			materialLabel15.Text = Convert.ToString(ObjectManager.GetMaxAllianceID());
			materialLabel16.Text = Convert.ToString(ObjectManager.GetMaxPlayerID());
            Version.Text = $"Version: {Constants.Version}";
            Build.Text = $"Build: {Constants.Version}";

           

			// CONFIG EDITOR
			txtStartingGems.Text = ConfigurationManager.AppSettings["startingGems"];
            txtStartingGold.Text = ConfigurationManager.AppSettings["startingGold"];
            txtStartingElixir.Text = ConfigurationManager.AppSettings["startingElixir"];
            txtStartingDarkElixir.Text = ConfigurationManager.AppSettings["startingDarkElixir"];
            txtStartingTrophies.Text = ConfigurationManager.AppSettings["startingTrophies"];
            txtStartingLevel.Text = ConfigurationManager.AppSettings["startingLevel"];
            txtUpdateURL.Text = ConfigurationManager.AppSettings["UpdateUrl"];
            txtUsePatch.Text = ConfigurationManager.AppSettings["useCustomPatch"];
            txtPatchURL.Text = ConfigurationManager.AppSettings["patchingServer"];
            txtMintenance.Text = ConfigurationManager.AppSettings["maintenanceTimeleft"];
            txtDatabaseType.Text = ConfigurationManager.AppSettings["databaseConnectionName"];
            txtPort.Text = ConfigurationManager.AppSettings["ServerPort"];
            txtAdminMessage.Text = ConfigurationManager.AppSettings["AdminMessage"];
            txtLogLevel.Text = ConfigurationManager.AppSettings["LogLevel"];
            txtClientVersion.Text = ConfigurationManager.AppSettings["ClientVersion"];

            //PLAYER MANAGER
            txtPlayerName.Enabled = false;
            txtPlayerScore.Enabled = false;
            txtPlayerGems.Enabled = false;
            txtPlayerGold.Enabled = false;
            txtPlayerElixir.Enabled = false;
            txtPlayerDarkElixir.Enabled = false;
            txtTownHallLevel.Enabled = false;
            txtAllianceID.Enabled = false;
            txtPlayerLevel.Enabled = false;

            listView1.Items.Clear();
            int count = 0;
            foreach (var acc in ResourcesManager.m_vOnlinePlayers)
            {
                ListViewItem item = new ListViewItem(acc.Avatar.AvatarName);
                item.SubItems.Add(Convert.ToString(acc.Avatar.UserId));
                item.SubItems.Add(Convert.ToString(acc.Avatar.m_vAvatarLevel));
                item.SubItems.Add(Convert.ToString(acc.Avatar.GetScore()));
                item.SubItems.Add(Convert.ToString(acc.Avatar.AccountPrivileges));
                listView1.Items.Add(item);
                count++;
                if(count >= 100)
                {
                    break;
                }
            }

            listView2.Items.Clear();
            int count2 = 0;
            foreach(var alliance in ResourcesManager.GetInMemoryAlliances())
            {
                ListViewItem item = new ListViewItem(alliance.m_vAllianceName);
                item.SubItems.Add(alliance.m_vAllianceId.ToString());
                item.SubItems.Add(alliance.m_vAllianceLevel.ToString());
                item.SubItems.Add(alliance.GetAllianceMembers().Count.ToString());
                item.SubItems.Add(alliance.m_vScore.ToString());
                listView2.Items.Add(item);
                count2++;

                if(count2 >= 100)
                {
                    break;
                }
            }
        }

        /* MAIN TAB */

        //Restart Button 
        private void materialRaisedButton2_Click(object sender, EventArgs e)
        {
            CSSControl.CSSRestart();
        }

        //Shutdown CSS Button
        private void materialRaisedButton12_Click(Object sender, EventArgs e)
        {
            Close();
            CSSControl.CSSClose();
        }

        //Close Button
        private void materialRaisedButton3_Click(object sender, EventArgs e)
        {
            Close();
        }

        //Reload Button
        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
			IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
			labelIP.Text = Convert.ToString(ipHostInfo.AddressList[0]);
			labelPort.Text = ConfigurationManager.AppSettings["ServerPort"];
            labelOnlinePlayers.Text = Convert.ToString(ResourcesManager.m_vOnlinePlayers.Count);
            labelConnectedPlayers.Text = Convert.ToString(ResourcesManager.GetConnectedClients().Count);
            labelMemoryPlayers.Text = Convert.ToString(ResourcesManager.m_vInMemoryLevels.Count);
			//materialLabel14.Text = Convert.ToString(ResourcesManager.GetAllPlayerIds().Count + ResourcesManager.GetAllClanIds().Count);
			//materialLabel15.Text = Convert.ToString(ResourcesManager.GetAllClanIds().Count);
			materialLabel16.Text = Convert.ToString(ResourcesManager.GetAllPlayerIds().Count);
		}

        /* END MAIN TAB */

        /* PLAYER MANAGER TAB*/

        /* PLAYER LIST TAB*/

        //Refresh Button
        private void materialRaisedButton11_Click(object sender, EventArgs e)
        {
            int count = 0;
            listView1.Items.Clear();
            foreach (var acc in ResourcesManager.m_vOnlinePlayers)
            {
                ListViewItem item = new ListViewItem(acc.Avatar.AvatarName);
                item.SubItems.Add(Convert.ToString(acc.Avatar.UserId));
                item.SubItems.Add(Convert.ToString(acc.Avatar.m_vAvatarLevel));
                item.SubItems.Add(Convert.ToString(acc.Avatar.GetScore()));
                item.SubItems.Add(Convert.ToString(acc.Avatar.AccountPrivileges));
                listView1.Items.Add(item);
                count++;
                if(count >= 100)
                {
                    break;
                }
            }
        }
        /* END OF PLAYER LIST TAB*/

        /* END OF EDIT PLAYER TAB*/

        //Load Player Button
        private async void materialRaisedButton6_Click(object sender, EventArgs e)
        {
            /* LOAD PLAYER */
            try
            {
                txtPlayerName.Enabled = true;
                txtPlayerScore.Enabled = true;
                txtPlayerGems.Enabled = true;
                txtPlayerGold.Enabled = true;
                txtPlayerElixir.Enabled = true;
                txtPlayerDarkElixir.Enabled = true;
                txtTownHallLevel.Enabled = true;
                txtAllianceID.Enabled = true;
                txtPlayerLevel.Enabled = true;

                Level l = await ResourcesManager.GetPlayer(long.Parse(txtPlayerID.Text));

                txtPlayerName.Text = Convert.ToString(l.Avatar.AvatarName);
                txtPlayerScore.Text = Convert.ToString(l.Avatar.GetScore());
                txtPlayerGems.Text = Convert.ToString(l.Avatar.m_vCurrentGems);
                txtPlayerGold.Text = Convert.ToString(l.Avatar.m_vCurrentGold);
                txtPlayerElixir.Text = Convert.ToString(l.Avatar.m_vCurrentElixir);
                txtPlayerDarkElixir.Text = Convert.ToString(l.Avatar.m_vCurrentDarkElixir);
                txtTownHallLevel.Text = Convert.ToString(l.Avatar.m_vTownHallLevel);
                txtAllianceID.Text = Convert.ToString(l.Avatar.AllianceId);
                materialLabel7.Text = l.Avatar.Region;
                txtPlayerLevel.Text = l.Avatar.m_vAvatarLevel.ToString();
            }
            catch (NullReferenceException)
            {
                var title = "Error";
                MessageBox.Show("Player with ID " + txtPlayerID.Text + " not found!", title, MessageBoxButtons.RetryCancel, MessageBoxIcon.Information);
                txtPlayerName.Enabled = false;
                txtPlayerScore.Enabled = false;
                txtPlayerGems.Enabled = false;
                txtPlayerGold.Enabled = false;
                txtPlayerElixir.Enabled = false;
                txtPlayerDarkElixir.Enabled = false;
                txtTownHallLevel.Enabled = false;
                txtAllianceID.Enabled = false;
                txtPlayerLevel.Enabled = false;

                txtPlayerName.Clear();
                txtPlayerScore.Clear();
                txtPlayerGems.Clear();
                txtPlayerGold.Clear();
                txtPlayerElixir.Clear();
                txtPlayerDarkElixir.Clear();
                txtTownHallLevel.Clear();
                txtAllianceID.Clear();
                txtPlayerLevel.Clear();
            }
            /* LOAD PLAYER */
        }

        //Clear Button
        private void materialRaisedButton8_Click(object sender, EventArgs e)
        {
            /* CLEAR */
            txtPlayerName.Clear();
            txtPlayerScore.Clear();
            txtPlayerGems.Clear();
            txtPlayerGold.Clear();
            txtPlayerElixir.Clear();
            txtPlayerDarkElixir.Clear();
            txtTownHallLevel.Clear();
            txtAllianceID.Clear();
            txtPlayerID.Clear();
            txtPlayerLevel.Clear();
            /* CLEAR */
        }

        //Save Button
        private async void materialRaisedButton7_Click(object sender, EventArgs e)
        {
            /* SAVE PLAYER */
            Level l = await ResourcesManager.GetPlayer(long.Parse(txtPlayerID.Text));

            l.Avatar.SetName(txtPlayerName.Text);
            l.Avatar.SetScore(Convert.ToInt32(txtPlayerScore.Text));
            l.Avatar.m_vCurrentGems = Convert.ToInt32(txtPlayerGems.Text);
            l.Avatar.m_vCurrentGold = Convert.ToInt32(txtPlayerGold.Text);
            l.Avatar.m_vCurrentElixir = Convert.ToInt32(txtPlayerElixir.Text);
            l.Avatar.m_vCurrentDarkElixir = Convert.ToInt32(txtPlayerDarkElixir.Text);
            l.Avatar.SetTownHallLevel(Convert.ToInt32(txtTownHallLevel.Text));
            l.Avatar.AllianceId = Convert.ToInt32(txtAllianceID.Text);
            l.Avatar.m_vAvatarLevel = Convert.ToInt32(txtPlayerLevel.Text);
            await Resources.DatabaseManager.Save(l);
            var title = "Finished!";
            MessageBox.Show("Player has been saved!", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            /* SAVE PLAYER */
        }

        /* END OF EDIT PLAYER TAB*/

        /* END OF PLAYER MANAGER TAB*/

        /*CONFIG EDITOR TAB*/

        //Reset Button
        private void materialRaisedButton5_Click(object sender, EventArgs e)
        {
            txtStartingGems.Text = "999999999";
            txtStartingGold.Text = "2000";
            txtStartingElixir.Text = "2000";
            txtStartingDarkElixir.Text = "2000";
            txtStartingTrophies.Text = "0";
            txtStartingLevel.Text = "1";
            txtUpdateURL.Text = "N/A";
            txtUsePatch.Text = "false";
            txtPatchURL.Text = "";
            txtMintenance.Text = "0";
            txtDatabaseType.Text = "";
            txtPort.Text = "9339";
            txtAdminMessage.Text = $"Welcome to Clash SL Server v{VersionChecker.GetVersionString()} By #DARK";
            txtLogLevel.Text = "0";
            txtClientVersion.Text = "8.709.16";
        }

        //Save Changes Button
        private void materialRaisedButton4_Click(object sender, EventArgs e)
        {
           var doc = new XmlDocument();
            var path = "config.css";
            doc.Load(path);
            var ie = doc.SelectNodes("appSettings/add").GetEnumerator();

            while (ie.MoveNext())
            {
                if ((ie.Current as XmlNode).Attributes["key"].Value == "startingGems")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtStartingGems.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "startingGold")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtStartingGold.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "startingElixir")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtStartingElixir.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "startingDarkElixir")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtStartingDarkElixir.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "startingTrophies")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtStartingTrophies.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "startingLevel")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtStartingLevel.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "UpdateUrl")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtUpdateURL.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "useCustomPatch")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtUsePatch.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "patchingServer")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtPatchURL.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "maintenanceTimeleft")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtMintenance.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "databaseConnectionName")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtDatabaseType.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "ServerPort")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtPort.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "AdminMessage")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtAdminMessage.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "LogLevel")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtLogLevel.Text;
                }
                if ((ie.Current as XmlNode).Attributes["key"].Value == "ClientVersion")
                {
                    (ie.Current as XmlNode).Attributes["value"].Value = txtClientVersion.Text;
                }
            }

            doc.Save(path);
            var title = "Clash SL Server Manager GUI";
            var message = "Changes has been saved!";
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /* END OF CONFIG EDITOR TAB*/

        /* MAIL TAB*/

        //Send To Gobal Chat Button
        private void materialRaisedButton9_Click(object sender, EventArgs e)
        {
            foreach (var onlinePlayer in ResourcesManager.m_vOnlinePlayers)
            {
                var pm = new GlobalChatLineMessage(onlinePlayer.Client)
                {
                    Message = textBox21.Text,
                    HomeId = 0,
                    CurrentHomeId = 0,
                    LeagueId = 22,
                    PlayerName = textBox22.Text
                };

                pm.Send();
            }
        }

        //Send To Mailbox Button
        private void materialRaisedButton10_Click(object sender, EventArgs e)
        {
            var mail = new AllianceMailStreamEntry();
            mail.ID = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            mail.SenderId = 0;
            mail.SenderId = 0;
            mail.m_vSenderName = textBox23.Text;
            mail.IsNew = 2; // 0 = Seen, 2 = New
            mail.AllianceId = 0;
            mail.AllianceBadgeData = 1526735450;
            mail.AllianceName = "CSS";
            mail.Message = textBox24.Text;
            mail.m_vSenderLevel = 300;
            mail.m_vSenderLeagueId = 22;

            foreach (var onlinePlayer in ResourcesManager.m_vOnlinePlayers)
            {
                var p = new AvatarStreamEntryMessage(onlinePlayer.Client);
                p.SetAvatarStreamEntry(mail);
                p.Send();
            }
        }

        /* END OF MAIL TAB*/

        private async void materialRaisedButton13_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPlayerID.Text))
            {
                MessageBox.Show("The Player-ID can't be empty!", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
            else
            {
                var id = Convert.ToInt64(txtPlayerID.Text);
                var player = await ResourcesManager.GetPlayer(id);
                new OutOfSyncMessage(player.Client).Send();
            }
        }

        private async void materialRaisedButton14_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPlayerID.Text))
            {
                MessageBox.Show("The Player-ID can't be empty!", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
            else
            {
                long id = Convert.ToInt64(txtPlayerID.Text);
                Level player = await ResourcesManager.GetPlayer(id);
                player.Avatar.AccountBanned = true;
                Resources.DatabaseManager.Save(player);
                new OutOfSyncMessage(player.Client).Send();
            }
        }

        private async void materialRaisedButton15_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPlayerID.Text))
            {
                MessageBox.Show("The Player-ID can't be empty!", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
            else
            {
                var id = Convert.ToInt64(txtPlayerID.Text);
                var player = await ResourcesManager.GetPlayer(id);
                player.Avatar.AccountBanned = true;
                Resources.DatabaseManager.Save(player);
            }
        }

        private async void materialRaisedButton16_Click(object sender, EventArgs e)
        {
            var name = txtSearchPlayer.Text;
            listView1.Items.Clear();
            if (string.IsNullOrEmpty(txtSearchPlayer.Text))
            {
                MessageBox.Show("The Player-Name can't be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                foreach (var n in ResourcesManager.m_vInMemoryLevels.Values.ToList())
                {
                    var l = await ResourcesManager.GetPlayer(n.Avatar.UserId);
                    var na = l.Avatar.AvatarName;
                    if (na == name || na == name.ToUpper() || na == name.ToLower())
                    {
                        ListViewItem item = new ListViewItem(n.Avatar.AvatarName);
                        item.SubItems.Add(Convert.ToString(n.Avatar.UserId));
                        item.SubItems.Add(Convert.ToString(n.Avatar.m_vAvatarLevel));
                        item.SubItems.Add(Convert.ToString(n.Avatar.GetScore()));
                        item.SubItems.Add(Convert.ToString(n.Avatar.AccountPrivileges));
                        listView1.Items.Add(item);
                    }
                }
            }
        }

        private void materialRaisedButton17_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The Name in the Game.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void materialRaisedButton18_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The Time where the Message will be automatically sendet. (In Seconds)", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void materialRaisedButton19_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Message will be shown in the Global Chat.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void materialRaisedButton20_Click(object sender, EventArgs e)
        {
            var Name = textBox1.Text;
            int Interval = Convert.ToInt32(((textBox2.Text + 0) + 0) + 0);
            var Message = textBox3.Text;

            if (Convert.ToInt32(Interval) < 1)
            {
                MessageBox.Show("The Interval can't be null!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                foreach (Level onlinePlayer in ResourcesManager.m_vOnlinePlayers)
                {
                    var pm = new GlobalChatLineMessage(onlinePlayer.Client)
                    {
                        Message = Message,
                        HomeId = 0,
                        CurrentHomeId = 0,
                        LeagueId = 22,
                        PlayerName = Name
                    };
                    pm.Send();
                }

                Count++;
                materialLabel13.Text = Convert.ToString(Count);

                T.Interval = Interval;
                T.Elapsed += ((s, o) =>
                {
                    foreach (Level onlinePlayer in ResourcesManager.m_vOnlinePlayers)
                    {
                        var pm = new GlobalChatLineMessage(onlinePlayer.Client)
                        {
                            Message = Message,
                            HomeId = 0,
                            CurrentHomeId = 0,
                            LeagueId = 22,
                            PlayerName = Name
                        };
                        pm.Send();
                    }
                    Count++;
                    materialLabel13.Text = Convert.ToString(Count);
                });
                T.Start();
            }
        }

        private void materialRaisedButton21_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            int count2 = 0;
            foreach (var alliance in ResourcesManager.GetInMemoryAlliances())
            {
                ListViewItem item = new ListViewItem(alliance.m_vAllianceName);
                item.SubItems.Add(alliance.m_vAllianceId.ToString());
                item.SubItems.Add(alliance.m_vAllianceLevel.ToString());
                item.SubItems.Add(alliance.GetAllianceMembers().Count.ToString());
                item.SubItems.Add(alliance.m_vScore.ToString());
                listView2.Items.Add(item);
                count2++;

                if (count2 >= 100)
                {
                    break;
                }
            }
        }

        private async void materialRaisedButton24_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtID.Text))
            {
                Alliance alliance = ObjectManager.GetAlliance(long.Parse(txtID.Text));
                txtAllianceName.Text = alliance.m_vAllianceName;
                txtAllianceLevel.Text = alliance.m_vAllianceLevel.ToString();
                txtAllianceDescription.Text = alliance.m_vAllianceDescription;
                txtAllianceScore.Text = alliance.m_vScore.ToString();
            }
            else
            {
                MessageBox.Show("The Alliance ID can't be null or empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void materialRaisedButton22_Click(object sender, EventArgs e)
        {
            txtAllianceName.Clear();
            txtAllianceLevel.Clear();
            txtAllianceDescription.Clear();
            txtAllianceScore.Text = "0";
        }

        private async void materialRaisedButton23_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtID.Text))
            {
                Alliance alliance = ObjectManager.GetAlliance(long.Parse(txtID.Text));
                alliance.m_vAllianceName = txtAllianceName.Text;
                alliance.m_vAllianceLevel = Convert.ToInt32(txtAllianceLevel.Text);
                alliance.m_vAllianceDescription = txtAllianceDescription.Text;
                alliance.m_vScore = Convert.ToInt32(txtAllianceScore.Text);
                Resources.DatabaseManager.Save(alliance);
            }
            else
            {
                MessageBox.Show("The Alliance ID can't be null or empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void materialRaisedButton25_Click(object sender, EventArgs e)
        {
        }
        private void materialRaisedButton19_Click_1(object sender, EventArgs e)
        {
            T.Stop();
            Count = 0;
            materialLabel13.Text = Convert.ToString(Count);
        }

        private void materialLabel40_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void materialTabSelector1_Click(object sender, EventArgs e)
        {

        }

        private void materialRaisedButton25_Click_1(object sender, EventArgs e)
        {
            Resources.DatabaseManager.Save(ResourcesManager.m_vInMemoryLevels.Values.ToList()).Wait();
            Resources.DatabaseManager.Save(ResourcesManager.GetInMemoryAlliances()).Wait();
            MessageBox.Show("All In-memory data saved to MySQL Database", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
