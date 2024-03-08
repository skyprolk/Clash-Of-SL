using CSS.Core;
using CSS.Files.Logic;
using CSS.Helpers.Binary;
using CSS.Helpers.Binary;
using CSS.Logic;

namespace CSS.Packets.Commands.Client
{
    // Packet 529
    internal class ToggleHeroSleepCommand : Command
    {
        public ToggleHeroSleepCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.BuildingID = this.Reader.ReadInt32();
            this.FlagSleep = this.Reader.ReadByte();
            this.Tick = this.Reader.ReadUInt32();
        }

        internal override void Process()
        {
            /*Building Building = (Building)this.Device.Player.GameObjectManager.GetGameObjectByID(this.BuildingID);
            if (Building != null)
            {

                HeroData _Hero = CSVManager.DataTables.GetHeroByName(Building.GetBuildingData().HeroType);
                if (this.FlagSleep == 1)
                {
                    this.Device.Player.Avatar.SetHeroState(_Hero, this.FlagSleep + 1);
                }
                else
                {
                    this.Device.Player.Avatar.SetHeroState(_Hero, 0);
                }
            }*/
        }

        public int BuildingID;
        public byte FlagSleep;
        public uint Tick;
    }
}