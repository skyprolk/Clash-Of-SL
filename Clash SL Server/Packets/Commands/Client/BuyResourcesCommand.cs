using System;
using System.IO;
using System.Net;
using CSS.Core;
using CSS.Core.Checker;
using CSS.Files.Logic;
using CSS.Helpers;
using CSS.Helpers.Binary;
using System.Diagnostics;

namespace CSS.Packets.Commands.Client
{
    // Packet 518
    internal class BuyResourcesCommand : Command
    {
        internal object m_vCommand;
        internal bool m_vIsCommandEmbedded;
        internal int m_vResourceCount;
        internal int m_vResourceId;
        internal int Unknown1;

        public BuyResourcesCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
       
        }

        internal override void Decode()
        {
            this.m_vResourceCount = this.Reader.ReadInt32();
            this.m_vResourceId = this.Reader.ReadInt32();
            this.m_vIsCommandEmbedded = this.Reader.ReadBoolean();
            if (m_vIsCommandEmbedded)
            {
                Depth++;
                if (Depth >= MaxEmbeddedDepth)
                {
                    Console.WriteLine("Detected CSS Exploit.");
                    Logger.Say("Detected CSS Exploit. Banning IP.");
                }
                Depth = Depth;
            }
            this.Unknown1 = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            if (Depth >= MaxEmbeddedDepth)
            {
                IPEndPoint r = this.Device.Socket.RemoteEndPoint as IPEndPoint;
                ConnectionBlocker.AddNewIpToBlackList(r.Address.ToString());
                ResourcesManager.DropClient(this.Device.Socket.Handle);
            }
            else
            {
                var rd = (ResourceData) CSVManager.DataTables.GetDataById(m_vResourceId);
                if (rd != null)
                {
                    if (m_vResourceCount >= 1)
                    {
                        if (!rd.PremiumCurrency)
                        {
                            var avatar = this.Device.Player.Avatar;
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
                                        int CommandID = this.Reader.ReadInt32();
                                        Command _Command =
                                            Activator.CreateInstance(CommandFactory.Commands[CommandID], this.Reader,
                                                this.Device, CommandID) as Command;
                                        if (_Command != null)
                                        {
                                            _Command.Decode();
                                            _Command.Process();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}