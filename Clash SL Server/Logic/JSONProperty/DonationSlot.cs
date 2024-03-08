namespace UCS.Logic.JSONProperty
{
    using Newtonsoft.Json;

    internal class DonationSlot
    {
        [JsonProperty("did", DefaultValueHandling = DefaultValueHandling.Include)]
        public long DonatorID;

        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Include)]
        public int Data;

        [JsonProperty("cnt", DefaultValueHandling = DefaultValueHandling.Include)]
        public int Count;
        [JsonProperty("lvl", DefaultValueHandling = DefaultValueHandling.Include)]
        public int Level;

        internal DonationSlot() { }

        internal DonationSlot(long _UserID, int _ID, int _Count, int _Level)
        {
            this.DonatorID = _UserID;
            this.Data = _ID;
            this.Count = _Count;
            this.Level = _Level;
        }
    }
}
