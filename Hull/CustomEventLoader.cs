using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using HullBreakerCompany.Events;

namespace HullBreakerCompany.Hull;

public class CustomEventLoader
{
    public static void LoadCustomEvents()
    {
        var customEventData = LoadEventDataFromCfgFiles();
        if (customEventData.Count == 0) return;

        foreach (var hullEventData in customEventData)
        {
            // Building custom event from custom config event data
            CustomEvent customEvent = new CustomEvent();
            customEvent.SetID(hullEventData["EventID"]);
            customEvent.SetWeight(int.Parse(hullEventData["EventWeight"]));
            foreach (var msg in ParseMessages(hullEventData["InGameMessage"])) {
                customEvent.AddMessage(msg);
            }
            foreach (var msg in ParseMessages(hullEventData["InGameShortMessage"])) {
                customEvent.AddShortMessage(msg);
            }
            if (hullEventData.ContainsKey("SpawnableEnemies")) {
                //customEvent.EnemySpawnList = new HashSet<string>(hullEvent["SpawnableEnemies"].Split(',')).ToList();
                Plugin.Mls.LogDebug($"SpawnableEnemies defined. Parsing..");
                customEvent.EnemySpawnList = ParseEnemies(hullEventData["SpawnableEnemies"]);
            }
            if (hullEventData.ContainsKey("SpawnableOutsideEnemies")) {
                Plugin.Mls.LogDebug($"SpawnableOutsideEnemies defined. Parsing..");
                customEvent.OutsideEnemySpawnList = ParseEnemies(hullEventData["SpawnableOutsideEnemies"]);
            }            
            if (hullEventData.ContainsKey("SpawnableDaytimeEnemies")) {
                Plugin.Mls.LogDebug($"SpawnableDaytimeEnemies defined. Parsing..");
                customEvent.DaytimeEnemySpawnList = ParseEnemies(hullEventData["SpawnableDaytimeEnemies"]);
            }
            if (hullEventData.ContainsKey("SpawnableScrap")) {
                Plugin.Mls.LogDebug($"SpawnableScrap defined. Parsing..");
                customEvent.ScrapSpawnList = ParseScrap(hullEventData["SpawnableScrap"]);
            }
            if (hullEventData.ContainsKey("GlobalPowerIncrease")) {
                Plugin.Mls.LogDebug($"GlobalPowerIncrease defined. Parsing..");
                customEvent.addPower = int.Parse(hullEventData["GlobalPowerIncrease"]);
            }
            if (hullEventData.ContainsKey("GlobalOutsidePowerIncrease")) {
                Plugin.Mls.LogDebug($"GlobalOutsidePowerIncrease defined. Parsing..");
                customEvent.addOutsidePower = int.Parse(hullEventData["GlobalOutsidePowerIncrease"]);
            }            
            if (hullEventData.ContainsKey("GlobalDaytimePowerIncrease")) {
                Plugin.Mls.LogDebug($"GlobalDaytimePowerIncrease defined. Parsing..");
                customEvent.addDaytimePower = int.Parse(hullEventData["GlobalDaytimePowerIncrease"]);
            }
            if (hullEventData.ContainsKey("GlobalInsideSpawnRateOverride")) {
                Plugin.Mls.LogDebug($"GlobalInsideSpawnRateOverride defined. Parsing..");
                customEvent.overrideSpawnRate = int.Parse(hullEventData["GlobalInsideSpawnRateOverride"]);
            }
            if (hullEventData.ContainsKey("GlobalOutsideSpawnRateOverride")) {
                Plugin.Mls.LogDebug($"GlobalOutsideSpawnRateOverride defined. Parsing..");
                customEvent.overrideOutsideSpawnRate = int.Parse(hullEventData["GlobalOutsideSpawnRateOverride"]);
            }            
            if (hullEventData.ContainsKey("GlobalDaytimeSpawnRateOverride")) {
                Plugin.Mls.LogDebug($"GlobalDaytimeSpawnRateOverride defined. Parsing..");
                customEvent.overrideDaytimeSpawnRate = int.Parse(hullEventData["GlobalDaytimeSpawnRateOverride"]);
            }

            // Register and enable the custom event
            AddEvent(customEvent);
        }
    }
    
