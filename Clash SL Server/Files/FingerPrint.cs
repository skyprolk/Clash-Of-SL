/*
 * Program : Clash Of SL Server
 * Description : A C# Writted 'Clash of SL' Server Emulator !
 *
 * Authors:  Sky Tharusha <Founder at Sky Production>,
 *           And the Official DARK Developement Team
 *
 * Copyright (c) 2021  Sky Production
 * All Rights Reserved.
 */

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CSS.Files
{
    internal class FingerPrint
    {
        #region Public Constructors

        public FingerPrint(string filePath)
        {
            files = new List<GameFile>();
            string fpstring = null;

            if (File.Exists(filePath))
            {
                using (var sr = new StreamReader(filePath))
                    fpstring = sr.ReadToEnd();
                LoadFromJson(fpstring);
                Console.WriteLine("[CSS]    ObjectManager: fingerprint loaded");
            }
            else
                Console.WriteLine(
                    "[CSS]    LoadFingerPrint: error! tried to load FingerPrint without file, run gen_patch first");
        }

        #endregion Public Constructors

        #region Public Properties

        public List<GameFile> files { get; set; }
        public string sha { get; set; }
        public string version { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void LoadFromJson(string jsonString)
        {
            var jsonObject = JObject.Parse(jsonString);

            var jsonFilesArray = (JArray) jsonObject["files"];
            foreach (JObject jsonFile in jsonFilesArray)
            {
                var gf = new GameFile();
                gf.Load(jsonFile);
                files.Add(gf);
            }
            sha = jsonObject["sha"].ToObject<string>();
            version = jsonObject["version"].ToObject<string>();
        }

        public string SaveToJson()
        {
            var jsonData = new JObject();

            var jsonFilesArray = new JArray();
            foreach (var file in files)
            {
                var jsonObject = new JObject();
                file.SaveToJson(jsonObject);
                jsonFilesArray.Add(jsonObject);
            }
            jsonData.Add("files", jsonFilesArray);
            jsonData.Add("sha", sha);
            jsonData.Add("version", version);

            return JsonConvert.SerializeObject(jsonData).Replace("/", @"\/");
        }

        #endregion Public Methods
    }

    internal class GameFile
    {
        #region Public Properties

        public string file { get; set; }
        public string sha { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void Load(JObject jsonObject)
        {
            sha = jsonObject["sha"].ToObject<string>();
            file = jsonObject["file"].ToObject<string>();
        }

        public string SaveToJson(JObject fingerPrint)
        {
            fingerPrint.Add("sha", sha);
            fingerPrint.Add("file", file);

            return JsonConvert.SerializeObject(fingerPrint);
        }

        #endregion Public Methods
    }
}