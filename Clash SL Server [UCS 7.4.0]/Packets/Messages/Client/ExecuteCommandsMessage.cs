using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Messages.Client
{
    // Packet 14102
    internal class ExecuteCommandsMessage : Message
    {
        public ExecuteCommandsMessage(Device device, Reader reader) : base(device, reader)
        {
        }

        internal int CTick;
        internal int STick;
        internal int Checksum;
        internal int Count;

        internal byte[] Commands;
        internal List<Command> LCommands;

        internal override void Decode()
        {
            this.CTick = this.Reader.ReadInt32();
            this.Checksum = this.Reader.ReadInt32();
            this.Count = this.Reader.ReadInt32();
            this.STick =  this.STick = (int) Math.Floor(DateTime.UtcNow.Subtract(this.Device.Player.Avatar.LastTickSaved).TotalSeconds * 20);
            this.LCommands = new List<Command>((int) this.Count);
            this.Commands = this.Reader.ReadBytes((int) (this.Reader.BaseStream.Length - this.Reader.BaseStream.Position));
        }

        internal override void Process()
        {

            this.Device.Player.Tick();

            if (this.Count > -1 && this.Count <= 400)
            {
                using (Reader Reader = new Reader(this.Commands))
                {
                    for (int _Index = 0; _Index < this.Count; _Index++)
                    {
                        int CommandID = Reader.ReadInt32();
                        if (CommandFactory.Commands.ContainsKey(CommandID))
                        {
                            Logger.Write("Command '" + CommandID + "' is handled");
                            Command Command = Activator.CreateInstance(CommandFactory.Commands[CommandID], Reader, this.Device,CommandID) as Command;

                            if (Command != null)
                            {
                                Command.Decode();
                                Command.Process();

                                this.LCommands.Add(Command);
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Logger.Write("Command " + CommandID + " has not been handled.");
                            if (this.LCommands.Any())
                                Logger.Write("Previous command was " + this.LCommands.Last().Identifier + ". [" + (_Index + 1) + " / " + this.Count + "]");
                            Console.ResetColor();
                            break;

                        }
                    }
                }
            }
            else
            {
                new OutOfSyncMessage(this.Device).Send();
            }
        }
    }
}