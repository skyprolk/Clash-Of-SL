namespace UCS.Logic.JSONProperty
{
    using Newtonsoft.Json;
    internal class Slot
    {
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Include)]
        public int Data;

        [JsonProperty("cnt", DefaultValueHandling = DefaultValueHandling.Include)]
        public int Count;

        internal Slot() { }

        internal Slot(int _ID, int _Count)
        {
            this.Data = _ID;
            this.Count = _Count;
        }
    }
}
