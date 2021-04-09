
using System;
using System.IO;
using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Packets.Messages.Server;
using CSS.Packets.Commands.Client;
using System.Text;
using static CSS.Logic.ClientAvatar;
using System.Collections.Generic;
using CSS.Files.Logic;

namespace CSS.Packets.Messages.Client
{
    // Packet 14101
    internal class GoHomeMessage : Message
    {
        public GoHomeMessage(Device device, Reader reader) : base(device, reader)
        {
        }

        internal override void Decode()
        {
            this.State = this.Reader.ReadInt32();
        }

        public int State;

        internal override async void Process()
        {
            try
            {
                ClientAvatar player = this.Device.Player.Avatar;
                Level _level = this.Device.Player;
                int scores = player.GetScore();
                Resources(_level, scores);
                
                if (State == 1)
                {
                    this.Device.PlayerState = Logic.Enums.State.WAR_EMODE;
                    this.Device.Player.Tick();
                    new OwnHomeDataMessage(this.Device, this.Device.Player).Send();
                }
                else if (this.Device.PlayerState == Logic.Enums.State.LOGGED)
                {
                    ResourcesManager.DisconnectClient(Device);
                }
                else
                {
                    this.Device.PlayerState = Logic.Enums.State.LOGGED;
                    this.Device.Player.Tick();
                    Alliance alliance = ObjectManager.GetAlliance(this.Device.Player.Avatar.AllianceId);
                    new OwnHomeDataMessage(Device, this.Device.Player).Send();
                    if (alliance != null)
                    {
                        new AllianceStreamMessage(Device, alliance).Send();
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }

        public void Resources(Level level,int scores)
        {
            Random random = new Random();
            ClientAvatar avatar = level.Avatar;
            int currentGold = avatar.GetResourceCount(CSVManager.DataTables.GetResourceByName("Gold"));
            int currentElixir = avatar.GetResourceCount(CSVManager.DataTables.GetResourceByName("Elixir"));
            int currentDarkElixir = avatar.GetResourceCount(CSVManager.DataTables.GetResourceByName("DarkElixir"));
            int currentDiamond = 0;
            int multiply = 1;
            int currentScore = avatar.GetScore();
            ResourceData goldLocation = CSVManager.DataTables.GetResourceByName("Gold");
            ResourceData elixirLocation = CSVManager.DataTables.GetResourceByName("Elixir");
            ResourceData darkElixirLocation = CSVManager.DataTables.GetResourceByName("DarkElixir");


            if (scores <= 200)
            {
                multiply = 1;
                currentGold = currentGold + multiply * 5;
                currentElixir = currentElixir + multiply * 5;
                currentDarkElixir = currentDarkElixir + scores;
                currentDiamond = multiply;
                if(scores <= 100)
                {
                    currentScore = currentScore + 30;
                }
                else
                {
                    currentScore = currentScore + random.Next(1,30) + multiply;
                }
            }
            else if (scores >= 1 && scores <= 400)
            {
                multiply = 4;
                currentGold = currentGold + multiply * scores;
                currentElixir = currentElixir + multiply * scores;
                currentDarkElixir = currentDarkElixir + scores;
                currentDiamond = multiply;
                currentScore = currentScore + random.Next(1, 40) + multiply;
            }
            else if (scores >= 401 && scores <= 800)
            {
                multiply = 8;
                currentGold = currentGold + multiply * scores;
                currentElixir = currentElixir + multiply * scores;
                currentDarkElixir = currentDarkElixir + scores;
                currentDiamond = multiply;
                currentScore = currentScore + random.Next(1, 35) + multiply;
            }
            else if (scores >= 801 && scores <= 1400)
            {
                multiply = 14;
                currentGold = currentGold + multiply * scores;
                currentElixir = currentElixir + multiply * scores;
                currentDarkElixir = currentDarkElixir + scores;
                currentDiamond = multiply;
                currentScore = currentScore + random.Next(1, 30) + multiply;
            }
            else if (scores >= 1401 && scores <= 2000)
            {
                multiply = 20;
                currentGold = currentGold + multiply * scores;
                currentElixir = currentElixir + multiply * scores;
                currentDarkElixir = currentDarkElixir + scores;
                currentDiamond = multiply;
                currentScore = currentScore + random.Next(1, 25) + multiply;
            }
            else if (scores >= 2001 && scores <= 2600)
            {
                multiply = 26;
                currentGold = currentGold + multiply * scores;
                currentElixir = currentElixir + multiply * scores;
                currentDarkElixir = currentDarkElixir + scores;
                currentDiamond = multiply;
                currentScore = currentScore + random.Next(1, 20) + multiply;
            }
            else if (scores >= 2601 && scores <= 3200)
            {
                multiply = 32;
                currentGold = currentGold + multiply * scores;
                currentElixir = currentElixir + scores;
                currentDarkElixir = currentDarkElixir + scores;
                currentDiamond = multiply;
                currentScore = currentScore + random.Next(1, 15) + multiply;
            }
            else if (scores >= 3201 && scores <= 4100)
            {
                multiply = 41;
                currentGold = currentGold + multiply * scores;
                currentElixir = currentElixir + scores;
                currentDarkElixir = currentDarkElixir + scores;
                currentDiamond = multiply;
                currentScore = currentScore + random.Next(1, 10) + multiply;

            }
            else
            {
                multiply = random.Next(42, 50);
                currentGold = currentGold + multiply*scores;
                currentElixir = currentElixir + scores;
                currentDarkElixir = currentDarkElixir + scores;
                currentDiamond = multiply;
                currentScore = currentScore + random.Next(1, 5) + multiply;
            }

            avatar.SetResourceCount(goldLocation, currentGold);
            avatar.SetResourceCount(elixirLocation, currentElixir);
            avatar.SetResourceCount(darkElixirLocation, currentDarkElixir);
            avatar.AddDiamonds(currentDiamond);
            avatar.SetScore(currentScore);
            new AvatarStreamMessage(this.Device, 2).Send();

        } 
    }
}
