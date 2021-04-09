using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UCS.Logic.JSONProperty;
using UCS.Logic.JSONProperty.Item;
using UCS.Core;

namespace UCS.Logic
{
    internal class Battle
    {
        internal double Last_Tick;

        internal double Preparation_Time = 30;
        internal double Attack_Time = 180;

        /// <summary>
        ///     Gets or sets the battle tick.
        /// </summary>
        /// <value>The battle tick.</value>
        internal double BattleTick
        {
            get
            {
                if (this.Preparation_Time > 0) return this.Preparation_Time;
                return this.Attack_Time;
            }
            set
            {
                if (this.Preparation_Time >= 1 && this.Commands.Count < 1)
                {
                    this.Preparation_Time -= (value - this.Last_Tick) / 63;
                    Logger.Write("Preparation Time : " + this.Preparation_Time);
                }
                else
                {
                    this.Attack_Time -= (value - this.Last_Tick) / 63;
                    Logger.Write("Attack Time      : " + this.Attack_Time);
                }
                this.Last_Tick = value;
                this.End_Tick = (int) value;

            }
        }

        [JsonProperty("level")] internal JObject Base;

        [JsonProperty("attacker")] internal ClientAvatar Attacker = new ClientAvatar();

        [JsonProperty("defender")] internal ClientAvatar Defender = new ClientAvatar();

        [JsonIgnore] internal Replay_Info Replay_Info = new Replay_Info();

        [JsonProperty("end_tick")] internal int End_Tick;

        [JsonProperty("timestamp")] internal int TimeStamp =
            (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        [JsonProperty("cmd")] internal Commands Commands = new Commands();

        [JsonProperty("prep_skip")] internal int Preparation_Skip = 0;

        [JsonProperty("calendar")] internal Calendar Calendar = new Calendar();
        [JsonProperty("battle_id")] internal long Battle_ID;

        [JsonConstructor]
        internal Battle()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Battle" /> class.
        /// </summary>
        /// <param name="Battle">The battle.</param>
        /// <param name="_Attacker">The attacker.</param>
        /// <param name="_Enemy">The enemy.</param>
        internal Battle(long Battle, Level _Attacker, Level _Enemy, bool clone = true)
        {
            this.Battle_ID = Battle;

            this.Attacker = _Attacker.Avatar;
            this.Defender = _Enemy.Avatar;
            this.Base = _Enemy.GameObjectManager.Save();
            this.Attacker.Units = new Units();
        }

        /// <summary>
        ///     Adds the command.
        /// </summary>
        /// <param name="Command">The command.</param>
        internal void Add_Command(Battle_Command Command)
        {
            this.Commands.Add(this, Command);
        }

        /// <summary>
        ///     Sets the replay informations.
        /// </summary>
        internal void Set_Replay_Info()
        {
            foreach (Slot _Slot in this.Defender.Resources)
            {
                this.Replay_Info.Loot.Add(new[] {_Slot.Data, _Slot.Count}); // For Debug
                this.Replay_Info.Available_Loot.Add(new[] {_Slot.Data, _Slot.Count});
            }

            this.Replay_Info.Stats.Home_ID[0] = (int) this.Defender.HighID;
            this.Replay_Info.Stats.Home_ID[1] = (int) this.Defender.LowID;
            this.Replay_Info.Stats.Original_Attacker_Score = this.Attacker.Trophies;
            this.Replay_Info.Stats.Original_Defender_Score = this.Defender.Trophies;
            this.Replay_Info.Stats.Battle_Time = 180 - (int) this.Attack_Time + 1;
        }
    }
}