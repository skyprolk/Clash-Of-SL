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

using System;
using System.IO;
using CSS.Core;
using CSS.Files.Logic;
using CSS.Helpers;
using CSS.Logic;

namespace CSS.PacketProcessing.Commands
{
    internal class BuyResourcesCommand : Command
    {
        #region Public Constructors

        public BuyResourcesCommand(CoCSharpPacketReader br)
        {
            m_vResourceId = br.ReadInt32WithEndian();
            m_vResourceCount = br.ReadInt32WithEndian();
            m_vIsCommandEmbedded = br.ReadBoolean();
            if (m_vIsCommandEmbedded)
            {
                Depth++;

                if (Depth >= MaxEmbeddedDepth)
                    throw new ArgumentException(
                        "A command contained embedded command depth was greater than max embedded commands.");
                m_vCommand = CommandFactory.Read(br);
            }
            Unknown1 = br.ReadInt32WithEndian();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
            var rd = (ResourceData) ObjectManager.DataTables.GetDataById(m_vResourceId);
            if (rd != null)
            {
                if (m_vResourceCount >= 1)
                {
                    if (!rd.PremiumCurrency)
                    {
                        var avatar = level.GetPlayerAvatar();
                        var diamondCost = GamePlayUtil.GetResourceDiamondCost(m_vResourceCount, rd);
                        var unusedResourceCap = avatar.GetUnusedResourceCap(rd);
                        if (m_vResourceCount <= unusedResourceCap)
                        {
                            if (avatar.HasEnoughDiamonds(diamondCost))
                            {
                                avatar.UseDiamonds(diamondCost);
                                avatar.CommodityCountChangeHelper(0, rd, m_vResourceCount);
                                if (m_vIsCommandEmbedded)
                                {
                                    Depth++;

                                    if (Depth >= MaxEmbeddedDepth)
                                        throw new ArgumentException(
                                            "A command contained embedded command depth was greater than max embedded commands.");

                                    ((Command) m_vCommand).Execute(level);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion Public Methods

        #region Private Fields

        readonly object m_vCommand;
        readonly bool m_vIsCommandEmbedded;
        readonly int m_vResourceCount;
        readonly int m_vResourceId;
        readonly int Unknown1;

        #endregion Private Fields
    }
}