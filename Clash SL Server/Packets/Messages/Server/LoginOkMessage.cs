using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSS.Core.Crypto;
using CSS.Helpers;
using CSS.Helpers.List;
using CSS.Logic;
using CSS.Logic.API;
using CSS.Logic.Enums;
using CSS.Utilities.Blake2B;
using CSS.Utilities.Sodium;

namespace CSS.Packets.Messages.Server
{
    // Packet 20104
    internal class LoginOkMessage : Message
    {
        public LoginOkMessage(Device client) : base(client)
        {
            this.Identifier = 20104;
            this.Device.PlayerState = State.LOGGED;
        }

        internal int ServerBuild;
        internal int ServerMajorVersion;
        internal int ContentVersion;

        internal override void Encode()
        {
            ClientAvatar avatar = this.Device.Player.Avatar;
            this.Data.AddLong(avatar.UserId);
            this.Data.AddLong(avatar.UserId);

            this.Data.AddString(avatar.UserToken);

            this.Data.AddString(avatar.FacebookId);
            this.Data.AddString(null);


            this.Data.AddInt(ServerMajorVersion);
            this.Data.AddInt(ServerBuild);
            this.Data.AddInt(ContentVersion);

            this.Data.AddString("prod");

            this.Data.AddInt(3); //Session Count
            this.Data.AddInt(490); //Playtime Second
            this.Data.AddInt(0);

            this.Data.AddString(FacebookApi.ApplicationID);

            this.Data.AddString("1482970881296"); // 14 75 26 87 86 11 24 33
            this.Data.AddString("1482952262000"); // 14 78 03 95 03 10 0

            this.Data.AddInt(0);
            this.Data.AddString(avatar.GoogleId);
            this.Data.AddString(avatar.Region.ToUpper());
            this.Data.AddString(null);
            this.Data.AddInt(1);

        }

        internal override void Encrypt()
        {
            Blake2BHasher blake = new Blake2BHasher();

            blake.Update(this.Device.Keys.SNonce);
            blake.Update(this.Device.Keys.PublicKey);
            blake.Update(Key.PublicKey);

            byte[] Nonce = blake.Finish();
            byte[] encrypted = this.Device.Keys.RNonce.Concat(this.Device.Keys.PublicKey).Concat(this.Data).ToArray();

            this.Data =  new List<byte>(Sodium.Encrypt(encrypted, Nonce, Key.PrivateKey, this.Device.Keys.PublicKey));

            this.Length = (ushort) this.Data.Count;
        }
    }
}
