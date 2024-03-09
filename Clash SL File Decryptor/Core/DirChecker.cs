using System.IO;

namespace CSFD.Core
{
    class DirChecker
    {
        public static string[] allFolders = new string[4] { "Decrypted - CSV", "Encrypted - CSV", "Decrypted - SC", "Encrypted - SC" };

        public static void Check()
        {
            if (!Directory.Exists(@"In"))
            {
                Directory.CreateDirectory(@"In");
            }

            foreach (string folder in allFolders)
            {
                var path = "In/" + folder;

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
        }
    }
}