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

namespace CSS.Logic.AvatarStreamEntry
{
    internal class AllianceKickOutStreamEntry : AvatarStreamEntry
    {
        #region Private Fields

        int m_vAllianceBadgeData;
        long m_vAllianceId;
        string m_vAllianceName;
        string m_vMessage;

        #endregion Private Fields

        #region Public Methods

        public override byte[] Encode()
        {
            var data = new List<byte>();

            data.AddRange(base.Encode());
            data.AddString(m_vMessage);
            data.AddInt64(m_vAllianceId);
            data.AddString(m_vAllianceName);
            data.AddInt32(m_vAllianceBadgeData);
            data.Add(1);
            data.AddInt32(0x29);
            data.AddInt32(0x0084E879);

            return data.ToArray();
        }

        public override int GetStreamEntryType()
        {
            return 5;
        }

        public void SetAllianceBadgeData(int data)
        {
            m_vAllianceBadgeData = data;
        }

        public void SetAllianceId(long id)
        {
            m_vAllianceId = id;
        }

        public void SetAllianceName(string name)
        {
            m_vAllianceName = name;
        }

        public void SetMessage(string message)
        {
            m_vMessage = message;
        }

        #endregion Public Methods
    }
}