using System.Collections.Generic;
using CSS.Helpers.List;
using CSS.Logic;

namespace CSS.Packets.Messages.Server
{
    // Packets 24411
    internal class AvatarStreamMessage : Message
    {
        public AvatarStreamMessage(Device client,int _type) : base(client)
        {
            this.Identifier = 24411;
            this.Type = _type;
        }

        public int Type;

        internal override void Encode()
        {
            string StreamTest = @"{""loot"":[[3000002,999999999],[3000001,999999999]],""availableLoot"":[[3000000,0],[3000001,145430],[3000002,142872],[3000003,517]],""units"":[[4000001,58]],""spells"":[],""levels"":[[4000001,4]],""stats"":{""townhallDestroyed"":false,""battleEnded"":true,""allianceUsed"":false,""destructionPercentage"":6,""battleTime"":90,""originalAttackerScore"":6022,""attackerScore"":-10,""originalDefenderScore"":1056,""defenderScore"":18,""allianceName"":""CSS"",""attackerStars"":0,""homeID"":[0,5],""allianceBadge"":1526735450,""allianceBadge2"":1660949336,""allianceID"":[88,884629],""deployedHousingSpace"":168,""armyDeploymentPercentage"":5}}";
            
            ClientAvatar pl = this.Device.Player.Avatar;
            this.Data.AddInt(1); //Stream Ammount
            this.Data.AddInt(Type); //Stream Type, 2 = attacked, 7 = defended;
            this.Data.AddLong(1); //Stream ID
            this.Data.Add(1);
            this.Data.AddInt(pl.HighID);
            this.Data.AddInt(pl.LowID);
            this.Data.AddString("CSS Server AI"); //Attacker Name
            this.Data.AddInt(1);
            this.Data.AddInt(0);
            this.Data.AddInt(446); //Age
            this.Data.Add(2); // 2 = new, 0 = old;
            this.Data.AddString(StreamTest);
            this.Data.AddInt(0);
            this.Data.Add(1);
            this.Data.AddInt(8);
            this.Data.AddInt(709);
            this.Data.AddInt(0);
            this.Data.Add(1);
            this.Data.AddLong(1);
            this.Data.AddInt(int.MaxValue);
        }
    }
}
