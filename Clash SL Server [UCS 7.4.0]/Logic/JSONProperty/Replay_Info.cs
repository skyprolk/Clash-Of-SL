namespace UCS.Logic.JSONProperty
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    internal class Replay_Info
    {
        [JsonConstructor]
        public Replay_Info()
        {

        }
        [JsonProperty("loot")]
        internal List<int[]> Loot = new List<int[]>();

        [JsonProperty("availableLoot")]
        internal List<int[]> Available_Loot = new List<int[]>();

        [JsonProperty("units")]
        internal List<int[]> Units = new List<int[]>();

        [JsonProperty("spells")]
        internal List<int[]> Spells = new List<int[]>();

        [JsonProperty("levels")]
        internal List<int[]> Levels = new List<int[]>();

        [JsonProperty("stats")]
        internal Replay_Stats Stats = new Replay_Stats();

        internal void Add_Unit(int Data, int Count)
        {
            int Index = this.Units.FindIndex(U => U[0] == Data);

            if (Index > -1) this.Units[Index][1] += Count;
            else this.Units.Add(new[] { Data, Count });
        }

        internal void Add_Spell(int Data, int Count)
        {
            int Index = this.Spells.FindIndex(U => U[0] == Data);

            if (Index > -1) this.Spells[Index][1] += Count;
            else this.Spells.Add(new[] { Data, Count });
        }

        internal void Add_Level(int Data, int Count)
        {
            int Index = this.Levels.FindIndex(U => U[0] == Data);

            if (Index > -1) this.Levels[Index][1] += Count;
            else this.Levels.Add(new[] { Data, Count });
        }

        internal void Add_Available_Loot(int Data, int Count)
        {
            int Index = this.Available_Loot.FindIndex(U => U[0] == Data);

            if (Index > -1) this.Available_Loot[Index][1] += Count;
            else this.Available_Loot.Add(new[] { Data, Count });
        }
        internal void Add_Loot(int Data, int Count)
        {
            int Index = this.Loot.FindIndex(U => U[0] == Data);

            if (Index > -1) this.Loot[Index][1] += Count;
            else this.Loot.Add(new[] { Data, Count });
        }
    }
}