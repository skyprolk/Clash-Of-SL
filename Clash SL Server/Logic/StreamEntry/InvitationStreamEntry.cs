using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using CSS.Helpers;
using CSS.Helpers.List;

namespace CSS.Logic.StreamEntry
{
    internal class InvitationStreamEntry : StreamEntry
    {
        public static string Message = "Hello, I would like to join your clan.";

        public override byte[] Encode()
        {
            List<byte> data = new List<byte>();
            data.AddRange(base.Encode());
            data.AddString(Message);
            data.AddString(m_vJudge);
            data.AddInt(m_vState);
            return data.ToArray();
        }

        public override int GetStreamEntryType() => 3;

        public override void Load(JObject jsonObject)
        {
            Message  = jsonObject["message"].ToObject<string>();
            m_vJudge = jsonObject["judge"].ToObject<string>();
            m_vState = jsonObject["state"].ToObject<int>();
        }

        public override JObject Save(JObject jsonObject)
        {
            jsonObject.Add("message", Message);
            jsonObject.Add("judge", m_vJudge);
            jsonObject.Add("state", m_vState);
            return jsonObject;
        }

        public void SetJudgeName(string name) => m_vJudge = name;

        public void SetMessage(string message) => Message = message;

        public void SetState(int status) => m_vState = status;
    }
}