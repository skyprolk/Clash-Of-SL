using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSS.Core;
using CSS.Core.Network;
using CSS.Helpers;
using CSS.Helpers.List;
using CSS.Logic;
using CSS.Logic.StreamEntry;

namespace CSS.Packets.Messages.Server
{
    // Packet 24311
    internal class AllianceStreamMessage : Message
    {
        readonly Alliance m_vAlliance;

        public AllianceStreamMessage(Device client, Alliance alliance) : base(client)
        {
            this.Identifier = 24311;
            m_vAlliance = alliance;
        }

        internal override void Encode()
        {
            var chatMessages = m_vAlliance.m_vChatMessages.ToList();
            this.Data.AddInt(0);
            this.Data.AddInt(chatMessages.Count);
            int count = 0;
            foreach (StreamEntry chatMessage in chatMessages)
            {
                this.Data.AddRange(chatMessage.Encode());
                count++;
                if (count >= 150)
                {
                    break;
                }
            }
        }
    }
}
