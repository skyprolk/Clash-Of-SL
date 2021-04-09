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
using CSS.Helpers;
using CSS.Logic;

namespace CSS.PacketProcessing.Commands
{
    internal class FreeWorkerCommand : Command
    {
        #region Public Fields

        public int m_vTimeLeftSeconds;

        #endregion Public Fields

        #region Public Constructors

        public FreeWorkerCommand(CoCSharpPacketReader br)
        {
            m_vTimeLeftSeconds = br.ReadInt32WithEndian();
            m_vIsCommandEmbedded = br.ReadBoolean();
            if (m_vIsCommandEmbedded)
            {
                Depth++;
                if (Depth >= MaxEmbeddedDepth)
                    throw new ArgumentException(
                        "A command contained embedded command depth was greater than max embedded commands.");
                m_vCommand = CommandFactory.Read(br);
            }
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Execute(Level level)
        {
            if (level.WorkerManager.GetFreeWorkers() == 0)
            {
                level.WorkerManager.FinishTaskOfOneWorker();
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

        #endregion Public Methods

        #region Private Fields

        readonly object m_vCommand;
        readonly bool m_vIsCommandEmbedded;

        #endregion Private Fields
    }
}