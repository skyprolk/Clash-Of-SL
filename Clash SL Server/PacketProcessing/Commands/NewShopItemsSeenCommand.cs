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

using System.IO;
using CSS.Helpers;

namespace CSS.PacketProcessing.Commands
{
    internal class NewShopItemsSeenCommand : Command
    {
        #region Public Constructors

        public NewShopItemsSeenCommand(CoCSharpPacketReader br)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public uint NewShopItemNumber { get; set; }
        public uint Unknown1 { get; set; }
        public uint Unknown2 { get; set; }
        public uint Unknown3 { get; set; }

        #endregion Public Properties
    }
}