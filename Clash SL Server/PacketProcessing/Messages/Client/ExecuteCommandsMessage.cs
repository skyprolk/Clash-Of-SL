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
using CSS.Helpers;
using CSS.Logic;

namespace CSS.PacketProcessing.Messages.Client
{
    internal class ExecuteCommandsMessage : Message
    {
        #region Public Constructors

        public ExecuteCommandsMessage(PacketProcessing.Client client, CoCSharpPacketReader br) : base(client, br)
        {
        }

        #endregion Public Constructors

        #region Public Fields

        public uint Checksum;

        public byte[] NestedCommands;
        public uint NumberOfCommands;
        public uint Subtick;

        #endregion Public Fields

        #region Public Methods

        public override void Decode()
        {
            using (var br = new CoCSharpPacketReader(new MemoryStream(GetData())))
            {
                Subtick = br.ReadUInt32WithEndian();
                Checksum = br.ReadUInt32WithEndian();
                NumberOfCommands = br.ReadUInt32WithEndian();

                if (NumberOfCommands > 0 && NumberOfCommands < 20)
                {
                    NestedCommands = br.ReadBytes(GetLength());
                }
            }
        }

        public override void Process(Level level)
        {
            try
            {
                level.Tick();

                if (NumberOfCommands > 0 && NumberOfCommands < 20)
                    using (var br = new CoCSharpPacketReader(new MemoryStream(NestedCommands)))
                        for (var i = 0; i < NumberOfCommands; i++)
                        {
                            var obj = CommandFactory.Read(br);
                            if (obj != null)
                            {
                                ((Command) obj).Execute(level);
                            }
                            else
                                break;
                        }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion Public Methods
    }
}