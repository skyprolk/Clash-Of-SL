using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using CSS.Helpers;
using CSS.Utilities.Sodium;
using CSS.Core.Settings;
using CSS.Helpers.Binary;
using CSS.Helpers.List;
using CSS.Logic.Enums;

namespace CSS.Packets
{
    internal class Message
    {
        internal ushort Identifier;
        internal ushort Length;
        internal ushort Version;

        internal int Offset;

        internal Device Device;

        internal Reader Reader;

        internal List<byte> Data;

        internal Message(Device Device)
        {
            this.Device = Device;
            this.Data = new List<byte>(Constants.SendBuffer);
        }

        internal Message(Device Device, Reader Reader)
        {
            this.Device = Device;
            this.Reader = Reader;
        }

        internal byte[] ToBytes
        {
            get
            {
                List<byte> Packet = new List<byte>();

                Packet.AddUShort(this.Identifier);
                Packet.Add(0);
                Packet.AddUShort(this.Length);
                Packet.AddUShort(this.Version);
                Packet.AddRange(this.Data);

                return Packet.ToArray();
            }
        }

        internal virtual void Decode()
        {

        }

        internal virtual void Encode()
        {

        }

        internal virtual void Process()
        {

        }

        internal virtual void Decrypt()
        {
            if (this.Device.PlayerState >= State.LOGGED)
            {
                this.Device.Keys.SNonce.Increment();

                byte[] Decrypted = Sodium.Decrypt(new byte[16].Concat(this.Reader.ReadBytes(this.Length)).ToArray(), this.Device.Keys.SNonce, this.Device.Keys.PublicKey);

                if (Decrypted == null)
                {
                    throw new CryptographicException("Tried to decrypt an incomplete message.");
                }

                this.Reader = new Reader(Decrypted);
                this.Length = (ushort)this.Reader.BaseStream.Length;
            }
        }

        internal virtual void Encrypt()
        {
            if (this.Device.PlayerState >= State.LOGGED)
            {
                this.Device.Keys.RNonce.Increment();

                this.Data = new List<byte>(Sodium.Encrypt(this.Data.ToArray(), this.Device.Keys.RNonce, this.Device.Keys.PublicKey).Skip(16).ToArray());
            }

            this.Length = (ushort)this.Data.Count;
        }
    }
}
