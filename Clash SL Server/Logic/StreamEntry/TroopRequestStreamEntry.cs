using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using CSS.Helpers;
using CSS.Logic.DataSlots;
using CSS.Core;
using System;
using CSS.Helpers.List;

namespace CSS.Logic.StreamEntry
{
    internal class TroopRequestStreamEntry : StreamEntry
    {
        private static int ID = (int)ObjectManager.m_vDonationSeed;
        public int m_vMaxSpell = 0;

        public TroopRequestStreamEntry()
        {
            m_vUnitDonation = new List<DonationSlot>();
            m_vDonatorList = new List<BookmarkSlot>();
        }

        public override byte[] Encode()
        {
            List<byte> data = new List<byte>();
            data.AddRange(base.Encode());
            data.AddInt(ID); // ID
            data.AddInt(m_vMaxTroop); // Max Troops
            data.AddInt(m_vMaxSpell); // Max Spells
            data.AddInt(m_vDonatedTroop); // Donated Troops
            data.AddInt(m_vDonatedSpell); // Donated Spells
            data.AddInt(m_vDonatorList.Count);
            for (int d = 0; d < m_vDonatorList.Count; d++)
            {
                foreach (DonationSlot i in m_vUnitDonation) // Components
                {
                    DonationSlot tp = m_vUnitDonation.Find(t => t.DonatorID == i.DonatorID && t.UnitLevel == i.UnitLevel);
                    data.AddLong(tp.DonatorID);
                    data.AddInt(tp.Count);
                    for (int a = 0; a < i.Count - 1; a++)
                    {
                        data.AddInt(tp.ID);
                        data.AddInt(tp.UnitLevel - 1);
                    }
                    data.AddInt(tp.ID);
                    data.AddInt(tp.UnitLevel);
                }
            }
            if (string.IsNullOrEmpty(Message))
            {
                data.Add(0);
            }
            else
            {
                data.Add(1);
                data.AddString(Message);
            }
            data.AddInt(m_vUnitDonation.Count); // Components Count
            foreach (DonationSlot i in m_vUnitDonation) // Components
            {
                data.AddInt(i.ID);
                data.AddInt(i.Count);
                data.AddInt(i.UnitLevel);
            }
            return data.ToArray();
        }

        public override int GetStreamEntryType() => 1;

        public override void Load(JObject jsonObject)
        {
            base.Load(jsonObject);
            ID = jsonObject["rid"].ToObject<int>();
            m_vMaxTroop = jsonObject["max_troops"].ToObject<int>();
            m_vMaxSpell = jsonObject["max_spells"].ToObject<int>();
            m_vDonatedTroop = jsonObject["donated_troops"].ToObject<int>();
            m_vDonatedSpell = jsonObject["donated_spell"].ToObject<int>();
            Message = jsonObject["message"].ToObject<string>();
            JArray jsonDonaterID = (JArray)jsonObject["donater_list"];
            foreach (JToken jToken in jsonDonaterID)
            {
                JObject data = (JObject)jToken;
                BookmarkSlot di = new BookmarkSlot(0);
                di.Load(data);
                m_vDonatorList.Add(di);
            }
            JArray jsonDonatedUnit = (JArray)jsonObject["donated_unit"];
            foreach (JToken jToken in jsonDonatedUnit)
            {
                JObject data = (JObject)jToken;
                DonationSlot ds = new DonationSlot(0, 0, 0, 0);
                ds.Load(data);
                m_vUnitDonation.Add(ds);
            }
        }

        public override JObject Save(JObject jsonObject)
        {
            jsonObject = base.Save(jsonObject);
            jsonObject.Add("rid", ID);
            jsonObject.Add("max_troops", m_vMaxTroop);
            jsonObject.Add("max_spells", m_vMaxSpell);
            jsonObject.Add("donated_troops", m_vDonatedTroop);
            jsonObject.Add("donated_spell", m_vDonatedSpell);
            jsonObject.Add("message", Message);
            JArray jsonDonaterID = new JArray();
            foreach (BookmarkSlot id in m_vDonatorList)
            {
                jsonDonaterID.Add(id.Save(new JObject()));
            }
            jsonObject.Add("donater_list", jsonDonaterID);
            JArray jsonDonatedUnit = new JArray();
            foreach (DonationSlot unit in m_vUnitDonation)
            {
                jsonDonatedUnit.Add(unit.Save(new JObject()));
            }
            jsonObject.Add("donated_unit", jsonDonatedUnit);
            return jsonObject;
        }
    }
}
