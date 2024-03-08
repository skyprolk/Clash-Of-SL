using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSS.Core.Checker
{
    internal class DirectoryChecker
    {
        public DirectoryChecker()
        {
            Directorys();
            Files();
            LoadFilter();
        }

        public static List<string> badwords = new List<string>();

        public static void LoadFilter()
        {
            if (File.Exists("filter.css"))
            {
                StreamReader sr = new StreamReader(@"filter.css");
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    badwords.Add(line);
                }
            }
        }

        public static void Directorys()
        {
            string[] Dirs = 
            {
                "Logs",
                "Patch",
                "Tools",
                "Library",
                "Gamefiles",
                "Gamefiles/update"
            };

            foreach (string Dir in Dirs)
            {
                if (!Directory.Exists(@Dir))
                {
                    Directory.CreateDirectory(@Dir);
                }
            }
        }

        public static void Files()
        {
			if (!File.Exists("filter.css"))
            {
                using (var s = new StreamWriter("filter.css"))
                {
                    s.WriteLine("./savegame");
                }
            }
        }
    }
}
