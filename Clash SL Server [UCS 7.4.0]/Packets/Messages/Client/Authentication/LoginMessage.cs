using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using CSS.Core;
using CSS.Core.Crypto;
using CSS.Core.Network;
using CSS.Core.Settings;
using CSS.Files.Logic;
using CSS.Helpers;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Logic.AvatarStreamEntry;
using CSS.Logic.Enums;
using CSS.Packets;
using CSS.Packets.Messages.Server;
using CSS.Utilities.Blake2B;
using CSS.Utilities.Sodium;
using static CSS.Packets.Device;

namespace CSS.Packets.Messages.Client
{
    // Packet 10101
    internal  class LoginMessage : Message
    {
        public LoginMessage(Device device, Reader reader) : base(device, reader)
        {
            this.Device.PlayerState = State.LOGIN;
        }

        public string AdvertisingGUID;
        public string AndroidDeviceID;
        public string ClientVersion;
        public string DeviceModel;
        public string FacebookDistributionID;
        public string Region;
        public string MacAddress;
        public string MasterHash;
        public string UDID;
        public string OpenUDID;
        public string OSVersion;
        public string UserToken;
        public string VendorGUID;
        public int ContentVersion;
        public int LocaleKey;
        public int MajorVersion;
        public int MinorVersion;
        public uint Seed;
        public bool IsAdvertisingTrackingEnabled;
        public bool Android;
        public long UserID;
        public Level level;


        /// <summary>
        /// Decrypts this message.
        /// </summary>
        internal override void Decrypt()
        {
            byte[] Buffer = this.Reader.ReadBytes(this.Length);
            this.Device.Keys.PublicKey = Buffer.Take(32).ToArray();

            Blake2BHasher Blake = new Blake2BHasher();

            Blake.Update(this.Device.Keys.PublicKey);
            Blake.Update(Key.PublicKey);

            this.Device.Keys.RNonce = Blake.Finish();

            Buffer = Sodium.Decrypt(Buffer.Skip(32).ToArray(), this.Device.Keys.RNonce, Key.PrivateKey, this.Device.Keys.PublicKey);
            this.Device.Keys.SNonce = Buffer.Skip(24).Take(24).ToArray();
            this.Reader = new Reader(Buffer.Skip(48).ToArray());

            this.Length = (ushort)Buffer.Length;

        }

        internal override void Decode()
        {
            this.UserID = this.Reader.ReadInt64();
            this.UserToken = Reader.ReadString();
            this.MajorVersion = Reader.ReadInt32();
            this.ContentVersion = Reader.ReadInt32();
            this.MinorVersion = Reader.ReadInt32();
            this.MasterHash = Reader.ReadString();
            this.UDID = this.Reader.ReadString();
            this.OpenUDID = this.Reader.ReadString();
            this.MacAddress = this.Reader.ReadString();
            this.DeviceModel = this.Reader.ReadString();
            this.LocaleKey = this.Reader.ReadInt32();
            this.Region = this.Reader.ReadString();
            this.AdvertisingGUID = this.Reader.ReadString();
            this.OSVersion = this.Reader.ReadString();
            this.Android = this.Reader.ReadBoolean();
            this.Reader.ReadString();
            this.AndroidDeviceID = this.Reader.ReadString();
            this.FacebookDistributionID = this.Reader.ReadString();
            this.IsAdvertisingTrackingEnabled = this.Reader.ReadBoolean();
            this.VendorGUID = this.Reader.ReadString();
            this.Seed = this.Reader.ReadUInt32();
            this.Reader.ReadByte();
            this.Reader.ReadString();
            this.Reader.ReadString();
            this.ClientVersion = this.Reader.ReadString();
        }

