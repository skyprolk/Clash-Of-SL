using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CSS.Files
{
    internal class FingerPrint
    {
        public FingerPrint()
        {

            files           = new List<GameFile>();
            string fpstring = null;

            if (File.Exists(@"Gamefiles/fingerprint.json"))
            {
                using (StreamReader sr = new StreamReader(@"Gamefiles/fingerprint.json"))
                    fpstring = sr.ReadToEnd();
                LoadFromJson(fpstring);
            }
            else
                Console.WriteLine(
                    "[CSS]    LoadFingerPrint: error! tried to load FingerPrint without file, run gen_patch first");
        }

        public List<GameFile> files { get; set; }
        public string sha { get; set; }
        public string version { get; set; }

        public void LoadFromJson(string jsonString)
        {
            JObject jsonObject    = JObject.Parse(jsonString);
            JArray jsonFilesArray = (JArray)jsonObject["files"];
            foreach (JObject jsonFile in jsonFilesArray)
            {
                GameFile gf = new GameFile();
                gf.Load(jsonFile);
                files.Add(gf);
            }
            sha = jsonObject["sha"].ToObject<string>();
            version = jsonObject["version"].ToObject<string>();
        }

        public string SaveToJson()
        {
            JObject jsonData       = new JObject();
            JArray jsonFilesArray  = new JArray();
            foreach (GameFile file in files)
            {
                JObject jsonObject = new JObject();
                file.SaveToJson(jsonObject);
                jsonFilesArray.Add(jsonObject);
            }
            jsonData.Add("files", jsonFilesArray);
            jsonData.Add("sha", sha);
            jsonData.Add("version", version);

            return JsonConvert.SerializeObject(jsonData).Replace("/", @"\/");
        }
    }

    internal class GameFile
    {
        public string file { get; set; }
        public string sha { get; set; }

        public void Load(JObject jsonObject)
        {
            sha  = jsonObject["sha"].ToObject<string>();
            file = jsonObject["file"].ToObject<string>();
        }

        public string SaveToJson(JObject fingerPrint)
        {
            fingerPrint.Add("sha", sha);
            fingerPrint.Add("file", file);

            return JsonConvert.SerializeObject(fingerPrint);
        }
    }
}