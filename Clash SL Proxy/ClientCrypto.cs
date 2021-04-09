using Sodium;
using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;

namespace CSP
{
    public class ClientCrypto : Protocol
    {
        protected static byte[] serverKey = Utilities.HexToBinary("4102C28189897A48CEDFA8C6E5378F55624F9E8408FA8A376643DBBCE715B21A".ToLower()); //8.709.23 Key

        protected KeyPair clientKey = PublicKeyBox.GenerateKeyPair();

        public static void DecryptPacket(Socket socket, ClientState state, byte[] packet)
        {
            int messageId = BitConverter.ToInt32(new byte[2].Concat(packet.Take(2)).Reverse().ToArray(), 0);
            int payloadLength = BitConverter.ToInt32(new byte[1].Concat(packet.Skip(2).Take(3)).Reverse().ToArray(), 0);
            int unknown = BitConverter.ToInt32(new byte[2].Concat(packet.Skip(2).Skip(3).Take(2)).Reverse().ToArray(), 0);
            byte[] cipherText = packet.Skip(2).Skip(3).Skip(2).ToArray();
            byte[] plainText;

            if (messageId == 20100)
            {
                plainText = cipherText;
            }
            else if (messageId == 20103)
            {
                byte[] nonce = GenericHash.Hash(state.nonce.Concat(state.clientKey.PublicKey).Concat(state.serverKey).ToArray(),null, 24);
                plainText = PublicKeyBox.Open(cipherText, nonce, state.clientKey.PrivateKey, state.serverKey);
                state.serverState.nonce = plainText.Take(24).ToArray();
                state.serverState.sharedKey = plainText.Skip(24).Take(32).ToArray();
                plainText = plainText.Skip(24).Skip(32).ToArray();
            }
            else if (messageId == 20104)
            {
                byte[] nonce = GenericHash.Hash(state.nonce.Concat(state.clientKey.PublicKey).Concat(state.serverKey).ToArray(), null, 24);
                plainText = PublicKeyBox.Open(cipherText, nonce, state.clientKey.PrivateKey, state.serverKey);
                state.serverState.nonce = plainText.Take(24).ToArray();
                state.serverState.sharedKey = plainText.Skip(24).Take(32).ToArray();
                plainText = plainText.Skip(24).Skip(32).ToArray();
            }
            else
            {
                state.serverState.nonce = Utilities.Increment(Utilities.Increment(state.serverState.nonce));
                plainText = SecretBox.Open(new byte[16].Concat(cipherText).ToArray(), state.serverState.nonce, state.serverState.sharedKey);
            }

            if (messageId == 24715)
            {
                using (PacketReader _Reader = new PacketReader(new MemoryStream(plainText)))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("[COC]");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("[ GLOBAL_CHAT ]    ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("'" + _Reader.ReadString() + " ' from '" + _Reader.ReadString() + "'.");
                    Console.ResetColor();
                }

                ServerCrypto.EncryptPacket(state.serverState.socket, state.serverState, messageId, unknown, plainText);
            }
            else if (messageId == 24114)
            {
                using (PacketReader _Reader = new PacketReader(new MemoryStream(plainText)))
                {
                    Console.WriteLine(_Reader.ReadCompressedString());
                }

                ServerCrypto.EncryptPacket(state.serverState.socket, state.serverState, messageId, unknown, plainText);
            }
            else if (messageId == 20106)
            {
                using (PacketReader _Reader = new PacketReader(new MemoryStream(plainText)))
                {
                    Console.WriteLine(_Reader.ReadInt64()); // player id
                    Console.WriteLine(_Reader.ReadByte()); // 0 = revert back, 1 = sent;
                    Console.WriteLine(BitConverter.ToString(_Reader.ReadBytes()).Replace("-", ""));
                }

                ServerCrypto.EncryptPacket(state.serverState.socket, state.serverState, messageId, unknown, plainText);
            }
            else if (messageId == 24335)
            {
                using (PacketReader _Reader = new PacketReader(new MemoryStream(plainText)))
                {
                    Console.WriteLine("War State:           " + _Reader.ReadInt32());
                    Console.WriteLine("Time Left:           " + _Reader.ReadInt32());
                    Console.WriteLine("Alliance ID:         " + _Reader.ReadInt64());
                    Console.WriteLine("Alliance Name:       " + _Reader.ReadString());
                    Console.WriteLine("Alliance Badge Data: " + _Reader.ReadInt32());
                    Console.WriteLine("Alliance Level:      " + _Reader.ReadInt32());
                    Console.WriteLine("War Members:         " + _Reader.ReadInt64());

                    for (int i = 1; i < 2; i++)
                    {
                        Console.WriteLine("PL" + i + " Alliance ID : " + _Reader.ReadInt64());
                        Console.WriteLine("PL" + i + " Player   ID : " + _Reader.ReadInt64());
                        Console.WriteLine("PL" + i + " Home     ID : " + _Reader.ReadInt64());
                        Console.WriteLine("PL" + i + " Name        : " + _Reader.ReadString());
                        Console.WriteLine("PL" + i + " Stars       : " + _Reader.ReadInt32());
                        Console.WriteLine("PL" + i + " Damage      : " + _Reader.ReadInt32());
                        Console.WriteLine("PL" + i + " Unknown     : " + _Reader.ReadInt32());
                        Console.WriteLine("PL" + i + " Attack Used : " + _Reader.ReadInt32());
                        Console.WriteLine("PL" + i + " Got Attacked: " + _Reader.ReadInt32());
                        Console.WriteLine("PL" + i + " Gold Gain   : " + _Reader.ReadInt32());
                        Console.WriteLine("PL" + i + " Elixir Gain : " + _Reader.ReadInt32());
                        Console.WriteLine("PL" + i + " DElixir Gain: " + _Reader.ReadInt32());
                        Console.WriteLine("PL" + i + " Gold A.     : " + _Reader.ReadInt32());
                        Console.WriteLine("PL" + i + " Elixir A.   : " + _Reader.ReadInt32());
                        Console.WriteLine("PL" + i + " DElixir A.  : " + _Reader.ReadInt32());
                        Console.WriteLine("PL" + i + " Unknown     : " + _Reader.ReadInt32());
                        Console.WriteLine("PL" + i + " Offences Weight: " + _Reader.ReadInt32());
                        Console.WriteLine("PL" + i + " Defenses Weight: " + _Reader.ReadInt32());
                        Console.WriteLine("PL" + i + " Unknown     : " + _Reader.ReadInt32());
                        Console.WriteLine("PL" + i + " TH Level    : " + (_Reader.ReadInt32() + 1));
                        Console.WriteLine("PL" + i + " Unknown     : " + _Reader.ReadInt32());
                        Console.WriteLine("PL" + i + " Unknown     : " + BitConverter.ToString(_Reader.ReadBytes()).Replace("-", ""));
                    }
                }

                ServerCrypto.EncryptPacket(state.serverState.socket, state.serverState, messageId, unknown, plainText);
            }
            else if (messageId == 20103)
            {
                using (PacketReader _Reader = new PacketReader(new MemoryStream(plainText)))
                {
                    Console.WriteLine("Error Code      : " + _Reader.ReadInt32());
                    Console.WriteLine("Fingerprint Data: " + _Reader.ReadString());
                    Console.WriteLine("Redirect URL    : " + _Reader.ReadString());
                    Console.WriteLine("Patch URL       : " + _Reader.ReadString());
                    Console.WriteLine("Update URL      : " + _Reader.ReadString());
                    Console.WriteLine("Reason          : " + _Reader.ReadInt32());
                    Console.WriteLine("Remaining Time  : " + _Reader.ReadInt32());
                }

                ServerCrypto.EncryptPacket(state.serverState.socket, state.serverState, messageId, unknown, plainText);
            }
            else if (messageId == 24310)
            {
                using (PacketReader _Reader = new PacketReader(new MemoryStream(plainText)))
                {
                    Console.WriteLine("Search String   : " + _Reader.ReadString());
                    int Count = _Reader.ReadInt32();

                    Console.WriteLine("Count: " + Count);

                    for (int i = 0; i < Count; i++)
                    {
                        Console.WriteLine("Alliance ID     : " + _Reader.ReadInt64());
                        Console.WriteLine("Alliance Name   : " + _Reader.ReadString());
                        Console.WriteLine("Alliance Badge  : " + _Reader.ReadInt32());
                        Console.WriteLine("Alliance Type   : " + _Reader.ReadInt32());
                        Console.WriteLine("Alliance M Count: " + _Reader.ReadInt32());
                        Console.WriteLine("Alliance Score  : " + _Reader.ReadInt32());
                        Console.WriteLine("Alliance R Score: " + _Reader.ReadInt32());
                        Console.WriteLine("Alliance W Wars : " + _Reader.ReadInt32());
                        Console.WriteLine("Alliance L Wars : " + _Reader.ReadInt32());
                        Console.WriteLine("Alliance D Wars : " + _Reader.ReadInt32());
                        Console.WriteLine("Alliance Region : " + _Reader.ReadInt32());
                        Console.WriteLine("Alliance War F  : " + _Reader.ReadInt32());
                        Console.WriteLine("Alliance Orgin  : " + _Reader.ReadInt32());
                        Console.WriteLine("Alliance Exp    : " + _Reader.ReadInt32());
                        Console.WriteLine("Alliance Level  : " + _Reader.ReadInt32());
                        Console.WriteLine("Unknown         : " + _Reader.ReadInt32());
                        Console.WriteLine("Unknown         : " + _Reader.ReadInt32());
                        Console.WriteLine("War Log Public  : " + _Reader.ReadInt32());
                        Console.WriteLine("Friendly War    : " + _Reader.ReadInt32());
                    }
                }

                ServerCrypto.EncryptPacket(state.serverState.socket, state.serverState, messageId, unknown, plainText);
            }
            else
            {

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("[COC]");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("[SERVER_PACKET]");
                Console.ResetColor();
                Console.WriteLine("    {0} ", PacketInfos.GetPacketName(messageId));

                string packetid = PacketInfos.GetPacketName(messageId);
                string path = "Packets/" + packetid + ".txt";
                using (StreamWriter sw = new StreamWriter(path, true))
                {
                    sw.WriteLine();
                    sw.WriteLine(Utilities.BinaryToHex(plainText).ToUpper());
                }

                ServerCrypto.EncryptPacket(state.serverState.socket, state.serverState, messageId, unknown, plainText);
            }
        }

