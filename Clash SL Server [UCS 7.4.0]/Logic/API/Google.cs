namespace UCS.Logic.API
{
    using System.IO;
    using System.Threading;

    using Newtonsoft.Json;

    using global::Google.Apis.Games.v1;
    using global::Google.Apis.Auth.OAuth2;
    using global::Google.Apis.Services;
    using global::Google.Apis.Util.Store;
    using UCS.Packets.Messages.Server.Api;
    using UCS.Core;
    using UCS.Core.Network;

    internal class Google
    {

        internal const string GlobalPlayersID = "CgkIg4DcwsQaEAIQAg";
        internal const string GlobalClansID = "CgkIg4DcwsQaEAIQAw";
        internal const string LocalPlayersID = "CgkIg4DcwsQaEAIQBA";
        internal const string LocalClansID = "CgkIg4DcwsQaEAIQBQ";

        [JsonProperty("gg_id")] internal string Identifier;
        [JsonProperty("gg_token")] internal string Token;

        internal UserCredential OCredentials;
        internal GamesService OClient;
        internal ClientAvatar Player;

        /// <summary>
        /// Initializes a new instance of the <see cref="Google"/> class.
        /// </summary>
        internal Google()
        {
            // Google.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Google"/> class.
        /// </summary>
        /// <param name="_Player">The player.</param>
        internal Google(ClientAvatar Player)
        {
            this.Player = Player;

            if (this.Filled)
            {
                this.Connect();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Google"/> is filled.
        /// </summary>
        internal bool Filled
        {
            get
            {
                return !string.IsNullOrEmpty(this.Identifier) && !string.IsNullOrEmpty(this.Token);
            }
        }

        /// <summary>
        /// Gets the credentials.
        /// </summary>
        internal void GetCredentials()
        {
            using (var Stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                this.OCredentials = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(Stream).Secrets, new[]
                {
                    GamesService.Scope.Games, GamesService.Scope.PlusLogin
                }, "com.ultrapowa.coc", CancellationToken.None, new FileDataStore("Com.Ultrapowa.CoC")).Result;
            }
        }

        /// <summary>
        /// Logs into the YouTube API's Servers.
        /// </summary>
        internal void Login()
        {
            this.OClient = new GamesService(new BaseClientService.Initializer
            {
                HttpClientInitializer = this.OCredentials,
                ApplicationName = "com.ultrapowa.coc",
                ApiKey = "AIzaSyA8rVhlt2L1NqqUzaEaVTidBfPIL-QU4HM"
            });
        }

        /// <summary>
        /// Connects this instance.
        /// </summary>
        internal void Connect()
        {
            this.GetCredentials();
            this.Login();
            //new Google_Connect_OK(ResourcesManager.GetPlayer(this.Player.UserID).Result.Client).Send();
            var ScoreSubmit = this.OClient.Scores.Submit(Google.GlobalPlayersID, this.Player.Trophies);
            var ScoreResult = ScoreSubmit.Execute();
        }
    }
}
