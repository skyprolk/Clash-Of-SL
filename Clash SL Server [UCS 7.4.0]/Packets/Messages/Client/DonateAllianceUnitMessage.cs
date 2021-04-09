using System;
using System.IO;
using CSS.Core;
using CSS.Core.Network;
using CSS.Files.Logic;
using CSS.Helpers;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Logic.StreamEntry;
using CSS.Packets.Commands.Client;
using CSS.Packets.Commands.Server;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Messages.Client
{
    // Packet 14310
    internal class DonateAllianceUnitMessage : Message
    {
        public CombatItemData Troop;
        public int MessageID;
        public byte BuyTroop;

        public DonateAllianceUnitMessage(Device device, Reader reader) : base(device, reader)
        {
        }

        internal override void Decode()
        {
            this.Reader.ReadInt32();
            this.Troop = (CombatItemData) this.Reader.ReadDataReference();
            this.Reader.ReadInt32();
            this.MessageID = this.Reader.ReadInt32();
            this.BuyTroop = this.Reader.ReadByte();
        }

        internal override async void Process()
        {
            try
            {
                /*if (this.BuyTroop >= 1)
                {     
                    this.Device.Player.Avatar.SetDiamonds(this.Device.Player.Avatar.m_vCurrentGems - Troop.GetHousingSpace());
                }*/

                Alliance a = ObjectManager.GetAlliance(this.Device.Player.Avatar.AllianceId);
                StreamEntry _Stream = a.m_vChatMessages.Find(c => c.ID == MessageID);
                Level _Sender = await ResourcesManager.GetPlayer(_Stream.SenderID);
                int upcomingspace = _Stream.m_vDonatedTroop + Troop.GetHousingSpace();

                if (upcomingspace <= _Stream.m_vMaxTroop)
                {
                    
                    DonatedAllianceUnitCommand _Donated = new DonatedAllianceUnitCommand(this.Device);
                    _Donated.Tick(_Sender);
                    _Donated.MessageID = this.MessageID;
                    _Donated.TroopID = Troop.GetGlobalID();

                    new AvailableServerCommandMessage(this.Device, _Donated.Handle()).Send();

                    ReceivedAllianceUnitCommand _Received = new ReceivedAllianceUnitCommand(this.Device);
                    _Received.Donator_Name = this.Device.Player.Avatar.AvatarName;
                    _Received.TroopID = this.Troop.GetGlobalID();
                    _Received.Troop_Level = this.Device.Player.Avatar.GetUnitUpgradeLevel(Troop);

                    new AvailableServerCommandMessage(_Sender.Client, _Received).Send();

                    Level _PreviousPlayer = await ResourcesManager.GetPlayer(_Stream.SenderID);
                    ClientAvatar _PreviousPlayerAvatar = _PreviousPlayer.Avatar;
                    _Stream.AddDonatedTroop(this.Device.Player.Avatar.UserId, Troop.GetGlobalID(), 1, this.Device.Player.Avatar.GetUnitUpgradeLevel(Troop));
                    
                    int _Capicity = Troop.GetHousingSpace();
                    _Stream.AddUsedCapicity(_Capicity);
                    _PreviousPlayerAvatar.SetAllianceCastleUsedCapacity(_PreviousPlayerAvatar.GetAllianceCastleUsedCapacity() + _Capicity);
                    _PreviousPlayerAvatar.AddAllianceTroop(this.Device.Player.Avatar.UserId, Troop.GetGlobalID(), 1, this.Device.Player.Avatar.GetUnitUpgradeLevel(Troop));

                    this.Device.Player.Avatar.m_vDonated++;
                    _Sender.Avatar.m_vReceived++;


                    foreach (AllianceMemberEntry op in a.GetAllianceMembers())
                    {
                        Level player = await ResourcesManager.GetPlayer(op.AvatarId);
                        if (player.Client != null)
                        {
                            new AllianceStreamEntryMessage(player.Client) { StreamEntry = _Stream }.Send();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
