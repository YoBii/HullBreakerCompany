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
            
            Plugin.BunkerEnemyScale = GetConfigValue("BunkerEnemyScale", 256, "Should change global bunker enemy spawn rate, not sure if its work");
            Plugin.LandMineTurretScale = GetConfigValue("LandMineTurretScale", 64, "Should change amount of Landmines & Turrets when these events are active: (Landmine & Turret)");
            Plugin.UseShortChatMessages = GetConfigValue("UseShortChatMessages", false, "Use short event message (one/two words), can add surprise effect & difficulty");
            
            Plugin.UseHullBreakerLevelSettings = GetConfigValue("useHullBreakerLevelSettings", true, "Use HullBreaker level settings, if false, use default level settings");
            Plugin.UseDefaultGameSettings = GetConfigValue("UseDefaultLevelSettings", false, "Use default level settings, if false, you can change on one's own");
            
            Plugin.ChangeQuotaValue = GetConfigValue("ChangeQuota", true, "Change quota");
            Plugin.QuotaIncrease = GetConfigValue("IncreasedQuota", 256, "Increased quota");
            
            Plugin.MaxEnemyPowerCount = GetConfigValue("maxEnemyPowerCount", 2000, "Max enemy power count");
            Plugin.MaxOutsideEnemyPowerCount = GetConfigValue("maxOutsideEnemyPowerCount", 20, "Max outside enemy power count");
            Plugin.MaxDaytimeEnemyPowerCount = GetConfigValue("maxDaytimeEnemyPowerCount", 200, "Max daytime enemy power count");
            Plugin.MinScrap = GetConfigValue("minScrap", 0, "Min scrap");
            Plugin.MaxScrap = GetConfigValue("maxScrap", 30, "Max scrap");
            Plugin.MinTotalScrapValue = GetConfigValue("minTotalScrapValue", 200, "Min total scrap value");
            Plugin.MaxTotalScrapValue = GetConfigValue("maxTotalScrapValue", 800, "Max total scrap value");
            
            Plugin.IncreaseEventCountPerDay = GetConfigValue("IncreaseEventCountPerDay", false, "The number of events will increase every day. Visit the company building to reset");
            Plugin.EventCount = GetConfigValue("EventCount", 3, "The number of events that will be active at the same time");

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