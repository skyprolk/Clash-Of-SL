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
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers;
using CSS.Logic;
using CSS.Logic.AvatarStreamEntry;
using CSS.PacketProcessing.Messages.Server;

namespace CSS.PacketProcessing.Messages.Client
{
    internal class LoginMessage : Message
    {
        #region Public Constructors

        public LoginMessage(PacketProcessing.Client client, CoCSharpPacketReader br) : base(client, br)
        {

        }

        #endregion Public Constructors

        #region Public Fields

        public string AdvertisingGUID;
        public string AndroidDeviceID;
        public string ClientVersion;
        public int ContentVersion;
        public string DeviceModel;
        public string FacebookDistributionID;
        public bool IsAdvertisingTrackingEnabled;
        public string Region;
        public int LocaleKey;
        public string MacAddress;
        public int MajorVersion;
        public string MasterHash;
        public int MinorVersion;
        public string OpenUDID;
        public string OSVersion;
        public int Seed;
        public int Unknown;
        public string Unknown1;
        public byte Unknown2;
        public string Unknown3;
        public byte Unknown4;
        public string Unknown5;
        public string Unknown6;
        public long UserID;
        public string UserToken;
        public string VendorGUID;
        public Level level;

        #endregion Public Fields

        #region Public Methods

        public override void Decode()
        {
            if (Client.CState == 1)
            {
                try
                {
                    using (var reader = new CoCSharpPacketReader(new MemoryStream(GetData())))
                    {
                        UserID = reader.ReadInt64();
                        UserToken = reader.ReadString();
                        MajorVersion = reader.ReadInt32();
                        ContentVersion = reader.ReadInt32();
                        MinorVersion = reader.ReadInt32();
                        MasterHash = reader.ReadString();
                        reader.ReadString();
                        OpenUDID = reader.ReadString();
                        MacAddress = reader.ReadString();
                        DeviceModel = reader.ReadString();
                        LocaleKey = reader.ReadInt32();
                        Region = reader.ReadString();
                        AdvertisingGUID = reader.ReadString();
                        OSVersion = reader.ReadString();
                        Unknown2 = reader.ReadByte();
                        Unknown3 = reader.ReadString();
                        AndroidDeviceID = reader.ReadString();
                        FacebookDistributionID = reader.ReadString();
                        IsAdvertisingTrackingEnabled = reader.ReadBoolean();
                        VendorGUID = reader.ReadString();
                        Seed = reader.ReadInt32();
                        reader.ReadByte();
                        reader.ReadString();
                        reader.ReadString();
                        ClientVersion = reader.ReadString();
                    }
                }
                catch (Exception e)
                {
                    Client.CState = 0;
                }
            }
        }

