namespace CSS.Logic.API
{
    using global::Facebook;

    internal class FacebookApi
    {
        internal const string ApplicationID = "766523513498250";
        internal const string ApplicationSecret = "6f7a83a21f1686ab59cebf35ae9863d0";
        internal const string ApplicationVersion = "2.8";


        internal FacebookClient FBClient;
        internal Level Player;


        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApi"/> class.
        /// </summary>
        internal FacebookApi()
        {
            // Facebook.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApi"/> class.
        /// </summary>
        /// <param name="_Player">The player.</param>
        internal FacebookApi(Level Player)
        {
            this.Player = Player;

            if (this.Filled)
            {
                this.Connect();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="FacebookApi"/> is connected.
        /// </summary>
        internal bool Connected => this.Filled && this.FBClient != null;

        /// <summary>
        /// Gets a value indicating whether this <see cref="FacebookApi"/> is filled.
        /// </summary>
        internal bool Filled => !string.IsNullOrEmpty(this.Player.Avatar.FacebookId) && !string.IsNullOrEmpty(this.Player.Avatar.FacebookToken);

        /// <summary>
        /// Connects this instance.
        /// </summary>
        internal void Connect()
        {
            this.FBClient = new FacebookClient(this.Player.Avatar.FacebookToken)
            {
                AppId = FacebookApi.ApplicationID,
                AppSecret = FacebookApi.ApplicationSecret,
                Version = FacebookApi.ApplicationVersion
            };

        }

        internal object Get(string Path, bool IncludeIdentifier = true)
        {
            return this.Connected ? this.FBClient.Get("https://graph.facebook.com/v" + FacebookApi.ApplicationVersion + "/" + (IncludeIdentifier ? this.Player.Avatar.FacebookId + '/' + Path : Path)) : null;
        }
    }
}