        internal override void Process()
        {
            try
            {
                if (this.Device.PlayerState == State.LOGIN)
                {
                    if (Constants.LicensePlanID == 3)
                    {
                        if (ResourcesManager.m_vOnlinePlayers.Count >= Constants.MaxOnlinePlayers)
                        {
                            new LoginFailedMessage(Device)
                            {
                                ErrorCode = 12,
                                Reason = "Sorry the Server is currently full! \n\nPlease try again in a few Minutes.\n"
                            }.Send();
                            return;
                        }
                    }

                    if (ParserThread.GetMaintenanceMode())
                    {
                        new LoginFailedMessage(Device)
                        {
                            ErrorCode = 10,
                            RemainingTime = ParserThread.GetMaintenanceTime(),
                            Version = 8
                        }.Send();
                        return;
                    }

                    if (Constants.LicensePlanID < 1)
                    {
                        if (ResourcesManager.m_vOnlinePlayers.Count >= 350)
                        {
                            new LoginFailedMessage(Device)
                            {
                                ErrorCode = 11,
                                Reason = "This is a Free Version of CSS. Please Upgrade to Pro or Ultra Using CSS Keygen"
                            }.Send();
                            return;
                        }
                    }
                    else if (Constants.LicensePlanID < 2)
                    {
                        if (ResourcesManager.m_vOnlinePlayers.Count >= 700)
                        {
                            new LoginFailedMessage(Device)
                            {
                                ErrorCode = 11,
                                Reason =
                                    "This is a Pro Version of CSS. Please Upgrade to Ultra using CSS Keygen"
                            }.Send();
                            return;
                        }
                    }

                    int time = Convert.ToInt32(ConfigurationManager.AppSettings["maintenanceTimeleft"]);
                    if (time != 0)
                    {
                        new LoginFailedMessage(Device)
                        {
                            ErrorCode = 10,
                            RemainingTime = time,
                            Version = 8
                        }.Send();
                        return;
                    }

                    if (ConfigurationManager.AppSettings["CustomMaintenance"] != string.Empty)
                    {
                        new LoginFailedMessage(Device)
                        {
                            ErrorCode = 10,
                            Reason = Utils.ParseConfigString("CustomMaintenance")
                        }.Send();
                        return;
                    }

                    string serverVersion = ClientVersion;
                    string clientVersion = ConfigurationManager.AppSettings["ClientVersion"];

                    string[] sv = serverVersion.Split('.');
                    string[] cv = clientVersion.Split('.');

                    Console.WriteLine("Client version : " + clientVersion);

                    if (cv[0] != sv[0] || cv[1] != sv[1] || cv[2] != sv[2])
                    {
                        new LoginFailedMessage(Device)
                        {
                            ErrorCode = 8,
                            UpdateUrl = Utils.ParseConfigString("UpdateUrl")
                        }.Send();
                        return;
                    }

                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["useCustomPatch"]) &&
                        MasterHash != ObjectManager.FingerPrint.sha)
                    {
                        new LoginFailedMessage(Device)
                        {
                            ErrorCode = 7,
                            ResourceFingerprintData = ObjectManager.FingerPrint.SaveToJson(),
                            ContentUrl = Utils.ParseConfigString("patchingServer"),
                            UpdateUrl = Utils.ParseConfigString("UpdateUrl")
                        }.Send();
                        return;
                    }
                    CheckClient();
                }
            }
            catch
            {
                
            }
        }

        private async void LogUser()
        {
            ResourcesManager.LogPlayerIn(level, Device);
            level.Avatar.Region = Resources.Region.GetIpCountry(level.Avatar.IPAddress = Device.IPAddress);

            new LoginOkMessage(this.Device)
            {
                ServerMajorVersion = MajorVersion,
                ServerBuild = MinorVersion,
                ContentVersion = ContentVersion
            }.Send();

            if (level.Avatar.AllianceId > 0)
            {

                Alliance alliance = ObjectManager.GetAlliance(level.Avatar.AllianceId);
                if (alliance != null)
                {
                    new AllianceFullEntryMessage(this.Device, alliance).Send();
                    new AllianceStreamMessage(this.Device, alliance).Send();
                    new AllianceWarHistoryMessage(this.Device, alliance).Send();
                }
                else
                {
                    this.level.Avatar.AllianceId = 0;
                }
            }
            new OwnHomeDataMessage(this.Device, level).Send();
            new BookmarkMessage(this.Device).Send();

            ClientAvatar _clientAvatar = this.Device.Player.Avatar;
            int avatarScore = _clientAvatar.GetScore();
            int score = avatarScore-10;
            if (score > 18)
            {
                if(_clientAvatar.m_vShieldTime > 0 || _clientAvatar.m_vProtectionTime > 0)
                {
                    //Protection OR Shield Enabled
                }
                else
                {
                    _clientAvatar.SetScore(score);
                }
            }

            //Defended
            new AvatarStreamMessage(this.Device,7).Send();

            if (ResourcesManager.IsPlayerOnline(level))
            {
                AllianceMailStreamEntry mail = new AllianceMailStreamEntry();
                mail.ID = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                mail.SenderId = 0;
                //mail.SetSenderName("Clash Of Heroes Team");
                mail.m_vSenderName = "DARK TEAM";
                mail.IsNew = 2;
                mail.AllianceId = 0;
                mail.m_vSenderLeagueId = 22;
                mail.AllianceBadgeData = 1526735450;
                //mail.SetAllianceName("COH-TEAM");
                mail.AllianceName = "DARK";
                mail.Message = ConfigurationManager.AppSettings["AdminMessage"];
                mail.m_vSenderLevel = 500;
                AvatarStreamEntryMessage p = new AvatarStreamEntryMessage(level.Client);
                p.SetAvatarStreamEntry(mail);
                p.Send();
            }
        }

        private async void CheckClient()
        {
            try
            {
                if (UserID == 0 || string.IsNullOrEmpty(UserToken))
                {
                     NewUser();
                     return;
                }

                level = await ResourcesManager.GetPlayer(UserID);
                if (level != null)
                {
                    if (level.Avatar.AccountBanned)
                    {
                        new LoginFailedMessage(Device) {ErrorCode = 11}.Send();
                        return;
                    }
                    if (string.Equals(level.Avatar.UserToken, UserToken, StringComparison.Ordinal))
                    {
                        LogUser();
                    }
                    else
                    {
                        new LoginFailedMessage(Device)
                        {
                            ErrorCode = 11,
                            Reason = "We have some Problems with your Account. Please clean your App Data."
                        }.Send();
                        return;
                    }
                }
                else
                {
                    new LoginFailedMessage(Device)
                    {
                        ErrorCode = 11,
                        Reason = "We have some Problems with your Account. Please clean your App Data."
                    }.Send();
                    return;
                }
            } catch (Exception) { }
        }

        private void NewUser()
        {
            level = ObjectManager.CreateAvatar(0, null);
            if (string.IsNullOrEmpty(UserToken))
            {
                for (int i = 0; i < 20; i++)
                {
                    char letter = (char)Resources.Random.Next('A', 'Z');
                    this.level.Avatar.UserToken +=  letter;
                }
            }
            
            level.Avatar.InitializeAccountCreationDate();
            level.Avatar.m_vAndroid = this.Android;

            Resources.DatabaseManager.Save(level);
            LogUser();
        }
    }
}
