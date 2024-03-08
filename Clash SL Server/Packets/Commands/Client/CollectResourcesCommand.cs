using CSS.Helpers.Binary;
using CSS.Logic;

namespace CSS.Packets.Commands.Client
{
    // Packet 506
    internal class CollectResourcesCommand : Command
    {
        public CollectResourcesCommand(Reader reader, Device client, int id) : base(reader, client, id)

        {
        }

        internal override void Decode()
        {
            this.BuildingId = this.Reader.ReadInt32();
            this.Unknown1 = this.Reader.ReadUInt32();
        }

        internal override void Process()
        {
            var go = this.Device.Player.GameObjectManager.GetGameObjectByID(BuildingId);

            if (go != null)
            {
                if (go.ClassId == 0 || go.ClassId == 4)
                {
                    var constructionItem = (ConstructionItem) go;
                    constructionItem.GetResourceProductionComponent().CollectResources();
                }
            }
        }

        public int BuildingId;
        public uint Unknown1;
    }
}