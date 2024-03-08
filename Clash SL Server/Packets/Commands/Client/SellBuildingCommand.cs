using System.IO;
using CSS.Helpers;
using CSS.Helpers.Binary;
using CSS.Logic;

namespace CSS.Packets.Commands.Client
{
    // Packet 503
    internal class SellBuildingCommand : Command
    {
        internal int m_vBuildingId;

        public SellBuildingCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.m_vBuildingId = this.Reader.ReadInt32();
            this.Reader.ReadUInt32();
        }

        internal override void Process()
        {
            var ca = this.Device.Player.Avatar;
            var go = this.Device.Player.GameObjectManager.GetGameObjectByID(m_vBuildingId);

            if (go != null)
            {
                if (go.ClassId == 4)
                {
                    var t = (Trap) go;
                    var upgradeLevel = t.GetUpgradeLevel();
                    var rd = t.GetTrapData().GetBuildResource(upgradeLevel);
                    var sellPrice = t.GetTrapData().GetSellPrice(upgradeLevel);
                    ca.CommodityCountChangeHelper(0, rd, sellPrice);
                    this.Device.Player.GameObjectManager.RemoveGameObject(t);
                }
                else if (go.ClassId == 6)
                {
                    var d = (Deco) go;
                    var rd = d.GetDecoData().GetBuildResource();
                    var sellPrice = d.GetDecoData().GetSellPrice();
                    if (rd.PremiumCurrency)
                    {
                        ca.m_vCurrentGems = ca.m_vCurrentGems + sellPrice;
                    }
                    else
                    {
                        ca.CommodityCountChangeHelper(0, rd, sellPrice);
                    }
                    this.Device.Player.GameObjectManager.RemoveGameObject(d);
                }
            }
        }
    }
}