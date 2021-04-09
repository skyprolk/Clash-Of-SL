using System;
using System.Configuration;
using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Core.Settings;
using UCS.Helpers;
using UCS.Logic;
using UCS.Packets.Messages.Server;

namespace UCS.Packets.Messages.Client
{
    // Packet 10100
    internal class SessionRequest : Message
    {

        public SessionRequest(Packets.Client client, PacketReader br) : base(client, br)
        {
        }

        public string Hash;
        public int MajorVersion;
        public int MinorVersion;
        public int Protocol;
        public int KeyVersion;
        public int Unknown;
        public int Device;
        public int Store;

        public override void Decode()
        {
            using (PacketReader reader = new PacketReader(new MemoryStream(GetData())))
            {
                Protocol = reader.ReadInt32();
                KeyVersion = reader.ReadInt32();
                MajorVersion = reader.ReadInt32();
                Unknown = reader.ReadInt32();
                MinorVersion = reader.ReadInt32();
                Hash = reader.ReadString();
                Device = reader.ReadInt32();
                Store = reader.ReadInt32();
            }
        }

        public override void Process(Level level)
        {
            if (Constants.IsRc4)
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["patchingServer"]))
                {
                    LoginFailedMessage p = new LoginFailedMessage(Client);
                    p.SetErrorCode(7);
                    p.SetResourceFingerprintData(ObjectManager.FingerPrint.SaveToJson());
                    p.SetContentURL(ConfigurationManager.AppSettings["patchingServer"]);
                    p.SetUpdateURL(ConfigurationManager.AppSettings["UpdateUrl"]);
                    PacketProcessor.Send(p);
                }
            }
            else
            PacketProcessor.Send(new HandshakeSuccess(Client, this));
        }

    }
}