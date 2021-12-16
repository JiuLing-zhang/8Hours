using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _8Hours.Models;

namespace _8Hours.Configs
{
    internal class GlobalConfig
    {
        private static readonly string ConfigPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\config.json";
        private static AppConfig? _app;
        public static AppConfig App
        {
            get
            {
                if (_app == null)
                {
                    string configPath = ConfigPath;
                    if (!System.IO.File.Exists(configPath))
                    {
                        throw new Exception("Config file is missing.");
                    }

                    try
                    {
                        string json = System.IO.File.ReadAllText(configPath);
                        _app = System.Text.Json.JsonSerializer.Deserialize<AppConfig>(json);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Config file format error.");
                    }
                }
                return _app ?? throw new ArgumentNullException(nameof(App));
            }
            set => _app = value;
        }

        public static void SaveConfig()
        {
            try
            {
                string json = System.Text.Json.JsonSerializer.Serialize(App);
                System.IO.File.WriteAllText(ConfigPath, json);
            }
            catch (Exception ex)
            {
                throw new Exception("Config file save failed.");
            }
        }
    }
}
