using CSS.Helpers.Binary;
using CSS.Logic;

namespace CSS.Packets.Commands.Client
{
    // Packet 534
    internal class CancelShieldCommand : Command
    {
        public CancelShieldCommand(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        internal override void Decode()
        {
            this.Tick = this.Reader.ReadInt32();
        }

        public int Tick;

        internal override void Process()
        {
            ClientAvatar player = this.Device.Player.Avatar;

            //if (player.GetShieldTime > 0)
            //{
                  player.m_vShieldTime = 0;
                //player.SetProtectionTime(1800);
            //}
            /*else 
            {
                player.SetShieldTime(0);
                player.SetProtectionTime(0);
            }*/
        }
    }      
}
