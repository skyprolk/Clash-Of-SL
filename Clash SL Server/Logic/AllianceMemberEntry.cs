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
using System.IO;
using Newtonsoft.Json.Linq;
using CSS.Core;
using CSS.Helpers;

namespace CSS.Logic
{
    internal class AllianceMemberEntry
    {
        #region Public Constructors

        public AllianceMemberEntry(long avatarId)
        {
            m_vAvatarId = avatarId;
            m_vIsNewMember = 0;
            m_vOrder = 1;
            m_vPreviousOrder = 1;
            m_vRole = 1;
            m_vDonatedTroops = 500;
            m_vReceivedTroops = 250;
            m_vWarCooldown = 0;
            m_vWarOptInStatus = 1;
        }

        #endregion Public Constructors

        #region Private Fields

        readonly int m_vDonatedTroops;
        readonly byte m_vIsNewMember;
        readonly int m_vReceivedTroops;
        readonly int[] m_vRoleTable = { 1, 1, 4, 2, 3 };
        readonly int m_vWarCooldown;
        readonly int m_vWarOptInStatus;
        long m_vAvatarId;
        int m_vOrder;
        int m_vPreviousOrder;
        int m_vRole;

        #endregion Private Fields

        #region Public Methods

        public static void Decode(byte[] avatarData)
        {
            using (var br = new CoCSharpPacketReader(new MemoryStream(avatarData)))
            {
            }
        }

        public static int GetDonations() => 150;

        public byte[] Encode()
        {
            var data = new List<byte>();
            var avatar = ResourcesManager.GetPlayer(m_vAvatarId);
            data.AddInt64(m_vAvatarId);
            data.AddString(avatar.GetPlayerAvatar().GetAvatarName());
            data.AddInt32(m_vRole);
            data.AddInt32(avatar.GetPlayerAvatar().GetAvatarLevel());
            data.AddInt32(avatar.GetPlayerAvatar().GetLeagueId());
            data.AddInt32(avatar.GetPlayerAvatar().GetScore());
            data.AddInt32(m_vDonatedTroops);
            data.AddInt32(m_vReceivedTroops);
            data.AddInt32(m_vOrder);
            data.AddInt32(m_vPreviousOrder);
            data.Add(m_vIsNewMember);
            data.AddInt32(m_vWarCooldown);
            data.AddInt32(m_vWarOptInStatus);
            data.Add(1);
            data.AddInt64(m_vAvatarId);
            return data.ToArray();
        }

        public long GetAvatarId() => m_vAvatarId;

        public int GetOrder() => m_vOrder;

        public int GetPreviousOrder() => m_vPreviousOrder;

        public int GetRole() => m_vRole;

        public bool HasLowerRoleThan(int role)
        {
            var result = true;
            if (role < m_vRoleTable.Length && m_vRole < m_vRoleTable.Length)
            {
                if (m_vRoleTable[m_vRole] >= m_vRoleTable[role])
                    result = false;
            }
            return result;
        }

        public byte IsNewMember() => m_vIsNewMember;

        public void Load(JObject jsonObject)
        {
            m_vAvatarId = jsonObject["avatar_id"].ToObject<long>();
            m_vRole = jsonObject["role"].ToObject<int>();
        }

        public JObject Save(JObject jsonObject)
        {
            jsonObject.Add("avatar_id", m_vAvatarId);
            jsonObject.Add("role", m_vRole);
            return jsonObject;
        }

        public void SetAvatarId(long id)
        {
            m_vAvatarId = id;
        }

        public void SetOrder(int order)
        {
            m_vOrder = order;
        }

        public void SetPreviousOrder(int order)
        {
            m_vPreviousOrder = order;
        }

        public void SetRole(int role)
        {
            m_vRole = role;
        }

        #endregion Public Methods
    }
}
