namespace UCS.Logic.JSONProperty.Item
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    internal class Event
    {
        [JsonProperty("id")]
        internal int ID = 0;

        [JsonProperty("version")]
        internal int Version = 0;

        [JsonProperty("visibleTime")]
        internal string VisibleTime = DateTime.UtcNow.ToString();

        [JsonProperty("startTime")]
        internal string StarTime = DateTime.UtcNow.ToString();

        [JsonProperty("endTime")]
        internal string EndTime = DateTime.UtcNow.ToString();

        [JsonProperty("boomboxEntry")]
        internal string BoomBoxEntry = string.Empty;

        [JsonProperty("eventEntryName")]
        internal string EnventEntryName = string.Empty;

        [JsonProperty("inboxEntryId")]
        internal int InboxEntryID = 0;

        [JsonProperty("notification")]
        internal string Notification = string.Empty;

        [JsonProperty("functions")]
        internal List<Functions> Functions = new List<Functions>();
    }

    internal class Functions
    {
        [JsonProperty("name")]
        internal string Name = string.Empty;

        [JsonProperty("parameters")]
        internal int[] Parameters;
    }
}