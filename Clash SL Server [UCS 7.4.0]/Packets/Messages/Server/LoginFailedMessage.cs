using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using CSS.Core.Crypto;
using CSS.Helpers.List;
using CSS.Logic.Enums;
using CSS.Utilities.Blake2B;
using CSS.Utilities.Sodium;

namespace CSS.Packets.Messages.Server
{
    // Packet 20103
    internal class LoginFailedMessage : Message
    {
        public LoginFailedMessage(Device client) : base(client)
        {
            this.Identifier = 20103;
            this.UpdateUrl = ConfigurationManager.AppSettings["UpdateUrl"];
            this.Version = 9;

            // 8  : Update
            // 10 : Maintenance
            // 11 : Banned
            // 12 : Timeout
            // 13 : Locked Account
        }

        internal string ContentUrl;
        internal string Reason;
        internal string RedirectDomain;
        internal string ResourceFingerprintData;
        internal string UpdateUrl;

        internal int ErrorCode;
        internal int RemainingTime;

        internal override void Encrypt()
        {
            if (this.Device.PlayerState >= State.LOGIN)
            {
                Blake2BHasher blake = new Blake2BHasher();

                blake.Update(this.Device.Keys.SNonce);
                blake.Update(this.Device.Keys.PublicKey);
                blake.Update(Key.PublicKey);

                byte[] Nonce = blake.Finish();
                byte[] encrypted = this.Device.Keys.RNonce.Concat(this.Device.Keys.PublicKey).Concat(this.Data).ToArray();

                this.Data = new List<byte>(Sodium.Encrypt(encrypted, Nonce, Key.PrivateKey, this.Device.Keys.PublicKey));
            }

            this.Length = (ushort) this.Data.Count;
        }

        internal override void Encode()
        {
            this.Data.AddInt(this.ErrorCode);
            this.Data.AddString(this.ResourceFingerprintData);
            this.Data.AddString(this.RedirectDomain);
            this.Data.AddString(this.ContentUrl);
            this.Data.AddString(this.UpdateUrl);
            this.Data.AddString(this.Reason);
            this.Data.AddInt(this.RemainingTime);
            this.Data.AddInt(-1);
            this.Data.Add(0);
            this.Data.AddInt(-1);
            this.Data.AddInt(-1);
            this.Data.AddInt(-1);
            this.Data.AddInt(-1);
        }
    }
}
