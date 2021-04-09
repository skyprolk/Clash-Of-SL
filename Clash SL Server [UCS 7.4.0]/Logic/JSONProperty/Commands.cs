namespace UCS.Logic.JSONProperty
{
    using System;
    using System.Collections.Generic;
    using UCS.Logic.JSONProperty.Item;

    internal class Commands : List<Battle_Command>
    {
        public new void Add(Battle Battle, Battle_Command Command)
        {
            if (Battle.Preparation_Time > 0) Battle.Preparation_Skip = (int)Math.Round(Battle.Preparation_Time);

            this.Add(Command);
        }
    }
}