        public static void EncryptPacket(Socket _Socket, ClientState _State, int messageId, int unknown, byte[] plainText)
        {
            byte[] cipherText;

            if (messageId == 10100)
            {
                cipherText = plainText;
            }
            else if (messageId == 10101)
            {
                byte[] nonce = GenericHash.Hash(_State.clientKey.PublicKey.Concat(_State.serverKey).ToArray(), null, 24);
                plainText    = _State.serverState.sessionKey.Concat(_State.nonce).Concat(plainText).ToArray();
                cipherText   = PublicKeyBox.Create(plainText, nonce, _State.clientKey.PrivateKey, _State.serverKey);
                cipherText   = _State.clientKey.PublicKey.Concat(cipherText).ToArray();
            }
            else
            {
                cipherText = SecretBox.Create(plainText, _State.nonce, _State.serverState.sharedKey).Skip(16).ToArray();
            }

            byte[] packet = BitConverter.GetBytes(messageId).Reverse().Skip(2).Concat(BitConverter.GetBytes(cipherText.Length).Reverse().Skip(1)).Concat(BitConverter.GetBytes(unknown).Reverse().Skip(2)).Concat(cipherText).ToArray();
            _Socket.BeginSend(packet, 0, packet.Length, 0, SendCallback, _State);
        }
    }
}
