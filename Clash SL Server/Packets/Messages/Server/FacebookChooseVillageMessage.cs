using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCS.Core;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.Packets.Messages.Server
{
    class FacebookChooseVillageMessage : Message
    {
        public FacebookChooseVillageMessage(Packets.Client client, Level _Level) : base(client)
        {
            SetMessageType(24262);
            _Player = _Level;
        }

        public Level _Player { get; set; }

        public override async void Encode()
        {
            try
            {
                ClientAvatar _ClientAvatar = Client.GetLevel().GetPlayerAvatar();

                List<byte> _data = new List<byte>();
                _data.AddString(null);
                _data.Add(1);

                _data.AddInt32(_ClientAvatar.GetAvatarHighIdInt());
                _data.AddInt32(_ClientAvatar.GetAvataLowIdInt());

                _data.AddString(_Player.GetPlayerAvatar().GetUserToken());
                _data.AddRange(await _Player.GetPlayerAvatar().Encode());

                Encrypt(_data.ToArray());
            }
            catch (Exception)
            {
            }
        }
    }
}
