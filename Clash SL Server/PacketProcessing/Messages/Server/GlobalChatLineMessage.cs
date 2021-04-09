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
using CSS.Helpers;
using CSS.Logic;

namespace CSS.PacketProcessing.Messages.Server
{
    //Packet 24715
    internal class GlobalChatLineMessage : Message
    {
        #region Public Constructors

        public GlobalChatLineMessage(PacketProcessing.Client client) : base(client)
        {
            SetMessageType(24715);

            m_vMessage = "default";
            m_vPlayerName = "default";
            m_vHomeId = 1;
            m_vCurrentHomeId = 1;
            m_vPlayerLevel = 1;
            m_vHasAlliance = false;
        }

        #endregion Public Constructors

        #region Private Fields

        readonly int m_vPlayerLevel;
        int m_vAllianceIcon;
        long m_vAllianceId;
        string m_vAllianceName;
        long m_vCurrentHomeId;
        bool m_vHasAlliance;
        long m_vHomeId;
        int m_vLeagueId;
        string m_vMessage;
        string m_vPlayerName;

        #endregion Private Fields

        #region Public Methods

        public override void Encode()
        {
            var pack = new List<byte>();

            pack.AddString(m_vMessage);
            pack.AddString(m_vPlayerName);
            pack.AddInt32(m_vPlayerLevel);
            pack.AddInt32(m_vLeagueId);
            pack.AddInt64(m_vHomeId);
            pack.AddInt64(m_vCurrentHomeId);

            if (!m_vHasAlliance)
            {
                pack.Add(0);
            }
            else
            {
                pack.Add(1);
                pack.AddInt64(m_vAllianceId);
                pack.AddString(m_vAllianceName);
                pack.AddInt32(m_vAllianceIcon);
            }

            Encrypt(pack.ToArray());
        }

        public void SetAlliance(Alliance alliance)
        {
            if (alliance != null)
            {
                m_vHasAlliance = true;
                m_vAllianceId = alliance.GetAllianceId();
                m_vAllianceName = alliance.GetAllianceName();
                m_vAllianceIcon = alliance.GetAllianceBadgeData();
            }
        }

        public void SetChatMessage(string message)
        {
            m_vMessage = message;
        }

        public void SetLeagueId(int leagueId)
        {
            m_vLeagueId = leagueId;
        }

        public void SetPlayerId(long id)
        {
            m_vHomeId = id;
            m_vCurrentHomeId = id;
        }

        public void SetPlayerName(string name)
        {
            m_vPlayerName = name;
        }

        #endregion Public Methods
    }
}