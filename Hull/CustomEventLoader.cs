﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using HullBreakerCompany.Events;
using Mono.Cecil;
using UnityEngine.Analytics;
using UnityEngine.UIElements;

namespace HullBreakerCompany.Hull;

public class CustomEventLoader
{
    public static void LoadCustomEvents()
    {
        var customEvents = LoadEventDataFromCfgFiles();
        if (customEvents.Count == 0) return;

        foreach (var hullEvent in customEvents)
        {
            // Building custom event from custom config event data
            CustomEvent customEvent = new CustomEvent();
            customEvent.SetID(hullEvent["EventID"]);
            customEvent.SetWeight(int.Parse(hullEvent["EventWeight"]));
            customEvent.SetMessage(hullEvent["InGameMessage"]);
            customEvent.SetShortMessage(hullEvent["InGameShortMessage"]);

            if (hullEvent.ContainsKey("SpawnableEnemies")) {
                //customEvent.EnemySpawnList = new HashSet<string>(hullEvent["SpawnableEnemies"].Split(',')).ToList();
                Plugin.Mls.LogInfo($"SpawnableEnemies defined. Parsing..");
                customEvent.EnemySpawnList = ParseEnemies(hullEvent["SpawnableEnemies"]);
            }
            if (hullEvent.ContainsKey("SpawnableOutsideEnemies")) {
                Plugin.Mls.LogInfo($"SpawnableOutsideEnemies defined. Parsing..");
                customEvent.OutsideEnemySpawnList = ParseEnemies(hullEvent["SpawnableOutsideEnemies"]);
            }            
            if (hullEvent.ContainsKey("SpawnableDaytimeEnemies")) {
                Plugin.Mls.LogInfo($"SpawnableDaytimeEnemies defined. Parsing..");
                customEvent.DaytimeEnemySpawnList = ParseEnemies(hullEvent["SpawnableDaytimeEnemies"]);
            }
            if (hullEvent.ContainsKey("SpawnableScrap")) {
                Plugin.Mls.LogInfo($"SpawnableScrap defined. Parsing..");
                customEvent.ScrapSpawnList = ParseScrap(hullEvent["SpawnableScrap"]);
            }
            if (hullEvent.ContainsKey("GlobalPowerIncrease")) {
                Plugin.Mls.LogInfo($"GlobalPowerIncrease defined. Parsing..");
                customEvent.addPower = int.Parse(hullEvent["GlobalPowerIncrease"]);
            }
            if (hullEvent.ContainsKey("GlobalOutsidePowerIncrease")) {
                Plugin.Mls.LogInfo($"GlobalOutsidePowerIncrease defined. Parsing..");
                customEvent.addOutsidePower = int.Parse(hullEvent["GlobalOutsidePowerIncrease"]);
            }            
            if (hullEvent.ContainsKey("GlobalDaytimePowerIncrease")) {
                Plugin.Mls.LogInfo($"GlobalDaytimePowerIncrease defined. Parsing..");
                customEvent.addDaytimePower = int.Parse(hullEvent["GlobalDaytimePowerIncrease"]);
            }
            if (hullEvent.ContainsKey("GlobalInsideSpawnRateOverride")) {
                Plugin.Mls.LogInfo($"GlobalInsideSpawnRateOverride defined. Parsing..");
                customEvent.overrideSpawnRate = int.Parse(hullEvent["GlobalInsideSpawnRateOverride"]);
            }
            if (hullEvent.ContainsKey("GlobalOutsideSpawnRateOverride")) {
                Plugin.Mls.LogInfo($"GlobalOutsideSpawnRateOverride defined. Parsing..");
                customEvent.overrideOutsideSpawnRate = int.Parse(hullEvent["GlobalOutsideSpawnRateOverride"]);
            }            
            if (hullEvent.ContainsKey("GlobalDaytimeSpawnRateOverride")) {
                Plugin.Mls.LogInfo($"GlobalDaytimeSpawnRateOverride defined. Parsing..");
                customEvent.overrideDaytimeSpawnRate = int.Parse(hullEvent["GlobalDaytimeSpawnRateOverride"]);
            }

            // Register and enable the custom event
            AddEvent(customEvent);
        }
    }
    
