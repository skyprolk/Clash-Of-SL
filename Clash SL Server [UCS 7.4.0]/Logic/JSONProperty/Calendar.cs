namespace UCS.Logic.JSONProperty
{
    using System.Collections.Generic;

    using Newtonsoft.Json;
    using UCS.Logic.JSONProperty.Item;

    internal class Calendar
    {
        [JsonProperty("events")]
        internal List<Event> Events = new List<Event>();
    }
}