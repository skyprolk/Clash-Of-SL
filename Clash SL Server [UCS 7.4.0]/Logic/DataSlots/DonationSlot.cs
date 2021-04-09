using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSS.Helpers;
using CSS.Helpers.Binary;
using CSS.Helpers.List;

namespace CSS.Logic.DataSlots
{
    internal class DonationSlot
    {
        public long DonatorID;
        public int ID;
        public int Count;
        public int UnitLevel;

        public DonationSlot(long did, int id, int ucount, int ulevel)
        {
            DonatorID = did;
            ID        = id;
            Count     = ucount;
            UnitLevel = ulevel;
        }

        public void Decode(Reader br)            
        {
            DonatorID = br.ReadInt64();
            ID        = br.ReadInt32();
            Count     = br.ReadInt32();
            UnitLevel = br.ReadInt32();
        }

        public byte[] Encode()
        {
            List<byte> data = new List<byte>();
            data.AddLong(DonatorID);
            data.AddInt(ID);
            data.AddInt(Count);
            data.AddInt(UnitLevel);
            return data.ToArray();
        }

        public void Load(JObject jsonObject)
        {
            DonatorID = jsonObject["donatorid"].ToObject<long>();
            ID        = jsonObject["unitid"].ToObject<int>();
            Count     = jsonObject["unitcount"].ToObject<int>();
            UnitLevel = jsonObject["unitlevel"].ToObject<int>();
        }

        public JObject Save(JObject jsonObject)
        {
            jsonObject.Add("donatorid", DonatorID);
            jsonObject.Add("unitid", ID);
            jsonObject.Add("unitcount", Count);
            jsonObject.Add("unitlevel", UnitLevel);
            return jsonObject;
        }
    }
}