    private static List<Dictionary<string, string>> LoadEventDataFromCfgFiles()
    {
        string directoryPath = GetCustomEventDirectoryPath();
        if (directoryPath == null) return [];

        string[] cfgFiles = Directory.GetFiles(directoryPath, "*.cfg");
        List<Dictionary<string, string>> allEventData = new List<Dictionary<string, string>>();

        foreach (string cfgFile in cfgFiles) {
            var eventData = ParseConfigFile(cfgFile);
            if (eventData == null) continue;
            allEventData.Add(eventData);
            Plugin.Mls.LogInfo($"Loaded custom event: {eventData["EventID"]}");
        }

        return allEventData;
    }
    private static string GetCustomEventDirectoryPath() {
        string directoryPath = Paths.BepInExRootPath + @"\HullEvents";

        if (!Directory.Exists(directoryPath)) {
            Plugin.Mls.LogWarning($"Directory does not exist: {directoryPath}");
            Plugin.Mls.LogInfo("Custom event folder 'HullEvents' not found. Skipping custom event loading.");
            return null;
        }
        return directoryPath;
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
    private static Dictionary<string, List<int>> ParseEnemies(string configValue) {
        Dictionary<string, List<int>> enemies = new Dictionary<string, List<int>>();
        string[] enemyPairs = configValue.Split(',');
        foreach (string pair in enemyPairs) {
            string[] values = pair.Split(":");
            if (values.Length != 4) {
                Plugin.Mls.LogWarning($"Invalid enemies format: {pair}");
                continue;
            }
            string enemy = values[0].Trim();
            int rarity = int.Parse(values[1].Trim());
            int maxCount;
            int power;
            maxCount = values.Length > 2 ? int.Parse(values[2].Trim()) : -1;
            power = values.Length > 3 ? int.Parse(values[3].Trim()) : -1;
            Plugin.Mls.LogInfo($"Parsed enemy: (name = {enemy}, rarity = {rarity}, maxCount = {maxCount}, Power = {power})");
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
            Plugin.Mls.LogInfo($"Parsed scrap item: (name = {item}, rarity = {rarity}");
            scrap.Add(item, rarity);
        }
        return scrap;
    }

    private static void AddEvent(HullEvent newEvent)
    {
        Plugin.Mls.LogInfo("Adding new event " + newEvent.ID() + " to dictionary");
        EventsManager.EventDictionary.Add(newEvent);
    }
    
    public static void DebugLoadCustomEvents()
    {
        foreach (var hullEvent in EventsManager.EventDictionary)
        {
            if (hullEvent is CustomEvent customEvent)
            {
                Plugin.Mls.LogInfo($"Event ID: {customEvent.ID()}");
                Plugin.Mls.LogInfo($"Message: {customEvent.GetMessage()}");
                Plugin.Mls.LogInfo($"Spawnable Enemies: {string.Join(";  ", customEvent.EnemySpawnList.Select(enemy => enemy.Key + " (Rarity percent:" + enemy.Value[0] + ", Max Count:" + enemy.Value[1] + ", Power Level:" + enemy.Value[2] + ")"))}");
                Plugin.Mls.LogInfo($"Spawnable Outside Enemies: {string.Join("; ", customEvent.OutsideEnemySpawnList.Select(enemy => enemy.Key + " (Rarity percent:" + enemy.Value[0] + ", Max Count:" + enemy.Value[1] + ", Power Level:" + enemy.Value[2] + ")"))}");
                Plugin.Mls.LogInfo($"Spawnable Daytime Enemies: {string.Join("; ", customEvent.DaytimeEnemySpawnList.Select(enemy => enemy.Key + " (Rarity percent:" + enemy.Value[0] + ", Max Count:" + enemy.Value[1] + ", Power Level:" + enemy.Value[2] + ")"))}");
                //Plugin.Mls.LogInfo($"Spawnable Outside Enemies: {string.Join(", ", customEvent.OutsideEnemySpawnList)}");
                //Plugin.Mls.LogInfo($"Spawnable Daytime Enemies: {string.Join(", ", customEvent.DaytimeEnemySpawnList)}");
                Plugin.Mls.LogInfo($"Spawnable scrap: {string.Join(", ", customEvent.ScrapSpawnList)}");
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