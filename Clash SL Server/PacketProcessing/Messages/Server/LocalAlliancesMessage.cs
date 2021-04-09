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

using System.Collections.Generic;
using System.Linq;
using CSS.Core;
using CSS.Helpers;
using CSS.Logic;

namespace CSS.PacketProcessing.Messages.Server
{
    internal class LocalAlliancesMessage : Message
    {
        #region Private Fields

        List<Alliance> m_vAlliances;

        #endregion Private Fields

        #region Public Constructors

        public LocalAlliancesMessage(PacketProcessing.Client client) : base(client)
        {
            SetMessageType(24402);
            m_vAlliances = new List<Alliance>();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Encode()
        {
            var packet = new List<byte>();
            var data = new List<byte>();

            var i = 1;
            foreach (var alliance in ObjectManager.GetInMemoryAlliances().OrderByDescending(t => t.GetScore()))
            {
                var all = alliance.GetAllianceId();
                data.AddInt64(all);
                data.AddString(alliance.GetAllianceName() + " #" + alliance.GetAllianceId());
                data.AddInt32(i);
                data.AddInt32(alliance.GetScore());
                data.AddInt32(i);
                data.AddInt32(alliance.GetAllianceBadgeData());
                data.AddInt32(alliance.GetAllianceMembers().Count);
                data.AddInt32(0);
                data.AddInt32(alliance.GetAllianceLevel());
                if (i >= 101)
                    break;
                i++;
            }
            packet.AddInt32(i - 1);
            packet.AddRange(data.ToArray());
            Encrypt(packet.ToArray());
        }

        #endregion Public Methods
    }
}
