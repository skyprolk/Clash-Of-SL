namespace UCS.Logic.JSONProperty
{
    using System.Collections.Generic;

    internal class Upgrades : List<Slot>
    {
        internal ClientAvatar Player;

        /// <summary>
        /// Initializes a new instance of the <see cref="Upgrades"/> class.
        /// </summary>

        internal Upgrades()
        {
            // Upgrades.
        }

        internal Upgrades(ClientAvatar _Player)
        {
            this.Player = _Player;
        }
    }
}