    private static List<Dictionary<string, string>> LoadEventDataFromCfgFiles()
    {
        List<string> directoryPaths = GetCustomEventDirectoryPaths();
        List<string> cfgFiles = new List<string>();
        foreach (string directoryPath in directoryPaths) {
            cfgFiles.AddRange(Directory.GetFiles(directoryPath, "*.cfg"));
        }

        if (cfgFiles.Count == 0) return [];

        List<Dictionary<string, string>> allEventData = new List<Dictionary<string, string>>();

        foreach (string cfgFile in cfgFiles) {
            var eventData = ParseConfigFile(cfgFile);
            if (eventData == null) continue;
            allEventData.Add(eventData);
            Plugin.Mls.LogInfo($"Loading custom event: {eventData["EventID"]} ({cfgFile})");
        }
        return allEventData;
    }
    private static List<string> GetCustomEventDirectoryPaths() {
        try {
            return (from dir in Directory.GetDirectories(Paths.BepInExRootPath, "*", SearchOption.AllDirectories)
                    where Path.GetFileName(dir).Equals("HullEvents", StringComparison.OrdinalIgnoreCase)
                    select dir).ToList();
        } catch (Exception ex) {
            Plugin.Mls.LogInfo("Custom event folder 'HullEvents' not found. Skipping custom event loading.");
            return new List<string>();
        }
    }
    private static Dictionary<string, string> ParseConfigFile(string file) {
        string[] lines = File.ReadAllLines(file);
        Dictionary<string, string> eventData = new Dictionary<string, string>();

        foreach (string line in lines) {
            if (line.StartsWith("[DISABLED]")) {
                Plugin.Mls.LogInfo($"Custom Event configuration file ({file}) is disabled. Skipping..");
                return null;
            }
            if (line.StartsWith("[") || line.StartsWith("#") || string.IsNullOrWhiteSpace(line)) continue;

            string[] keyValue = line.Split('=');
            if (keyValue.Length == 2) {
                string key = keyValue[0].Trim();
                string value = keyValue[1].Trim();
                eventData.Add(key, value);
            }
        }

        return eventData;
    }
    private static List<string> ParseMessages(string configValue) {
        List<string> result = new List<string>();
        string[] messages = configValue.Split(';');
        if (messages.Length < 1) {
            Plugin.Mls.LogError($"Error while parsing custom event messages: no message defined");
            return result;
        }
        foreach (string message in messages) {
            if (string.IsNullOrWhiteSpace(message)) continue;
            result.Add(message.Trim());
        }
        return result;
    }
    private static Dictionary<string, List<int>> ParseEnemies(string configValue) {
        Dictionary<string, List<int>> enemies = new Dictionary<string, List<int>>();
        string[] enemyPairs = configValue.Split(',');
        foreach (string pair in enemyPairs) {
            string[] values = pair.Split(":");
            if (values.Length <2) {
                Plugin.Mls.LogWarning($"Invalid enemies format: {pair}");
                continue;
            }
            string enemy = values[0].Trim();
            int rarity = int.Parse(values[1].Trim());
            int maxCount = values.Length > 2 ? int.Parse(values[2].Trim()) : -1;
            int power = values.Length > 3 ? int.Parse(values[3].Trim()) : -1;
            Plugin.Mls.LogDebug($"Parsed enemy: (name = {enemy}, rarity = {rarity}, maxCount = {maxCount}, Power = {power})");
            enemies.Add(enemy, [rarity, maxCount, power]);
        }
        return enemies;
    }
    private static Dictionary<string, int> ParseScrap(string configValue) {
        Dictionary<string, int> scrap = new Dictionary<string, int>();
        string[] enemyPairs = configValue.Split(',');
        foreach (string pair in enemyPairs) {
            string[] values = pair.Split(":");
            if (values.Length != 2) {
                Plugin.Mls.LogWarning($"Invalid scrap format: {pair}");
                continue;
            }
            string item = values[0].Trim();
            int rarity = int.Parse(values[1].Trim());
            Plugin.Mls.LogDebug($"Parsed scrap item: (name = {item}, rarity = {rarity})");
            scrap.Add(item, rarity);
        }
        return scrap;
    }

    private static void AddEvent(HullEvent newEvent)
    {
        if (EventsManager.EventDictionary.Any(e => e.GetID() == newEvent.GetID())) {
            Plugin.Mls.LogWarning("Custom event " + newEvent.GetID() + " can't be added because an event with the same ID already exists! Check for duplicate config files.");
        } else {
            Plugin.Mls.LogInfo("Adding " + newEvent.GetID() + " to event dictionary");
            EventsManager.EventDictionary.Add(newEvent);
        }
    }
    
    public static void DebugLoadCustomEvents()
    {
        Plugin.Mls.LogInfo($"Listing all loaded custom events..");
        foreach (var hullEvent in EventsManager.EventDictionary)
        {
            if (hullEvent is CustomEvent customEvent)
            {
                Plugin.Mls.LogInfo($"Event ID: {customEvent.GetID()}");
                Plugin.Mls.LogInfo($"Messages: {customEvent.GetReadableMessages()}");
                Plugin.Mls.LogInfo($"Short messages: {customEvent.GetReadableShortMessage()}");
                Plugin.Mls.LogInfo($"Enemies: {string.Join(";  ", customEvent.EnemySpawnList.Select(enemy => "[" + enemy.Key + ": Rarity percent = " + enemy.Value[0] + ", Max Count = " + enemy.Value[1] + ", Power Level = " + enemy.Value[2] + "]"))}");
                Plugin.Mls.LogInfo($"Outside Enemies: {string.Join("; ", customEvent.OutsideEnemySpawnList.Select(enemy => "[" + enemy.Key + ": Rarity percent = " + enemy.Value[0] + ", Max Count = " + enemy.Value[1] + ", Power Level = " + enemy.Value[2] + "]"))}");
                Plugin.Mls.LogInfo($"Daytime Enemies: {string.Join("; ", customEvent.DaytimeEnemySpawnList.Select(enemy => "[" + enemy.Key + ": Rarity percent = " + enemy.Value[0] + ", Max Count = " + enemy.Value[1] + ", Power Level = " + enemy.Value[2] + "]"))}");
                Plugin.Mls.LogInfo($"Scrap: {string.Join(", ", customEvent.ScrapSpawnList)}");
                Plugin.Mls.LogInfo($"Global power increase: {customEvent.addPower}");
                Plugin.Mls.LogInfo($"Global outside power increase: {customEvent.addOutsidePower}");
                Plugin.Mls.LogInfo($"Global daytime power increase: {customEvent.addDaytimePower}");
                Plugin.Mls.LogInfo($"Global spawn chance override: {customEvent.overrideSpawnRate}");
                Plugin.Mls.LogInfo($"Global outside spawn chance override: {customEvent.overrideOutsideSpawnRate}");
                Plugin.Mls.LogInfo($"Global daytime spawn chance override: {customEvent.overrideDaytimeSpawnRate}");
            }
        }
    }
}