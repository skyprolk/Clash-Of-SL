using Newtonsoft.Json;

namespace UCS.Logic.JSONProperty
{
    using System.Collections.Generic;
    using UCS.Helpers.List;
    using UCS.Logic.JSONProperty.Item;
    internal class Npcs : List<Npc>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Achievements"/> class.
        /// </summary>
        [JsonConstructor]
        internal Npcs()
        {
            // Npcs.
        }

        internal byte[] ToBytes
        {
            get
            {
                List<byte> Packet = new List<byte>();

                Packet.AddInt(this.Count);
                foreach (Npc Npc in this)
                {
                    Packet.AddInt(Npc.NPC_Id);
                }

                Packet.AddInt(this.Count);
                foreach (Npc Npc in this)
                {
                    Packet.AddInt(Npc.Elixir_Looted);
                }

                Packet.AddInt(this.Count);
                foreach (Npc Npc in this)
                {
                    Packet.AddInt(Npc.Gold_Looted);
                }

                Packet.AddInt(this.Count);
                foreach (Npc Npc in this)
                {
                    Packet.AddInt(Npc.Dark_Elixir_Looted);
                }

                return Packet.ToArray();
            }
        }
    }
}
