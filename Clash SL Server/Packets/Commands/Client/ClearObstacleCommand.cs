using CSS.Helpers.Binary;

namespace CSS.Packets.Commands.Client
{
    // Packet 507
    internal class ClearObstacleCommand : Command
    {
        public ClearObstacleCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.ObstacleId = this.Reader.ReadInt32();
            this.Tick = this.Reader.ReadUInt32();
        }

        internal override void Process()
        {
            /*ClientAvatar playerAvatar = level.Avatar;
            Obstacle gameObjectByID = (Obstacle)level.GameObjectManager.GetGameObjectByID(ObstacleId);
            ObstacleData obstacleData = gameObjectByID.GetObstacleData();
            if (playerAvatar.HasEnoughResources(obstacleData.GetClearingResource(), obstacleData.ClearCost) && level.HasFreeWorkers())
            {
                ResourceData clearingResource = obstacleData.GetClearingResource();
                playerAvatar.SetResourceCount(clearingResource, playerAvatar.GetResourceCount(clearingResource) - obstacleData.ClearCost);
                gameObjectByID.StartClearing();
            }*/
        }

        public int ObstacleId;
        public uint Tick;
    }
}
