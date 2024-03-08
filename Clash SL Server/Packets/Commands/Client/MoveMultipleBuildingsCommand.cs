using System.Collections.Generic;
using CSS.Helpers.Binary;
using CSS.Logic;

namespace CSS.Packets.Commands.Client
{
    // Packet 533
    internal class BuildingToMove
    {
        public int GameObjectId;
        public int X;
        public int Y;
    }

    internal class MoveMultipleBuildingsCommand : Command
    {
        internal List<BuildingToMove> m_vBuildingsToMove;

        public MoveMultipleBuildingsCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.m_vBuildingsToMove = new List<BuildingToMove>();
            var buildingCount = this.Reader.ReadInt32();
            for (var i = 0; i < buildingCount; i++)
            {
                var buildingToMove = new BuildingToMove
                {
                    X = this.Reader.ReadInt32(),
                    Y = this.Reader.ReadInt32(),
                    GameObjectId = this.Reader.ReadInt32()
                };
                this.m_vBuildingsToMove.Add(buildingToMove);
            }
            this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            foreach (var buildingToMove in this.m_vBuildingsToMove)
            {
                GameObject go = this.Device.Player.GameObjectManager.GetGameObjectByID(buildingToMove.GameObjectId);
                go.SetPositionXY(buildingToMove.X, buildingToMove.Y, this.Device.Player.Avatar.m_vActiveLayout);
            }
        }
    }
}