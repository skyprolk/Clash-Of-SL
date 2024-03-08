using System;
using Newtonsoft.Json;
using UCS.Core;
using UCS.Files.Logic;

namespace UCS.Logic.JSONProperty
{
    using System.Collections.Generic;
    using System.Linq;

    internal class Achievements : List<Slot>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Achievements"/> class.
        /// </summary>
        /// 
        [JsonConstructor]
        internal Achievements()
        {
            // Achievements.
        }

        internal List<Slot> Completed
        {
            get
            {
                return this.Where(Achievement => Achievement.Count > 0).ToList();
            }
        }

        internal new void AddAchievement(Slot Achievement)
        {
            int Index = this.FindIndex(A => A.Data == Achievement.Data);
            if (Index < 0)
                this.Add(Achievement);
        }

    }
}
