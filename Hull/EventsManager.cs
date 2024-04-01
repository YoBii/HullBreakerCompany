using System;
using System.Collections.Generic;
using HullBreakerCompany.Events.Enemy;
using HullBreakerCompany.Events.Misc;
using HullBreakerCompany.Events.Scrap;
using UnityEngine;
using System.Linq;
using Mono.Cecil;
using HarmonyLib;
using LethalQuantities.Objects;

namespace HullBreakerCompany.Hull;

public abstract class EventsManager {
    //public static string CurrentMessage = "";
    public static int DaysPassed;
    public static bool modEventsLoaded = false;
        
    private static LevelModifier levelModifier;

    public static List<HullEvent> currentEvents = new();

    public static List<HullEvent> EventDictionary = new()
    {
        // Enemy events
        { new ArachnophobiaEvent() },
        { new BeeEvent() },
        { new DevochkaPizdecEvent() },
        { new FlowerManEvent() },
        { new HellEvent() },
        { new HoarderBugEvent() },
        { new LizardsEvent() },
        { new MaskedEvent() }, //v2.0.0
        { new NutcrackerEvent()}, //v1.3.8
        { new SlimeEvent() },
        { new SpringManEvent() },
        // Misc events
        { new EnemyBountyEvent() },
        { new HackedTurretsEvent() }, //v1.2.0
        { new HordeModeEvent() }, //v2.0.0
        { new HullBreakEvent()}, //v1.3.5
        { new LandMineEvent() },
        { new NothingEvent() },
        { new OnAPowderKegEvent() },
        { new OneForAllEvent() }, // EXPERIMENTAL v2.0.0
        { new OpenTheNoorEvent() },
        { new OutSideEnemyDayEvent() },
        //{ new TimeAnomaly() }, // needs client sync
        //{ new TimeDilationEvent() }, // needs client sync
        { new TurretEvent() },
        // Scrap events
        { new ArmdayEvent() }, //v2.0.0
        { new BabkinPogrebEvent() }, //v1.2.0
        { new ChristmasEveEvent() }, //v2.0.0
        { new ClownshowEvent() }, //v2.0.0
        { new DayDrinkingEvent() }, //v2.0.0
        { new LuckyDayEvent() }, //v2.0.0
        { new SelfDefenseEvent() }, //v2.0.0
    };

