/*
 * Program : Clash Of SL Server
 * Description : A C# Writted 'Clash of SL' Server Emulator !
 *
 * Authors:  Sky Tharusha <Founder at Sky Production>,
 *           And the Official DARK Developement Team
 *
 * Copyright (c) 2021  Sky Production
 * All Rights Reserved.
 */

using CSS.Core;
using CSS.Core.Network;
using CSS.Logic;
using CSS.PacketProcessing.Messages.Server;

namespace CSS.PacketProcessing.GameOpCommands
{
    internal class MaxRessourcesCommand : GameOpCommand
    {
        #region Public Constructors

        public MaxRessourcesCommand(string[] Args)
        {
            SetRequiredAccountPrivileges(0);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
            if (level.GetAccountPrivileges() >= GetRequiredAccountPrivileges())
            {
                var p = level.GetPlayerAvatar();
                p.SetResourceCount(ObjectManager.DataTables.GetResourceByName("Gold"), 999999999);
                p.SetResourceCount(ObjectManager.DataTables.GetResourceByName("Elixir"), 999999999);
                p.SetResourceCount(ObjectManager.DataTables.GetResourceByName("DarkElixir"), 999999999);
                p.SetDiamonds(999999);
                var own = new OwnHomeDataMessage(level.GetClient(), level);
                PacketManager.ProcessOutgoingPacket(own);
            }
            else
                SendCommandFailedMessage(level.GetClient());
        }

        #endregion Public Methods
    }
}