using Newtonsoft.Json;

namespace UCS.Logic.API
{
    using global::Facebook;

    internal class Facebook
    {
        internal const string ApplicationID = "766523513498250";
        internal const string ApplicationSecret = "6f7a83a21f1686ab59cebf35ae9863d0";
        internal const string ApplicationVersion = "2.8";

        [JsonProperty("fb_id")] internal string Identifier;
        [JsonProperty("fb_token")] internal string Token;

        internal FacebookClient FBClient;
        internal ClientAvatar Player;


        /// <summary>
        /// Initializes a new instance of the <see cref="Facebook"/> class.
        /// </summary>
        internal Facebook()
        {
            // Facebook.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Facebook"/> class.
        /// </summary>
        /// <param name="_Player">The player.</param>
        internal Facebook(ClientAvatar Player)
        {
            this.Player = Player;

            if (this.Filled)
            {
                this.Connect();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Facebook"/> is connected.
        /// </summary>
        internal bool Connected => this.Filled && this.FBClient != null;

        /// <summary>
        /// Gets a value indicating whether this <see cref="Facebook"/> is filled.
        /// </summary>
        internal bool Filled => !string.IsNullOrEmpty(this.Identifier) && !string.IsNullOrEmpty(this.Token);

        /// <summary>
        /// Connects this instance.
        /// </summary>
        internal void Connect()
        {
            this.FBClient = new FacebookClient(this.Token)
            {
                AppId = Facebook.ApplicationID,
                AppSecret = Facebook.ApplicationSecret,
                Version = Facebook.ApplicationVersion
            };

        }

        internal object Get(string Path, bool IncludeIdentifier = true)
        {
            return this.Connected ? this.FBClient.Get("https://graph.facebook.com/v" + Facebook.ApplicationVersion + "/" + (IncludeIdentifier ? this.Identifier + '/' + Path : Path)) : null;
        }
    }
}

