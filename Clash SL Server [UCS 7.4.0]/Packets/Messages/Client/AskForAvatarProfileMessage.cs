using System;
using System.IO;
using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers;
using CSS.Helpers.Binary;
using CSS.Logic;
using CSS.Packets.Messages.Server;

namespace CSS.Packets.Messages.Client
{
    // Packet 14325
    internal class AskForAvatarProfileMessage : Message
    {
        public AskForAvatarProfileMessage(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        long m_vAvatarId;
        long m_vCurrentHomeId;

        internal override void Decode()
        {
            this.m_vAvatarId = this.Reader.ReadInt64();
            if (this.Reader.ReadBoolean())
                this.m_vCurrentHomeId = this.Reader.ReadInt64();
        }

        internal override async void Process()
        {
            try
            {
                Level targetLevel = await ResourcesManager.GetPlayer(m_vAvatarId);
                if (targetLevel != null)
                {
                    targetLevel.Tick();
                    new AvatarProfileMessage(this.Device) { Level = targetLevel }.Send();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
