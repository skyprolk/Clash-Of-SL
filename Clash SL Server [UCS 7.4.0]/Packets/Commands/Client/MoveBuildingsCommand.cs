using System;
using System.Windows;
using CSS.Helpers.Binary;

namespace CSS.Packets.Commands.Client
{
    internal class MoveBuildingsCommand : Command
    {
        public MoveBuildingsCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.MovedBuilding = this.Reader.ReadInt32();
            this.ReplacedBuilding = this.Reader.ReadInt32();
            this.Tick = this.Reader.ReadInt32();
        }

        public int Tick;

        public int ReplacedBuilding;

        public int MovedBuilding;

        internal override void Process()
        {
            if(MovedBuilding > 0)
            {
                Vector movedBuildingPosition = this.Device.Player.GameObjectManager.GetGameObjectByID(MovedBuilding).GetPosition();
                Vector replacedBuildingPosition = this.Device.Player.GameObjectManager.GetGameObjectByID(ReplacedBuilding).GetPosition();

                this.Device.Player.GameObjectManager.GetGameObjectByID(MovedBuilding).SetPositionXY(Convert.ToInt32(replacedBuildingPosition.X), Convert.ToInt32(replacedBuildingPosition.Y), this.Device.Player.Avatar.m_vActiveLayout);
                this.Device.Player.GameObjectManager.GetGameObjectByID(ReplacedBuilding).SetPositionXY(Convert.ToInt32(movedBuildingPosition.X), Convert.ToInt32(movedBuildingPosition.Y), this.Device.Player.Avatar.m_vActiveLayout);
            }
        }
    }
}
