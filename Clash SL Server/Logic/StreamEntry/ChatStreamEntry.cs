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
using Newtonsoft.Json.Linq;
using CSS.Helpers;

namespace CSS.Logic.StreamEntry
{
    internal class ChatStreamEntry : StreamEntry
    {
        #region Private Fields

        string m_vMessage;

        #endregion Private Fields

        #region Public Methods

        public override byte[] Encode()
        {
            var data = new List<byte>();
            data.AddRange(base.Encode());
            data.AddString(m_vMessage);
            return data.ToArray();
        }

        public string GetMessage() => m_vMessage;

        public override int GetStreamEntryType() => 2;

        public override void Load(JObject jsonObject)
        {
            base.Load(jsonObject);
            m_vMessage = jsonObject["message"].ToObject<string>();
        }

        public override JObject Save(JObject jsonObject)
        {
            jsonObject = base.Save(jsonObject);
            jsonObject.Add("message", m_vMessage);
            return jsonObject;
        }

        public void SetMessage(string message)
        {
            m_vMessage = message;
        }

        #endregion Public Methods
    }
}
