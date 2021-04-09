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
    internal class InvitationStreamEntry : StreamEntry
    {
        #region Public Fields

        public static string Judge;
        public static string Message = "Hello, i want to join your clan.";
        public static int State = 3;

        #endregion Public Fields

        #region Public Methods

        public override byte[] Encode()
        {
            var data = new List<byte>();
            data.AddRange(base.Encode());
            data.AddString(Message);
            data.AddString(Judge);
            data.AddInt32(State);
            return data.ToArray();
        }

        public override int GetStreamEntryType()
        {
            return 3;
        }

        public override void Load(JObject jsonObject)
        {
            base.Load(jsonObject);
            Message = jsonObject["message"].ToObject<string>();
            Judge = jsonObject["judge"].ToObject<string>();
            State = jsonObject["state"].ToObject<int>();
        }

        public override JObject Save(JObject jsonObject)
        {
            jsonObject = base.Save(jsonObject);
            jsonObject.Add("message", Message);
            jsonObject.Add("judge", Judge);
            jsonObject.Add("state", State);
            return jsonObject;
        }

        public void SetJudgeName(string name)
        {
            Judge = name;
        }

        public void SetMessage(string message)
        {
            Message = message;
        }

        public void SetState(int status)
        {
            State = status;
        }

        #endregion Public Methods
    }
}