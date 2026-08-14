using BepInEx;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace HullBreakerCompany.Hull
{
    public class ConfigManager
    {
        private static string _configPath = Path.Combine(Paths.ConfigPath, "HullBreakerCompany.cfg");
        private static ConfigFile _configFile;

        private static string KeyDesc(string keyId, string fallback) => LocaleManager.GetConfigDescription(keyId) ?? fallback;

        private static T GetConfigValue<T>(string section, string key, T defaultValue, string description = null)
        {
            EnsureConfigExists();
            return _configFile.Bind(section, key, defaultValue, description).Value;
        }

        private static void SetConfigValue()
        {
            EnsureConfigExists();
            Plugin.Language = _configFile.Bind("0 - General", "Language", "en").Value;

            Plugin.EventCount = GetConfigValue("1 - Event Settings", "EventCount", 1, KeyDesc("EventCount", "Maximum number of active events. Less are possible depending on NothingEvent's weight."));
            Plugin.IncreaseEventCountPerDay = GetConfigValue("1 - Event Settings", "IncreaseEventCountPerDay", true, KeyDesc("IncreaseEventCountPerDay", "Whether to increase EventCount by one for each day passed. Resets when a new quota begins.\nSetting this to \"true\" and EventCount to n will roll n events on the first day, n+1 on the second, .."));

            Plugin.EnableEventMessages = GetConfigValue("1.1 - Event Messages", "EnableEventMessages", true, KeyDesc("EnableEventMessages", "Enable chat event messages"));
            Plugin.UseShortChatMessages = GetConfigValue("1.1 - Event Messages", "UseShortChatMessages", false, KeyDesc("UseShortChatMessages", "Use short event message"));
            Plugin.UniqueEventMessages = GetConfigValue("1.1 - Event Messages", "UseUniqueChatMessages", false, KeyDesc("UseUniqueChatMessages", "If true will use non ambigous messages in chat. Every event has a unique message that doesn't change."));
            Plugin.ColoredEventMessages = GetConfigValue("1.1 - Event Messages", "UseColoredChatMessages", false, KeyDesc("UseColoredChatMessages", "If true will use colors for event messages to indicate whether they are 'good' or 'bad' events."));

            Plugin.LandmineScale = GetConfigValue("1.2 - Event Configuration", "LandmineScale", 32, KeyDesc("LandmineScale", "The amount of additional landmines spawned by landmine event (there are more events that spawns mines and thus scale with the value set here)"));
            Plugin.TurretScale = GetConfigValue("1.2 - Event Configuration", "TurretScale", 12, KeyDesc("TurretScale", "The amount of additional turrets spawned by turret event"));
            Plugin.SpikeTrapScale = GetConfigValue("1.2 - Event Configuration", "SpikeTrapScale", 12, KeyDesc("SpikeTrapScale", "The amount of additional spike traps spawned by SpikeTrap event"));

            Plugin.BountyRewardMin = GetConfigValue("1.2 - Event Configuration", "BountyRewardMin", 20, KeyDesc("BountyRewardMin", "Minimum amount of credits rewarded for killing an enemy during EnemyBountyEvent"));
            Plugin.BountyRewardMax = GetConfigValue("1.2 - Event Configuration", "BountyRewardMax", 70, KeyDesc("BountyRewardMax", "Maximum amount of credits rewarded for killing an enemy during EnemyBountyEvent"));
            Plugin.BountyRewardLimit = GetConfigValue("1.2 - Event Configuration", "BountyRewardLimit", 10, KeyDesc("BountyRewardLimit", "Limits the number of times you can get rewarded for kills during EnemyBountyEvent. Final reward will always pay `BountyRewardMax` * 1.5\nSet this to 0 for unlimited number of rewards."));
            Plugin.HullBreakEventCreditsMin = GetConfigValue("1.2 - Event Configuration", "HullBreakEventCreditsMin", 50, KeyDesc("HullBreakEventCreditsMin", "Minimum amount of credits granted by HullBreakEvent"));
            Plugin.HullBreakEventCreditsMax = GetConfigValue("1.2 - Event Configuration", "HullBreakEventCreditsMax", 200, KeyDesc("HullBreakEventCreditsMax", "Maximum amount of credits granted by HullBreakEvent"));

            Plugin.LevelSettings = _configFile.Bind("2 - Level Settings", "LevelSettings", "vanilla",
                new ConfigDescription(KeyDesc("LevelSettings", "Specifies the settings to apply to every level by default\n" +
                "These are baseline settings. Changes by events will always apply on top.\n" +
                "'vanilla': keep level settings as they are\n" +
                "'hullbreaker': hard difficulty. Significantly increases spawn rate inside and outside. Increases max power of inside and outside nemies by 16 and 20 respectively\n" +
                "'custom': uses the values you specified below. Defaults for 'custom' are the values used in 'hullbreaker'\n"),
                new AcceptableValueList<string>(["vanilla", "hullbreaker", "custom"]),
                Array.Empty<object>())).Value;

            Plugin.MaxEnemyPowerCount = GetConfigValue("2.1 - Level Settings", "MaxEnemyPowerCount", 16, KeyDesc("MaxEnemyPowerCount", "Increase max enemy power count by this value"));
            Plugin.MaxOutsideEnemyPowerCount = GetConfigValue("2.1 - Level Settings", "MaxOutsideEnemyPowerCount", 20, KeyDesc("MaxOutsideEnemyPowerCount", "Increase max outside enemy power count by this value (e.g. moutdog or forestgiant)"));
            Plugin.MaxDaytimeEnemyPowerCount = GetConfigValue("2.1 - Level Settings", "MaxDaytimeEnemyPowerCount", 0, KeyDesc("MaxDaytimeEnemyPowerCount", "Increase max daytime enemy power count by this value (e.g. bees)"));
            Plugin.BunkerEnemyScale = GetConfigValue("2.1 - Level Settings", "Spawn curve override", 256, KeyDesc("SpawnCurveOverride", "Change spawn rate for inside enemies. A value of '256' will max out the amount of monsters spawned in each spawn wave starting as soon as you land."));

        }

        public static void RefreshConfig()
        {
            Plugin.Mls.LogInfo("Refreshing config..");
            _configFile = null;
            SetConfigValue();
        }

        public static Dictionary<string, int> GetWeights()
        {
            Plugin.Mls.LogInfo("Refreshing weights..");
            EnsureConfigExists();

            var weights = new Dictionary<string, int>();

            foreach (var hullEvent in EventsManager.EventDictionary)
            {
                string description = LocaleManager.GetParsed("common.eventWeightDescription", ("id", hullEvent.GetID()), ("desc", hullEvent.GetDescription())) ?? $"{hullEvent.GetID()} event: {hullEvent.GetDescription()}";
                weights[hullEvent.GetID()] = _configFile.Bind("3 - Event Weights", hullEvent.GetID(), hullEvent.GetWeight(), description).Value;
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
            sw.WriteLine("[3 - Event Weights]");
            foreach (var hullEvent in EventsManager.EventDictionary)
            {
                sw.WriteLine(hullEvent.GetID() + "=" + hullEvent.GetWeight());
            }
        }
    }
}