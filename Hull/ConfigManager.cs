using BepInEx.Configuration;
using System.Collections.Generic;
using System.IO;
using BepInEx;

namespace HullBreakerCompany.Hull
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
            Plugin.LandmineScale = GetConfigValue("LandmineScale", 24, "Should change the amount of additional landmines spawned by landmine event");
            Plugin.TurretScale = GetConfigValue("TurretScale", 8, "Should change the amount of additional turrets spawned by turret event");
            Plugin.EnableEventMessages = GetConfigValue("EnableEventMessages", true, "Enable chat event messages");
            Plugin.UseShortChatMessages = GetConfigValue("UseShortChatMessages", false, "Use short event message");
            
            Plugin.UseHullBreakerLevelSettings = GetConfigValue("UseHullBreakerLevelSettings", true, "Use HullBreaker level settings. Increases maximum enemy count as well as spawn rate inside and outside. Set to false if you want to use default or custom settings. See below.");
            Plugin.UseVanillaGameSettings = GetConfigValue("UseVanillaLevelSettings", false, "Use vanilla level settings. If false, you can set your own custom settings below");
            
            Plugin.MaxEnemyPowerCount = GetConfigValue("MaxEnemyPowerCount", 10, "Max enemy power count");
            Plugin.MaxOutsideEnemyPowerCount = GetConfigValue("MaxOutsideEnemyPowerCount", 10, "Max outside enemy power count");
            Plugin.MaxDaytimeEnemyPowerCount = GetConfigValue("MaxDaytimeEnemyPowerCount", 20, "Max daytime enemy power count");
            Plugin.BunkerEnemyScale = GetConfigValue("BunkerEnemyScale", 256, "Should change global bunker enemy spawn rate, not sure if its work");
            
            Plugin.EventCount = GetConfigValue("EventCount", 3, "Total number of events that are randomly selected. Can roll NothingEvent resulting in less active events");
            Plugin.IncreaseEventCountPerDay = GetConfigValue("IncreaseEventCountPerDay", false, "The number of events rolled will increase by one every day. Resets on visiting the company building or loading a save");

            Plugin.BountyRewardMin = GetConfigValue("BountyRewardMin", 40, "Minimum amount of credits rewarded for killing an enemy during EnemyBountyEvent");
            Plugin.BountyRewardMax = GetConfigValue("BountyRewardMax", 80, "Maximum amount of credits rewarded for killing an enemy during EnemyBountyEvent");
            Plugin.HullBreakEventCreditsMin = GetConfigValue("HullBreakEventCreditsMin", 50, "Minimum amount of credits granted by HullBreakEvent");
            Plugin.HullBreakEventCreditsMax = GetConfigValue("HullBreakEventCreditsMax", 200, "Maximum amount of credits granted by HullBreakEvent");
        }
        
        public static Dictionary<string, int> GetWeights()
        {
            EnsureConfigExists();

            var weights = new Dictionary<string, int>();

            foreach (var hullEvent in Plugin.EventDictionary)
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
            foreach (var hullEvent in Plugin.EventDictionary)
            {
                sw.WriteLine(hullEvent.ID() + "=" + hullEvent.GetWeight());
            }
        }
    }
}