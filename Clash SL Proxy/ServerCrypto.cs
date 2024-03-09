using Sodium;
using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using CSP;

namespace CSP
{
    public class ServerCrypto : Protocol
    {
        protected static KeyPair serverKey = PublicKeyBox.GenerateKeyPair(Utilities.HexToBinary("1891D401FADB51D25D3A9174D472A9F691A45B974285D47729C45C6538070D85"));

        public static void DecryptPacket(Socket socket, ServerState state, byte[] packet)
        {
            int messageId = BitConverter.ToInt32(new byte[2].Concat(packet.Take(2)).Reverse().ToArray(), 0);
            int payloadLength = BitConverter.ToInt32(new byte[1].Concat(packet.Skip(2).Take(3)).Reverse().ToArray(), 0);
            int unknown = BitConverter.ToInt32(new byte[2].Concat(packet.Skip(2).Skip(3).Take(2)).Reverse().ToArray(), 0);
            byte[] cipherText = packet.Skip(2).Skip(3).Skip(2).ToArray();
            byte[] plainText;

            if (messageId == 10100)
            {
                plainText = cipherText;
            }
            else if (messageId == 10101)
            {
                state.clientKey         = cipherText.Take(32).ToArray();
                byte[] nonce            = GenericHash.Hash(state.clientKey.Concat(state.serverKey.PublicKey).ToArray(), null, 24);
                cipherText              = cipherText.Skip(32).ToArray();
                plainText               = PublicKeyBox.Open(cipherText, nonce, state.serverKey.PrivateKey, state.clientKey);
                state.sessionKey        = plainText.Take(24).ToArray();
                state.clientState.nonce = plainText.Skip(24).Take(24).ToArray();
                plainText               = plainText.Skip(24).Skip(24).ToArray();
            }
            else
            {
                state.clientState.nonce = Utilities.Increment(Utilities.Increment(state.clientState.nonce));
                plainText = SecretBox.Open(new byte[16].Concat(cipherText).ToArray(), state.clientState.nonce,state.sharedKey);
            }

            if (messageId == 14102)
            {
                using (PacketReader _Reader = new PacketReader(new MemoryStream(plainText)))
                {
                    _Reader.ReadInt32();
                    _Reader.ReadInt32();
                    int Count = _Reader.ReadInt32();
                    byte[] Commands = _Reader.ReadBytes((int)(_Reader.BaseStream.Length - _Reader.BaseStream.Position));

                    using (PacketReader _Reader2 = new PacketReader(new MemoryStream(Commands)))
                    {
                        int CommandID = _Reader2.ReadInt32();
                        for (int i = 0; i < Count; i++)
                        {
                            if (Count > -1 && CommandID > 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.Write("[COC]");
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.Write("[   COMMAND   ]");
                                Console.ResetColor();
                                Console.WriteLine("    {0}", CommandInfos.GetPacketName(CommandID));
                            }
                        }
                    }
                }
                ClientCrypto.EncryptPacket(state.clientState.socket, state.clientState, messageId, unknown, plainText);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("[COC]");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("[CLIENT_PACKET]");
                Console.ResetColor();
                Console.WriteLine("    {0}", PacketInfos.GetPacketName(messageId));

                string packetid = PacketInfos.GetPacketName(messageId);
                string path = "Packets/" + packetid + ".txt";
                using (StreamWriter sw = new StreamWriter(path, true))
                {
                    sw.WriteLine();
                    sw.WriteLine(Utilities.BinaryToHex(plainText).ToUpper());
                }

                ClientCrypto.EncryptPacket(state.clientState.socket, state.clientState, messageId, unknown, plainText);
            }
        }

        public static void EncryptPacket(Socket _Socket, ServerState _State, int _PacketID, int unknown, byte[] plainText)
        {
            byte[] cipherText;

            if (_PacketID == 20100)
            {
                cipherText = plainText;
            }
            else if (_PacketID == 20103)
            {
                byte[] nonce = GenericHash.Hash(_State.clientState.nonce.Concat(_State.clientKey).Concat(_State.serverKey.PublicKey).ToArray(), null, 24);
                plainText    = _State.nonce.Concat(_State.sharedKey).Concat(plainText).ToArray();
                cipherText   = PublicKeyBox.Create(plainText, nonce, _State.serverKey.PrivateKey, _State.clientKey);
            }
            else if (_PacketID == 20104)
            {
                byte[] nonce = GenericHash.Hash(_State.clientState.nonce.Concat(_State.clientKey).Concat(_State.serverKey.PublicKey).ToArray(), null, 24);
                plainText    = _State.nonce.Concat(_State.sharedKey).Concat(plainText).ToArray();
                cipherText   = PublicKeyBox.Create(plainText, nonce, _State.serverKey.PrivateKey, _State.clientKey);
            }
            else
            {
                cipherText = SecretBox.Create(plainText, _State.nonce, _State.sharedKey).Skip(16).ToArray();
            }

            byte[] packet = BitConverter.GetBytes(_PacketID).Reverse().Skip(2).Concat(BitConverter.GetBytes(cipherText.Length).Reverse().Skip(1)).Concat(BitConverter.GetBytes(unknown).Reverse().Skip(2)).Concat(cipherText).ToArray();
            _Socket.BeginSend(packet, 0, packet.Length, 0, SendCallback, _State);
        }
    }
}
