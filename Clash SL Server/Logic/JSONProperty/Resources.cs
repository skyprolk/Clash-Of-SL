using Newtonsoft.Json;

namespace UCS.Logic.JSONProperty
{
    using System.Collections.Generic;
    using System.Linq;
    using UCS.Helpers;
    using UCS.Helpers.List;

    internal class Resources : List<Slot>
    {
        internal ClientAvatar Player;

        /// <summary>
        /// Initializes a new instance of the <see cref="Resources"/> class.
        /// </summary>
        [JsonConstructor]
        internal Resources()
        {
            // Resources.
        }

        internal Resources(ClientAvatar _Player, bool Initialize = false)
        {
            this.Player = _Player;

            if (Initialize)
                this.Initialize();
        }

        internal int Gems => this.Get(Enums.Resource.Diamonds);

        internal int Get(int Gl_ID)
        {
            int i = this.FindIndex(R => R.Data == Gl_ID);

            if (i > -1)
                return this[i].Count;

            return 0;
        }

        internal int Get(Enums.Resource Gl_ID)
        {
            return this.Get(3000000 + (int) Gl_ID);
        }

        internal void Set(int Gl_ID, int Count)
        {
            int i = this.FindIndex(R => R.Data == Gl_ID);

            if (i > -1)
                this[i].Count = Count;
            else this.Add(new Slot(Gl_ID, Count));
        }

        internal void Set(Enums.Resource Resource, int Count)
        {
            this.Set(3000000 + (int) Resource, Count);
        }

        internal void Plus(int Gl_ID, int Count)
        {
            int i = this.FindIndex(R => R.Data == Gl_ID);

            if (i > -1)
                this[i].Count += Count;
            else this.Add(new Slot(Gl_ID, Count));
        }

        internal void Plus(Enums.Resource Resource, int Count)
        {
            this.Plus(3000000 + (int) Resource, Count);
        }

        internal bool Minus(int Gl_ID, int Count)
        {
            int i = this.FindIndex(R => R.Data == Gl_ID);

            if (i > -1)
                if (this[i].Count >= Count)
                {
                    this[i].Count -= Count;
                    return true;
                }

            return false;
        }

        internal void Minus(Enums.Resource _Resource, int _Value)
        {
            int Index = this.FindIndex(T => T.Data == 3000000 + (int) _Resource);

            if (Index > -1)
            {
                this[Index].Count -= _Value;
            }
        }

        internal byte[] ToBytes
        {
            get
            {
                List<byte> Packet = new List<byte>();

                Packet.AddInt(this.Count - 1);
                foreach (Slot Resource in this.Skip(1))
                {
                    Packet.AddInt(Resource.Data);
                    Packet.AddInt(Resource.Count);
                }

                return Packet.ToArray();
            }
        }

        internal void Initialize()
        {

            this.Set(Enums.Resource.Diamonds, Utils.ParseConfigInt("startingGems"));

            this.Set(Enums.Resource.Gold, Utils.ParseConfigInt("startingGold"));
            this.Set(Enums.Resource.Elixir, Utils.ParseConfigInt("startingElixir"));

            this.Set(Enums.Resource.DarkElixir, Utils.ParseConfigInt("startingDarkElixir"));
        }
    }
}