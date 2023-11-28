using BepInEx.Configuration;
using System.Collections.Generic;
using System.IO;
using BepInEx;

namespace HullBreakerCompany.hull
{
    public class ConfigManager
    {
        private static string _configPath = Path.Combine(Paths.ConfigPath, "HullBreakerCompany.cfg");
        private static ConfigFile _configFile;
        public static int GetEventCount()
        {
            EnsureConfigExists();
            return _configFile.Bind("Settings", "EventCount", 3).Value;
        }
        public static bool GetIncreaseEventCountPerDay()
        {
            EnsureConfigExists();
            return _configFile.Bind("Settings", "IncreaseEventCountPerDay", false, "The number of events will increase every day. Visit the company building to reset").Value;
        }
        public static float GetBunkerEnemyScale()
        {
            EnsureConfigExists();
            return _configFile.Bind("Settings", "BunkerEnemyScale", 256, "Should change global bunker enemy spawn rate, not sure if its work").Value;
        }
        public static float GetLandMineTurretScale()
        {
            EnsureConfigExists();
            return _configFile.Bind("Settings", "LandMineTurretScale", 64, "Should change amount of Landmines & Turrets when these events are active: (Landmine & Turret)").Value;
        }
        public static bool GetUseShortChatMessages()
        {
            EnsureConfigExists();
            return _configFile.Bind("Settings", "UseShortChatMessages", false, "Use short event message (one/two words), can add surprise effect & difficulty").Value;
        }
        public static Dictionary<string, int> GetWeights()
        {
            EnsureConfigExists();

            var weights = new Dictionary<string, int>();

            foreach (var hullEvent in Plugin.eventDictionary)
            {
                weights[hullEvent.ID()] = _configFile.Bind("Weights", hullEvent.ID(), hullEvent.GetWeight(), hullEvent.GetDescription()).Value;
            }

            return weights;
        }
        private static void EnsureConfigExists()
        {
            if (_configFile == null)
            {
                if (!File.Exists(_configPath))
                {
                    CreateDefaultConfigFile();
                }
                _configFile = new ConfigFile(_configPath, true);
            }
        }

        private static void CreateDefaultConfigFile()
        {
            using StreamWriter sw = File.CreateText(_configPath);
            sw.WriteLine("[Weights]");
            foreach (var hullEvent in Plugin.eventDictionary)
            {
                sw.WriteLine(hullEvent.ID() + "=" + hullEvent.GetWeight());
            }
        }
    }
}