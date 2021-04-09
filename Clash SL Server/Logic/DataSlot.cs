/*
 * Program : Clash Of SL Server
 * Description : A C# Writted 'Clash of SL' Server Emulator !
 *
 * Authors:  Sky Tharusha <Founder at Sky Production>,
 *           And the Official DARK Developement Team
 *
 * Copyright (c) 2021  Sky Production
 * All Rights Reserved.
 */

using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using CSS.Core;
using CSS.Files.Logic;
using CSS.Helpers;

namespace CSS.Logic
{
    internal class DataSlot
    {
        public DataSlot(Data d, int value)
        {
            Data = d;
            Value = value;
        }

        public Data Data;
        public int Value;

        public void Decode(BinaryReader br)
        {
            Data = br.ReadDataReference();
            Value = br.ReadInt32WithEndian();
        }

        public byte[] Encode()
        {
            var data = new List<byte>();
            data.AddInt32(Data.GetGlobalID());
            data.AddInt32(Value);
            return data.ToArray();
        }

        public void Load(JObject jsonObject)
        {
            Data = ObjectManager.DataTables.GetDataById(jsonObject["global_id"].ToObject<int>());
            Value = jsonObject["value"].ToObject<int>();
        }

        public JObject Save(JObject jsonObject)
        {
            jsonObject.Add("global_id", Data.GetGlobalID());
            jsonObject.Add("value", Value);
            return jsonObject;
        }

    }
    internal class TroopDataSlot
    {
        public TroopDataSlot(Data d, int value, int value1)
        {
            Data = d;
            Value = value;
            Value1 = value1;
        }

        public Data Data;
        public int Value;
        public int Value1;

        public void Decode(BinaryReader br)
        {
            Data = br.ReadDataReference();
            Value = br.ReadInt32WithEndian();
            Value1 = br.ReadInt32WithEndian();
        }

        public byte[] Encode()
        {
            var data = new List<byte>();
            data.AddInt32(Data.GetGlobalID());
            data.AddInt32(Value);
            data.AddInt32(Value1);
            return data.ToArray();
        }

        public void Load(JObject jsonObject)
        {
            Data = ObjectManager.DataTables.GetDataById(jsonObject["global_id"].ToObject<int>());
            Value = jsonObject["count"].ToObject<int>();
            Value1 = jsonObject["level"].ToObject<int>();
        }

        public JObject Save(JObject jsonObject)
        {
            jsonObject.Add("global_id", Data.GetGlobalID());
            jsonObject.Add("count", Value);
            jsonObject.Add("level", Value1);
            return jsonObject;
        }
    }
}