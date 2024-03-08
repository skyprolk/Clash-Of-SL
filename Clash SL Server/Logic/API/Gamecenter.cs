namespace UCS.Logic.API
{
    using Newtonsoft.Json;

    internal class Gamecenter
    {
        internal ClientAvatar Player;

        [JsonProperty("gc_id")] internal string Identifier;
        [JsonProperty("gc_cert")] internal string Certificate;
        [JsonProperty("gc_bun")] internal string AppBundle;

        /// <summary>
        /// Initializes a new instance of the <see cref="Gamecenter"/> class.
        /// </summary>
        internal Gamecenter()
        {
            // Gamecenter.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Gamecenter"/> class.
        /// </summary>
        /// <param name="_Player">The player.</param>
        internal Gamecenter(ClientAvatar Player)
        {
            this.Player = Player;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Gamecenter"/> is filled.
        /// </summary>
        internal bool Filled => !string.IsNullOrEmpty(this.Identifier) && !string.IsNullOrEmpty(this.Certificate) && !string.IsNullOrEmpty(this.AppBundle);
    }
}