using CSS.Helpers.Binary;

namespace CSS.Packets.Commands.Client
{
    // Packet 546
    internal class EditVillageLayoutCommand : Command
    {
        internal int X;
        internal int Y;
        internal int BuildingID;
        internal int Layout;

        public EditVillageLayoutCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.X = this.Reader.ReadInt32();
            this.Y = this.Reader.ReadInt32();
            this.BuildingID = this.Reader.ReadInt32();
            this.Layout = this.Reader.ReadInt32();
            this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            /*if (Layout != level.Avatar.GetActiveLayout())
            {
                GameObject go = level.GameObjectManager.GetGameObjectByID(BuildingID);
                go.SetPositionXY(X, Y, Layout);
            } */
        }

    }
}
