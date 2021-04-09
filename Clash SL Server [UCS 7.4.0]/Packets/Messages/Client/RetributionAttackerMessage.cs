using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSS.Core;
using CSS.Core.Network;
using CSS.Files.Logic;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Messages.Client
{
    internal class RetributionAttackerMessage : Message
    {
        public RetributionAttackerMessage(Device device, Reader reader) : base(device, reader)
        {
        }


        internal override async void Process()
        {
            ClientAvatar p = this.Device.Player.Avatar;
            if (this.Device.PlayerState == Logic.Enums.State.IN_BATTLE)
            {
                ResourcesManager.DisconnectClient(Device);
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
                this.Device.PlayerState = Logic.Enums.State.SEARCH_BATTLE;
                new RetributionDataMessage(Device, this.Device.Player, 17000049).Send();
            }
        }
    }
}
