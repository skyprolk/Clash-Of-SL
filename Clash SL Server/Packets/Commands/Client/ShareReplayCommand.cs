using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers.Binary;
using UCS.Logic;
using UCS.Logic.StreamEntry;
using UCS.Packets.Messages.Server;

namespace UCS.Packets.Commands.Client
{
    // Packet 542
    class ShareReplayCommand : Command
    {
        public ShareReplayCommand(Reader Reader, Device _Device, int _ID) : base(Reader, _Device, _ID)
        {
        }

        internal override void Decode()
        {
        }

        internal int Tick { get; set; }

        internal override async void Process()
        {
            /*try
            {
                ClientAvatar _Player = this.Device.Player.Avatar;
                Alliance _Alliance = await ObjectManager.GetAlliance(_Player.AllianceID);

                ShareStreamEntry _CM = new ShareStreamEntry();
                _CM.SetReplayjson(File.ReadAllText("replay-json.txt"));
                _CM.SetEnemyName("Ultrapowa");
                _CM.SetMessage("Look at this battle! :D");
                _CM.SetSenderId(_Player.GetId());
                _CM.SetSenderName(_Player.Username);
                _CM.SetSenderRole(await _Player.GetAllianceRole());

                _Alliance.AddChatMessage(_CM);

                foreach (AllianceMemberEntry _Member in _Alliance.GetAllianceMembers())
                {
                    Level _M = await ResourcesManager.GetPlayer(_Member.GetAvatarId());
                    AllianceStreamEntryMessage p = new AllianceStreamEntryMessage(_M.Client);
                    p.SetStreamEntry(_CM);
                    p.Send();
                }
            }
            catch (Exception)
            {
            }*/
        }
    }
}
