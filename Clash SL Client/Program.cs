using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC
{
    class Program
    {
        static void Main(string[] args)
        {
            Client _Client = new Client();
            _Client.Connect("127.0.0.1", 9339);
        }
    }
}
