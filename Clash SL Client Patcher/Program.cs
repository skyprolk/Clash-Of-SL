using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;

namespace CSCP
{
    internal class Patcher
    {
        internal static string AssemblyVersion
        {
            get
            {
                return "v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }
        
        internal static void Main(string[] args)
        {

            Console.Title = "Clash SL Client Patcher " + AssemblyVersion + " - © " + DateTime.Now.Year;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
╔═══╗╔═══╗╔═══╗╔╗╔═╗
╚╗╔╗║║╔═╗║║╔═╗║║║║╔╝
─║║║║║║─║║║╚═╝║║╚╝╝
─║║║║║╚═╝║║╔╗╔╝║╔╗║
╔╝╚╝║║╔═╗║║║║╚╗║║║╚╗
╚═══╝╚╝─╚╝╚╝╚═╝╚╝╚═╝
            ");
            Console.WriteLine("Clash SL Client Patcher " + AssemblyVersion);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[CSCP] Intializing Preflight checks");
            if (Directory.Exists("Original"))
            {
                Console.WriteLine("[Step 1/3] Flies Directory exists Skipping creation step");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Step 1/3] Flies Directory does not exist creating the directory");
                Directory.CreateDirectory("Original");
            }
            Thread.Sleep(1000);
            if (Directory.Exists("Patched"))
            {
                Console.WriteLine("[Step 2/3] Patched Directory exists Skipping creation step");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Step 2/3] Patched Directory does not exist creating the directory");
                Directory.CreateDirectory("Patched");
            }
            Thread.Sleep(1000);
            DirectoryInfo _Files = new DirectoryInfo("Original");

            if (File.Exists(_Files.FullName + @"\libg.so"))
            {
                Console.WriteLine("[Step 3/3] libg.so file exists within Files folder proceeding");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Step 3/3] libg.so file doesn't exist within Files folder Quiting");
                Environment.Exit(1);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            DirectoryInfo _folder = new DirectoryInfo("Patched");
            string[] paths = Directory.GetFiles("Original", "*lib*", SearchOption.TopDirectoryOnly);

            foreach (string Path in paths)
            {
                byte[] _file = File.ReadAllBytes(Path);
                using (MemoryStream Stream = new MemoryStream(_file))
                {
                    Console.WriteLine("\n[CSCP] - Clash of SL libg.so information : ");

                    long Offset = FindPosition(Stream, Keys.ClashOfClansAndroid);
                    if (Offset > -1)
                    {
                        Console.WriteLine("[CSCP] - Offset       : " + (Offset - 32) + " [0x" + (Offset - 32).ToString("X") + "]");
                        Console.WriteLine("[CSCP] - Detected Key (HEX)    : " + BitConverter.ToString(_file.ToList().GetRange((int)Offset - 32, 32).ToArray()).Replace("-", ""));
                        Console.WriteLine("[CSCP] - Replace Public Key with COSS key? [Y/N]");
                        Console.Write("\n");

                        if (Console.ReadKey(false).Key == ConsoleKey.Y)
                        {
                            Console.Write("\n");
                            Console.WriteLine("[CSCP] Patching keys ...");
                            List<Byte> Patched = _file.ToList();
                            Patched.RemoveRange((int)Offset - 32, 32);
                            Patched.InsertRange((int)Offset - 32, Keys.CustomKey);

                            if (File.Exists(_folder.FullName + @"/libg.so"))
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("[CSCP] Detected previous existence of libg.so in 'patched' folder, Deleting !");
                                File.Delete(_folder.FullName + "/libg.so");
                            }
                            Console.ForegroundColor = ConsoleColor.Green;
                            using (FileStream FStream = File.Create(_folder.FullName + "/libg.so", Patched.Count, FileOptions.None))
                            {
                                FStream.Write(Patched.ToArray(), 0, Patched.Count);
                            }
                        }
                        else
                        {
                            Console.WriteLine("[CSCP] Patching Cancelled");
                            break;
                        }
                    }


                }
                
            }
        }




        internal static long FindPosition(Stream _Stream, byte[] _Search)
        {
            byte[] Buffer = new byte[_Search.Length];

            using (BufferedStream Stream = new BufferedStream(_Stream, _Search.Length))
            {
                int Index = 0;
                while ((Index = Stream.Read(Buffer, 0, _Search.Length)) == _Search.Length)
                {
                    if (_Search.SequenceEqual(Buffer))
                    {
                        return Stream.Position - _Search.Length;
                    }
                    else
                    {
                        Stream.Position -= _Search.Length - Padding(Buffer, _Search);
                    }
                }
            }
            return -1;
        }


        internal static int Padding(byte[] _Bytes, byte[] _Search)
        {
            int Index = 1;
            {
                while (Index < _Bytes.Length)
                {
                    int Lenght = _Bytes.Length - Index;

                    byte[] Buffer1 = new byte[Lenght];
                    byte[] Buffer2 = new byte[Lenght];

                    Array.Copy(_Bytes, Index, Buffer1, 0, Lenght);
                    Array.Copy(_Search, Buffer2, Lenght);

                    if (Buffer1.SequenceEqual(Buffer2))
                    {
                        return Index;
                    }
                    Index++;
                }
                return Index;
            }
        }
    }
}