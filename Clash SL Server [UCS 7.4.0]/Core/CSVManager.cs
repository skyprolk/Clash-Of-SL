using Newtonsoft.Json.Linq;
using SevenZip.SDK;
using SevenZip.SDK.Compress.LZMA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CSS.Files;
using CSS.Files.CSV;
using CSS.Files.Logic;
using static CSS.Core.Logger;

namespace CSS.Core
{
    internal class CSVManager
    {
        private static readonly List<Tuple<string, string, int>> _gameFiles = new List<Tuple<string, string, int>>();

        private static DataTables _DataTables;

        public CSVManager()
        {
            //DownloadLatestCSVFiles();

            try
            {
                _gameFiles.Add(new Tuple<string, string, int>("Buildings", @"Gamefiles/logic/buildings.csv", 0));
                _gameFiles.Add(new Tuple<string, string, int>("Resources", @"Gamefiles/logic/resources.csv", 2));
                _gameFiles.Add(new Tuple<string, string, int>("Characters", @"Gamefiles/logic/characters.csv", 3));
                _gameFiles.Add(new Tuple<string, string, int>("Obstacles", @"Gamefiles/logic/obstacles.csv", 7));
                _gameFiles.Add(new Tuple<string, string, int>("Experience Levels", @"Gamefiles/logic/experience_levels.csv", 10));
                _gameFiles.Add(new Tuple<string, string, int>("Traps", @"Gamefiles/logic/traps.csv", 11));
                _gameFiles.Add(new Tuple<string, string, int>("Leagues", @"Gamefiles/logic/leagues.csv", 12));
                _gameFiles.Add(new Tuple<string, string, int>("Globals", @"Gamefiles/logic/globals.csv", 13));
                _gameFiles.Add(new Tuple<string, string, int>("Townhall Levels", @"Gamefiles/logic/townhall_levels.csv", 14));
                _gameFiles.Add(new Tuple<string, string, int>("NPCs", @"Gamefiles/logic/npcs.csv", 16));
                _gameFiles.Add(new Tuple<string, string, int>("Decos", @"Gamefiles/logic/decos.csv", 17));
                _gameFiles.Add(new Tuple<string, string, int>("Shields", @"Gamefiles/logic/shields.csv", 19));
                _gameFiles.Add(new Tuple<string, string, int>("Achievements", @"Gamefiles/logic/achievements.csv", 22));
                _gameFiles.Add(new Tuple<string, string, int>("Spells", @"Gamefiles/logic/spells.csv", 25));
                _gameFiles.Add(new Tuple<string, string, int>("Heroes", @"Gamefiles/logic/heroes.csv", 27));
                /*
                gameFiles.Add(new Tuple<string, string, int>("Alliance Badge Layers", @"Gamefiles/logic/alliance_badge_layers.csv", 30));
                gameFiles.Add(new Tuple<string, string, int>("Alliance Badges", @"Gamefiles/logic/alliance_badges.csv", 31));
                gameFiles.Add(new Tuple<string, string, int>("Alliance Levels", @"Gamefiles/logic/alliance_levels.csv", 32));
                gameFiles.Add(new Tuple<string, string, int>("Alliance Portal", @"Gamefiles/logic/alliance_portal.csv", 33));
                gameFiles.Add(new Tuple<string, string, int>("Buildings Classes", @"Gamefiles/logic/building_classes.csv", 34));
                gameFiles.Add(new Tuple<string, string, int>("Effects", @"Gamefiles/logic/effects.csv", 35));
                gameFiles.Add(new Tuple<string, string, int>("Locales", @"Gamefiles/logic/locales.csv", 36));
                gameFiles.Add(new Tuple<string, string, int>("Missions", @"Gamefiles/logic/missions.csv", 37));
                gameFiles.Add(new Tuple<string, string, int>("Projectiles", @"Gamefiles/logic/projectiles.csv", 38));
                gameFiles.Add(new Tuple<string, string, int>("Regions", @"Gamefiles/logic/regions.csv", 39));
                gameFiles.Add(new Tuple<string, string, int>("Variables", @"Gamefiles/logic/variables.csv", 40)); 
                gameFiles.Add(new Tuple<string, string, int>("War", @"Gamefiles/logic/war.csv", 28));
                */
                _DataTables = new DataTables();
                int Count = 0;
                foreach (Tuple<string, string, int> _File in _gameFiles)
                {
                    _DataTables.InitDataTable(new CSVTable(_File.Item2), _File.Item3);
                    Count++;
                }
                Say("CSV Tables  have been succesfully loaded. (" + Count + ")");
            }
            catch (Exception)
            {
                Say();
                Error("Error loading Gamefiles. Looks like you have :");
                Error("     -> Edited the Files Wrong");
                Error("     -> Made mistakes by deleting values");
                Error("     -> Entered too High or Low value");
                Error("     -> Please check to these errors");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        public static List<Tuple<string, string, int>> Gamefiles => _gameFiles;

        public static DataTables DataTables => _DataTables;

        public static void DownloadLatestCSVFiles()
        {
            try
            {
                string _DownloadString = "http://b46f744d64acd2191eda-3720c0374d47e9a0dd52be4d281c260f.r11.cf2.rackcdn.com/" + ObjectManager.FingerPrint.sha + "/";
                bool DownloadOnlyCSV;
                back:
                Say("Do you want to download only CSV Files or all (Includes SC...)? Y = Yes, N = No.");
                Say("", true);
                string answer = Console.ReadLine().ToUpper();

                if (answer == "Y")
                {
                    DownloadOnlyCSV = true;
                }
                else if(answer == "N")
                {
                    DownloadOnlyCSV = false;
                }
                else
                {
                    goto back;
                }

                WebClient _WC = new WebClient();
                string _FingerPrint = _WC.DownloadString(new Uri(_DownloadString + "fingerprint.json"));

                JObject jsonObject = JObject.Parse(_FingerPrint);
                JArray jsonFilesArray = (JArray)jsonObject["files"];

                foreach (JObject _File in jsonFilesArray)
                {
                    string _CSV = _File["file"].ToObject<string>();
                    string[] _Folder = _CSV.Split('/');

                    if (DownloadOnlyCSV)
                    {
                        if (_Folder[0] == "csv")
                        {
                            DownloadFile(_DownloadString, _CSV, _Folder[0], _Folder[1], false, true);
                        }
                        else if (_Folder[0] == "logic")
                        {
                            DownloadFile(_DownloadString, _CSV, _Folder[0], _Folder[1], false, true);
                        }
                    }
                    else
                    {
                        if (_Folder[0] != "sfx")
                        {
                            DownloadFile(_DownloadString, _CSV, _Folder[0], _Folder[1]);
                        }
                    }
                }
                Say("All Files has been succesfully downloaded!");
            }
            catch (Exception)
            { 
            }
        } 

        public static void DownloadFile(string _Link, string _Sublink, string _Folder, string _FileName, bool HasSubFolder = false, bool IsCSV = false)
        {
            try
            {
                string _FileLink = _Link + _Sublink;

                if (HasSubFolder)
                {
                    // Todo
                }
                else
                {
                    if (!Directory.Exists($"Gamefiles/update/compressed/{_Folder}"))
                    {
                        Directory.CreateDirectory($"Gamefiles/update/compressed/{_Folder}");
                    }

                    WebClient _WC = new WebClient();
                    Say($"Downloading '{_FileName}'... ", true);
                    _WC.DownloadFile(new Uri(_FileLink), $"Gamefiles/update/compressed/{ _Folder}/{_FileName}");

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("DONE");
                    Console.ResetColor();

                    Decoder _Decoder = new Decoder();
                    using (FileStream _FS = new FileStream($"Gamefiles/update/compressed/{ _Folder}/{_FileName}", FileMode.Open))
                    {
                        if (!Directory.Exists($"Gamefiles/update/decompressed/{_Folder}"))
                        {
                            Directory.CreateDirectory($"Gamefiles/update/decompressed/{_Folder}");
                        }

                        if (IsCSV)
                        {
                            Say($"Decompressing '{_FileName}'... ", true);
                            using (FileStream _Stream = new FileStream($"Gamefiles/update/decompressed/{ _Folder}/{_FileName}", FileMode.Create))
                            {
                                byte[] numArray = new byte[5];
                                _FS.Read(numArray, 0, 5);
                                byte[] buffer = new byte[4];
                                _FS.Read(buffer, 0, 4);
                                int int32 = BitConverter.ToInt32(buffer, 0);
                                _Decoder.SetDecoderProperties(numArray);
                                _Decoder.Code((Stream)_FS, (Stream)_Stream, _FS.Length, (long)int32, (ICodeProgress)null);
                                _Stream.Flush();
                                _Stream.Close();
                            }
                            _FS.Close();

                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("DONE");
                            Console.ResetColor();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
                                                                                                       