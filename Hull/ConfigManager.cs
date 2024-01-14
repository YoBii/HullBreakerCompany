﻿using BepInEx.Configuration;
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
            Plugin.LandmineScale = GetConfigValue("LandmineScale", 24, "Should change amount of landmines when the landmine event ist active");
            Plugin.TurretScale = GetConfigValue("TurretScale", 8, "Should change amount of turrets when the turret event ist active");
            Plugin.UseShortChatMessages = GetConfigValue("UseShortChatMessages", false, "Use short event message (one/two words), can add surprise effect & difficulty");
            Plugin.EnableEventMessages = GetConfigValue("EnableEventMessages", true, "Enable chat event messages");
            
            Plugin.UseHullBreakerLevelSettings = GetConfigValue("UseHullBreakerLevelSettings", true, "Use HullBreaker level settings, if false, use default level settings");
            Plugin.UseDefaultGameSettings = GetConfigValue("UseDefaultLevelSettings", false, "Use default level settings, if false, you can change on one's own");
            
            Plugin.MaxEnemyPowerCount = GetConfigValue("MaxEnemyPowerCount", 10, "Max enemy power count");
            Plugin.MaxOutsideEnemyPowerCount = GetConfigValue("MaxOutsideEnemyPowerCount", 10, "Max outside enemy power count");
            Plugin.MaxDaytimeEnemyPowerCount = GetConfigValue("MaxDaytimeEnemyPowerCount", 20, "Max daytime enemy power count");
            Plugin.BunkerEnemyScale = GetConfigValue("BunkerEnemyScale", 256, "Should change global bunker enemy spawn rate, not sure if its work");
            
            Plugin.IncreaseEventCountPerDay = GetConfigValue("IncreaseEventCountPerDay", false, "The number of events will increase every day. Visit the company building to reset");
            Plugin.EventCount = GetConfigValue("EventCount", 3, "The number of events that will be active at the same time");

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