using System;
using System.Net;
using CSS.Core;
using CSS.Core.Checker;
using CSS.Helpers.Binary;

namespace CSS.Packets.Commands.Client
{
    // Packet 521
    internal class FreeWorkerCommand : Command
    {
        public int m_vTimeLeftSeconds;

        public FreeWorkerCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.m_vTimeLeftSeconds = this.Reader.ReadInt32();
            this.m_vIsCommandEmbedded = this.Reader.ReadBoolean();

            if (m_vIsCommandEmbedded)
            {
                Depth++;
                if (Depth >= MaxEmbeddedDepth)
                {
                    Console.WriteLine("Detected CSS.Exploit");
                    return;
                }
                Depth = Depth;
            }

        }

        internal override void Process()
        {
            if (Depth >= MaxEmbeddedDepth)
            {
                IPEndPoint r = this.Device.Socket.RemoteEndPoint as IPEndPoint;
                ConnectionBlocker.AddNewIpToBlackList(r.Address.ToString());
                ResourcesManager.DropClient(this.Device.Socket.Handle);
            }

            if (this.Device.Player.WorkerManager.GetFreeWorkers() == 0)

            {
                Depth = 0;
                this.Device.Player.WorkerManager.FinishTaskOfOneWorker();
                if (this.m_vIsCommandEmbedded)
                {
                    int CommandID = this.Reader.ReadInt32();
                    Command _Command =  Activator.CreateInstance(CommandFactory.Commands[CommandID], this.Reader, this.Device, CommandID) as Command;
                    if (_Command != null)
                    {
                        _Command.Decode();
                        _Command.Process();
                    }
                }

            }
        }

        internal object m_vCommand;
        internal bool m_vIsCommandEmbedded;
    }
}