namespace UCS.Logic.JSONProperty
{
    using Newtonsoft.Json;

    internal class Replay_Stats
    {
        [JsonConstructor]
        public Replay_Stats()
        {
            
        }

        [JsonProperty("townhallDestroyed", DefaultValueHandling = DefaultValueHandling.Include)]
        internal bool TownHall_Destroyed = false;

        [JsonProperty("battleEnded", DefaultValueHandling = DefaultValueHandling.Include)]
        internal bool Battle_Ended = false;

        [JsonProperty("allianceUsed", DefaultValueHandling = DefaultValueHandling.Include)]
        internal bool Alliance_Used = false;

        [JsonProperty("destructionPercentage", DefaultValueHandling = DefaultValueHandling.Include)]
        internal int Destruction_Percentate = 0;

        [JsonProperty("battleTime", DefaultValueHandling = DefaultValueHandling.Include)]
        internal int Battle_Time = 0;

        [JsonProperty("originalAttackerScore", DefaultValueHandling = DefaultValueHandling.Include)]
        internal int Original_Attacker_Score = 0;

        [JsonProperty("attackerScore", DefaultValueHandling = DefaultValueHandling.Include)]
        internal int Attacker_Score = 0;

        [JsonProperty("originalDefenderScore", DefaultValueHandling = DefaultValueHandling.Include)]
        internal int Original_Defender_Score = 0;

        [JsonProperty("defenderScore", DefaultValueHandling = DefaultValueHandling.Include)]
        internal int Defender_Score = 0;

        [JsonProperty("allianceName", DefaultValueHandling = DefaultValueHandling.Include)]
        internal string Alliance_Name = string.Empty;

        [JsonProperty("attackerStars", DefaultValueHandling = DefaultValueHandling.Include)]
        internal int Attacker_Stars = 0;

        [JsonProperty("homeID", DefaultValueHandling = DefaultValueHandling.Include)]
        internal int[] Home_ID = { 0, 0 };

        [JsonProperty("allianceBadge", DefaultValueHandling = DefaultValueHandling.Include)]
        internal int Alliance_Badge = -1;

        [JsonProperty("allianceBadge2", DefaultValueHandling = DefaultValueHandling.Include)]
        internal int Alliance_Badge_2 = -1;

        [JsonProperty("deployedHousingSpace", DefaultValueHandling = DefaultValueHandling.Include)]
        internal int Deployed_HousingSpace = 0;

        [JsonProperty("armyDeploymentPercentage", DefaultValueHandling = DefaultValueHandling.Include)]
        internal int Army_Deployment_Percentage = 0;
    }
}