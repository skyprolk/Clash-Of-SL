using Newtonsoft.Json.Linq;
using CSS.Core;
using CSS.Files.Logic;

namespace CSS.Logic
{
    internal class CombatComponent : Component
    {
        public CombatComponent(ConstructionItem ci, Level level) : base(ci)
        {
            var bd = (BuildingData) ci.GetData();
            if (bd.AmmoCount != 0)
            {
                m_vAmmo = bd.AmmoCount;
            }
        }

        public override int Type => 1;

        const int m_vType = 0x01AB3F00;

        int m_vAmmo;

        public void FillAmmo()
        {
            var ca = GetParent().Avatar.Avatar;
            var bd = (BuildingData) GetParent().GetData();
            var rd = CSVManager.DataTables.GetResourceByName(bd.AmmoResource);

            if (ca.HasEnoughResources(rd, bd.AmmoCost))
            {
                ca.CommodityCountChangeHelper(0, rd, bd.AmmoCost);
                m_vAmmo = bd.AmmoCount;
            }
        }

        public override void Load(JObject jsonObject)
        {
            if (jsonObject["ammo"] != null)
            {
                m_vAmmo = jsonObject["ammo"].ToObject<int>();
            }
        }

        public override JObject Save(JObject jsonObject)
        {
            jsonObject.Add("ammo", m_vAmmo);
            return jsonObject;
        }
    }
}
