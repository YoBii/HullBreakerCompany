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
        public static T GetConfigValue<T>(string key, T defaultValue, string description = null)
        {
            EnsureConfigExists();
            return _configFile.Bind("Settings", key, defaultValue, description).Value;
        }
        
        public static void SetConfigValue() {
            
            Plugin.BunkerEnemyScale = ConfigManager.GetConfigValue("BunkerEnemyScale", 256, "Should change global bunker enemy spawn rate, not sure if its work");
            Plugin.LandMineTurretScale = ConfigManager.GetConfigValue("LandMineTurretScale", 64, "Should change amount of Landmines & Turrets when these events are active: (Landmine & Turret)");
            Plugin.UseShortChatMessages = ConfigManager.GetConfigValue("UseShortChatMessages", false, "Use short event message (one/two words), can add surprise effect & difficulty");
            
            Plugin.useHullBreakerLevelSettings = ConfigManager.GetConfigValue("useHullBreakerLevelSettings", false, "Use HullBreaker level settings, if false, use default level settings");
            
            Plugin.maxEnemyPowerCount = ConfigManager.GetConfigValue("maxEnemyPowerCount", 2000, "Max enemy power count");
            Plugin.maxOutsideEnemyPowerCount = ConfigManager.GetConfigValue("maxOutsideEnemyPowerCount", 20, "Max outside enemy power count");
            Plugin.maxDaytimeEnemyPowerCount = ConfigManager.GetConfigValue("maxDaytimeEnemyPowerCount", 200, "Max daytime enemy power count");
            Plugin.maxScrap = ConfigManager.GetConfigValue("maxScrap", 30, "Max scrap");
            Plugin.maxTotalScrapValue = ConfigManager.GetConfigValue("maxTotalScrapValue", 800, "Max total scrap value");
            
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