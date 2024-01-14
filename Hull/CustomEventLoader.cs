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
        var customEvents = LoadEventDataFromCfgFiles();
        if (customEvents.Count == 0) return;

        foreach (var hullEvent in customEvents)
        {
            CustomEvent customEvent = new CustomEvent();
            customEvent.SetID(hullEvent["EventID"]);
            customEvent.SetWeight(int.Parse(hullEvent["EventWeight"]));
            customEvent.Rarity = int.Parse(hullEvent["EnemyRarity"]);
            if (hullEvent.ContainsKey("SpawnableEnemies"))
            {
                customEvent.EnemySpawnList = new HashSet<string>(hullEvent["SpawnableEnemies"].Split(',')).ToList();
            }
            if (hullEvent.ContainsKey("SpawnableOutsideEnemies"))
            {
                customEvent.OutsideSpawnList = new HashSet<string>(hullEvent["SpawnableOutsideEnemies"].Split(',')).ToList();
            }

            customEvent.SetMessage(hullEvent["InGameMessage"]);
            customEvent.SetShortMessage(hullEvent["InGameShortMessage"]);

            Plugin.EventDictionary.Add(customEvent);
        }
    }
    
    private static List<Dictionary<string, string>> LoadEventDataFromCfgFiles()
    {
        string directoryPath = Paths.BepInExRootPath + @"\HullEvents";

        if (!Directory.Exists(directoryPath))
        {
            Plugin.Mls.LogError($"Directory does not exist: {directoryPath}");
            return new List<Dictionary<string, string>>();
        }

        string[] cfgFiles = Directory.GetFiles(directoryPath, "*.cfg");
        List<Dictionary<string, string>> allEventData = new List<Dictionary<string, string>>();

        foreach (string cfgFile in cfgFiles)
        {
            string[] lines = File.ReadAllLines(cfgFile);
            Dictionary<string, string> eventData = new Dictionary<string, string>();

            foreach (string line in lines)
            {
                if (line.StartsWith("[") || string.IsNullOrWhiteSpace(line)) continue;

                string[] keyValue = line.Split('=');
                if (keyValue.Length == 2)
                {
                    string key = keyValue[0].Trim();
                    string value = keyValue[1].Trim();
                    eventData[key] = value;
                }
            }

            allEventData.Add(eventData);
            Plugin.Mls.LogInfo($"Loaded event: {eventData["EventID"]}");
        }

        return allEventData;
    }
    public static void AddEvent(HullEvent newEvent)
    {
        Plugin.Mls.LogInfo("Adding new event" + newEvent.ID() + " to dictionary");
        Plugin.EventDictionary.Add(newEvent);
    }
    
    public static void DebugLoadCustomEvents()
    {
        foreach (var hullEvent in Plugin.EventDictionary)
        {
            if (hullEvent is CustomEvent customEvent)
            {
                Plugin.Mls.LogInfo($"Event ID: {customEvent.ID()}");
                Plugin.Mls.LogInfo($"Spawnable Enemies: {string.Join(", ", customEvent.EnemySpawnList)}");
                // Mls.LogInfo($"Spawnable Outside Enemies: {string.Join(", ", customEvent.OutsideSpawnList)}");
                Plugin.Mls.LogInfo($"Message: {customEvent.GetMessage()}");
            }
        }
    }
}