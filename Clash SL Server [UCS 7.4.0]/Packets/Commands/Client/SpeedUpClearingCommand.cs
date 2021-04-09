using CSS.Helpers.Binary;

namespace CSS.Packets.Commands.Client
{
    // Packet 514
    internal class SpeedUpClearingCommand : Command
    {
        internal int m_vObstacleId;

        internal int m_vTick;

        public SpeedUpClearingCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.m_vObstacleId = this.Reader.ReadInt32();
            this.m_vTick = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            /*GameObject gameObjectByID = level.GameObjectManager.GetGameObjectByID(m_vObstacleId);
            if (gameObjectByID != null && gameObjectByID.ClassId == 3)
            {
                ((Obstacle)gameObjectByID).SpeedUpClearing();
            }
            */
        }
    }
}
