using System;
using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Logic.Enums;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Commands.Client
{
    // Packet 700
    internal class SearchOpponentCommand : Command
    {
        public SearchOpponentCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
        }

        internal override async void Process()
        {
            try
            {
                if (this.Device.PlayerState == State.IN_BATTLE)
                {
                    ResourcesManager.DisconnectClient(this.Device);
                }
                else
                {
                    /*if (level.Avatar.GetUnits().Count < 10)
                    {
                        for (int i = 0; i < 31; i++)
                        {
                            Data unitData = CSVManager.DataTables.GetDataById(4000000 + i);
                            CharacterData combatData = (CharacterData)unitData;
                            int maxLevel = combatData.GetUpgradeLevelCount();
                            DataSlot unitSlot = new DataSlot(unitData, 1000);

                            level.Avatar.GetUnits().Add(unitSlot);
                            level.Avatar.SetUnitUpgradeLevel(combatData, maxLevel - 1);
                        }

                        for (int i = 0; i < 18; i++)
                        {
                            Data spellData = CSVManager.DataTables.GetDataById(26000000 + i);
                            SpellData combatData = (SpellData)spellData;
                            int maxLevel = combatData.GetUpgradeLevelCount();
                            DataSlot spellSlot = new DataSlot(spellData, 1000);

                            level.Avatar.GetSpells().Add(spellSlot);
                            level.Avatar.SetUnitUpgradeLevel(combatData, maxLevel - 1);
                        }
                    }*/

                    // New Method
                    this.Device.PlayerState = State.SEARCH_BATTLE;
                    Level Defender = ObjectManager.GetRandomOnlinePlayer();
                    if (Defender != null)
                    {
                        Defender.Tick();
                        new EnemyHomeDataMessage(this.Device, Defender, this.Device.Player).Send();
                    }
                    else
                    {
                        new OutOfSyncMessage(this.Device).Send();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
