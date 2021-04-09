namespace UCS.Logic.JSONProperty
{
    using System;
    using System.Collections.Generic;
    internal class Units : List<Slot>, ICloneable
    {
        internal ClientAvatar Player;

        /// <summary>
        /// Initializes a new instance of the <see cref="Units"/> class.
        /// </summary>

        internal Units()
        {
            // Units.
        }

        internal Units Clone()
        {
            return this.MemberwiseClone() as Units;
        }
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        internal Units(ClientAvatar _Player)
        {
            this.Player = _Player;
        }
    }
}
