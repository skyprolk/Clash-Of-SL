namespace UCS.Logic.JSONProperty.Item
{
    using Newtonsoft.Json;

    internal class Battle_Command
    {
        [JsonProperty("ct", DefaultValueHandling = DefaultValueHandling.Include)] public int Command_Type = 0;

        [JsonProperty("c", DefaultValueHandling = DefaultValueHandling.Include)] internal Command_Base Command_Base =
            new Command_Base();
    }

    internal class Command_Base
    {
        [JsonProperty("base", DefaultValueHandling = DefaultValueHandling.Include)] internal Base Base = new Base();

        [JsonProperty("d")] internal int Data = 0;

        [JsonProperty("x")] internal int X = 0;

        [JsonProperty("y")] internal int Y = 0;
    }

    internal class Base
    {
        [JsonProperty("t", DefaultValueHandling = DefaultValueHandling.Include)] internal int Tick = 0;
    }
}