        public override void Process(Level a)
        {
            if (Client.CState >= 1)
            {
                int time = Convert.ToInt32(ConfigurationManager.AppSettings["maintenanceTimeleft"]);
                if (time != 0)
                {
                    var p = new LoginFailedMessage(Client);
                    p.SetErrorCode(10);
                    p.RemainingTime(time);
                    p.SetMessageVersion(8);
                    PacketManager.ProcessOutgoingPacket(p);
                    return;
                }

                var cv = ClientVersion.Split('.');
                Console.WriteLine("Client Version : " + ClientVersion);
                Console.WriteLine("Sever Version : 8.332.*");
                if (cv[0] != "8" || cv[1] != "332")
                {
                    var p = new LoginFailedMessage(Client);
                    p.SetErrorCode(8);
                    p.SetUpdateURL(Convert.ToString(ConfigurationManager.AppSettings["UpdateUrl"]));
                    PacketManager.ProcessOutgoingPacket(p);
                    return;
                }

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["useCustomPatch"]) &&
                    MasterHash != ObjectManager.FingerPrint.sha)
                {
                    var p = new LoginFailedMessage(Client);
                    p.SetErrorCode(7);
                    p.SetResourceFingerprintData(ObjectManager.FingerPrint.SaveToJson());
                    p.SetContentURL(ConfigurationManager.AppSettings["patchingServer"]);
                    p.SetUpdateURL(ConfigurationManager.AppSettings["UpdateUrl"]);
                    PacketManager.ProcessOutgoingPacket(p);
                    return;
                }
                CheckClient();
            }
        }

        void LogUser()
        {

            ResourcesManager.LogPlayerIn(level, Client);
            level.Tick();
            var loginOk = new LoginOkMessage(Client);
            var avatar = level.GetPlayerAvatar();
            loginOk.SetAccountId(avatar.GetId());
            loginOk.SetPassToken(avatar.GetUserToken());
            loginOk.SetServerMajorVersion(MajorVersion);
            loginOk.SetServerBuild(MinorVersion);
            loginOk.SetContentVersion(ContentVersion);
            loginOk.SetServerEnvironment("prod");
            loginOk.SetDaysSinceStartedPlaying(0);
            loginOk.SetServerTime(Math.Round(level.GetTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds * 1000).ToString(CultureInfo.InvariantCulture));
            loginOk.SetAccountCreatedDate("1414003838000");
            loginOk.SetStartupCooldownSeconds(0);
            loginOk.SetCountryCode(avatar.GetUserRegion().ToUpper());
            PacketManager.ProcessOutgoingPacket(loginOk);
            PacketManager.ProcessOutgoingPacket(new OwnHomeDataMessage(Client, level));
            var alliance = ObjectManager.GetAlliance(level.GetPlayerAvatar().GetAllianceId());
            if (ResourcesManager.IsPlayerOnline(level))
            {
                var mail = new AllianceMailStreamEntry();
                mail.SetId((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                mail.SetSenderId(0);
                mail.SetSenderAvatarId(0);
                mail.SetSenderName("DARK TEAM");
                mail.SetIsNew(0);
                mail.SetAllianceId(0);
                mail.SetSenderLeagueId(22);
                mail.SetAllianceBadgeData(1728059989);
                mail.SetAllianceName("DARK ADMIN");
                mail.SetMessage(ConfigurationManager.AppSettings["AdminMessage"]);
                mail.SetSenderLevel(500);
                var p = new AvatarStreamEntryMessage(level.GetClient());
                p.SetAvatarStreamEntry(mail);
                PacketManager.ProcessOutgoingPacket(p);
            }

            if (alliance != null)
            {
                PacketManager.ProcessOutgoingPacket (new AllianceFullEntryMessage(Client, alliance));
                PacketManager.ProcessOutgoingPacket (new AllianceStreamMessage(Client, alliance));
                PacketManager.ProcessOutgoingPacket (new AllianceWarHistoryMessage(Client));
            }
            PacketManager.ProcessOutgoingPacket (new BookmarkMessage(Client));
        }

        void CheckClient()
        {
            if (UserID == 0 || string.IsNullOrEmpty(UserToken))
            {
                NewUser();
                return;
            }

            level = ResourcesManager.GetPlayer(UserID);
            if (level != null)
            {
                if (level.Banned())
                {
                    var p = new LoginFailedMessage(Client);
                    p.SetErrorCode(11);
                    PacketManager.ProcessOutgoingPacket(p);
                    return;
                }
                if (string.Equals(level.GetPlayerAvatar().GetUserToken(), UserToken, StringComparison.Ordinal))
                {
                    LogUser();
                }
                else
                {
                    var p = new LoginFailedMessage(Client);
                    p.SetErrorCode(11);
                    p.SetReason("We have detected unrecognized token sended from your devices. Please clean your App Data.");
                    PacketManager.ProcessOutgoingPacket(p);
                    return;
                }
            }
            else
            {
                var p = new LoginFailedMessage(Client);
                p.SetErrorCode(11);
                p.SetReason("We have detected unrecognized token sended from your devices. Please clean your App Data.");
                PacketManager.ProcessOutgoingPacket(p);
                return;
            }
        }

        void NewUser()
        {
            level = ObjectManager.CreateAvatar(0, null);
            if (string.IsNullOrEmpty(UserToken))
            {
                byte[] tokenSeed = new byte[20];
                new Random().NextBytes(tokenSeed);
                using (SHA1 sha = new SHA1CryptoServiceProvider())
                    UserToken = BitConverter.ToString(sha.ComputeHash(tokenSeed)).Replace("-", string.Empty);
            }
            level.GetPlayerAvatar().SetRegion(Region.ToUpper());
            level.GetPlayerAvatar().SetToken(UserToken);
            if (!string.IsNullOrEmpty(AndroidDeviceID))
                DatabaseManager.Singelton.Save(level);
            LogUser();
        }

        #endregion Public Methods
    }
}