    public static void AddModEvents() {
        if (modEventsLoaded) return;
        Plugin.Mls.LogInfo("First level load. Checking for compatible mods..");
        
        Dictionary<HullEvent, string> modEvents = new() {
            { new BoombaEvent(), "evaisa.lethalthings" },
            { new HerobrineEvent(), "Kittenji.HerobrineMod" },
            { new MeltdownEvent(), "me.loaforc.facilitymeltdown" },
            { new ShyGuyEvent(), "DBJ.ShyGuyPatcherPatcher" }
        };

        //Plugin.Mls.LogInfo($"{String.Join(", ", BepInEx.Bootstrap.Chainloader.PluginInfos.Keys.ToList().ToArray())}");    
        foreach(var modEventPair in modEvents) {
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(modEventPair.Value)) {
                Plugin.Mls.LogInfo($"{BepInEx.Bootstrap.Chainloader.PluginInfos[modEventPair.Value].Metadata.Name} found! Enabling event: {modEventPair.Key.ID()}");
                EventDictionary.Add(modEventPair.Key);
            } else {
                Plugin.Mls.LogInfo($"{modEventPair.Value} not present! Disabling event: {modEventPair.Key.ID()}");
            }
        }
        modEventsLoaded = true;
    }
    public static void ExecuteEvents(SelectableLevel newLevel) {
        HullManager.LogBox("EVENT EXECUTION");

        LevelModifier levelMod = new(newLevel);
        levelModifier = levelMod;
        
        UpdateLevelSettings();
        RefreshDaysPassed();

        currentEvents.Clear();
        
        var randomEvents = RandomSelector.GetRandomGameEvents();

        var blacklistedEvents = new List<String>();
        blacklistedEvents.AddRange(randomEvents);

        foreach (string gameEvent in randomEvents)
        {
            try
            {
                HullEvent hullEvent = EventDictionary.FirstOrDefault(e => e.ID() == gameEvent);
                if (hullEvent == null) continue;

                bool success;

                do {
                    Plugin.Mls.LogInfo($"Selected Event: {hullEvent.ID()}");
                    success = hullEvent.Execute(newLevel, levelModifier);
                    if (!success) {
                        blacklistedEvents.Add(hullEvent.ID());
                        Plugin.Mls.LogInfo($"Skipping Event: {hullEvent.ID()}");
                        if (blacklistedEvents.Count() == EventDictionary.Count()) {
                            Plugin.Mls.LogError("Event selection failed!");
                            break;
                        }
                    var newEvent = RandomSelector.GetAnotherRandomGameEvent(blacklistedEvents);
                    hullEvent = EventDictionary.FirstOrDefault(e => e.ID() == newEvent);
                    } else {
                        Plugin.Mls.LogInfo($"Executing Event: {hullEvent.ID()}");
                    }
                } while (!success);
                currentEvents.Add(hullEvent);
            }
            catch (NullReferenceException ex)
            {
                Plugin.Mls.LogError(
                    $"NullReferenceException caught while processing event: {gameEvent}. Exception message: {ex.Message}. Caused : {ex.InnerException}");
            }
        }
        Plugin.Mls.LogInfo($"Round events: " + currentEvents.Count);
        Plugin.Mls.LogInfo($"Nothing events: " + currentEvents.Where(e => e.ID().Equals("Nothing")).Count());

        levelModifier.ApplyModificationsToLevel();

        PrintEventMessagesToGameChat();

        PrintDebugLogs(newLevel);
    }

    private static int RefreshDaysPassed() {
        DaysPassed = HullManager.Instance.timeOfDay.quotaVariables.deadlineDaysAmount - HullManager.Instance.timeOfDay.daysUntilDeadline + 1;
        Plugin.Mls.LogInfo($"Days passed: {DaysPassed}");
        return DaysPassed;
    }
    private static void UpdateLevelSettings() {
        Plugin.Mls.LogInfo($"Level Settings config value: {Plugin.LevelSettings}");
        switch (Plugin.LevelSettings) {
            case "hullbreaker":
                Plugin.Mls.LogInfo("Applying Hullbreaker level settings");
                levelModifier.AddMaxEnemyPower(16);
                levelModifier.AddMaxOutsideEnemyPower(20);
                levelModifier.AddEnemySpawnChanceThroughoutDay(256);
                levelModifier.AddDaytimeEnemySpawnChanceThroughoutDay(5);
                break;
            case "custom":
                Plugin.Mls.LogInfo("Applying custom level settings from config");
                levelModifier.AddMaxEnemyPower(Plugin.MaxEnemyPowerCount);
                levelModifier.AddMaxOutsideEnemyPower(Plugin.MaxOutsideEnemyPowerCount);
                levelModifier.AddMaxDaytimeEnemyPower(Plugin.MaxDaytimeEnemyPowerCount);
                levelModifier.AddEnemySpawnChanceThroughoutDay((int)Plugin.BunkerEnemyScale);
                break;
            default:
                Plugin.Mls.LogInfo("Keeping Vanilla level settings");
                break;

        }
    }
    private static void PrintEventMessagesToGameChat() {
        if (Plugin.EventCount != 0 && Plugin.EnableEventMessages) //check if configs allows events
        {            
            //Only print Notes to game chat when there's at least one Event that's not NothingEvent
            if (currentEvents.Count > currentEvents.Where(e => e.ID().Equals("Nothing")).Count()) {
                HullManager.AddChatEventMessage("<color=red>NOTES ABOUT MOON:</color>", true);
                HullManager.SendChatEventMessages();
            } 
        }
    }
    private static void PrintDebugLogs(SelectableLevel level) {
        HullManager.LogEnemies(level.Enemies, "INSIDE ENEMIES");
        HullManager.LogEnemies(level.OutsideEnemies, "OUTSIDE ENEMIES");
        HullManager.LogEnemies(level.DaytimeEnemies, "DAYTIME ENEMIES");

        HullManager.LogScrapRarity(level.spawnableScrap, "LOOT TABLE");
    }

    // LethalQuantities Compatability Patch
    [HarmonyPatch(typeof(RoundState), nameof(RoundState.OnDestroy))]
    [HarmonyPrefix]
    public static void RoundStateOnDestroyPrefix() {
        levelModifier.undoModificationsEarly();
    }
}