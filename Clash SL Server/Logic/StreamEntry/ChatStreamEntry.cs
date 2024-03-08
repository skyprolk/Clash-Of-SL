using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using CSS.Helpers;
using CSS.Helpers.List;

namespace CSS.Logic.StreamEntry
{
    internal class ChatStreamEntry : StreamEntry
    {
        internal string Message;
    
        public override byte[] Encode()
        {
            List<byte> data = new List<byte>();
            data.AddRange(base.Encode());
            data.AddString(Message);
            return data.ToArray();
        }

        public override int GetStreamEntryType() => 2;

        public override void Load(JObject jsonObject)
        {
            Message = jsonObject["message"].ToObject<string>();
        }

        public override JObject Save(JObject jsonObject)
        {
            jsonObject.Add("message", Message);
            return jsonObject;
        }
    }
}
