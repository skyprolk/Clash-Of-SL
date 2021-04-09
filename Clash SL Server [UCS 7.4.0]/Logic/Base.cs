using System.Collections.Generic;
using System.IO;
using CSS.Helpers;
using CSS.Helpers.Binary;
using CSS.Helpers.List;

namespace CSS.Logic
{
    internal class Base
    {
        public Base()
        {
        }

        public virtual void Decode(byte[] baseData)
        {
        }

        public virtual byte[] Encode()
        {
            List<byte> data = new List<byte>();
            return data.ToArray();
        }
    }
}
