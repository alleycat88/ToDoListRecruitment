using System;
using System.IO;
using Newtonsoft.Json;

namespace ToDoListRecruitment.Utility
{
    public class SettingsController
    {
        private static string settingsFilePath = "./Values/Settings.json";

        public dynamic LoadSettings()
        {
            using(StreamReader r = File.OpenText(settingsFilePath))
            {
                string settingsJson = r.ReadToEnd();
                dynamic settings = JsonConvert.DeserializeObject(settingsJson);
                return settings;
            }
        }
    }
}