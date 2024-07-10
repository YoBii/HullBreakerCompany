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
    public static bool customEventsLoaded = false;
        
    private static LevelModifier levelModifier;

    public static List<HullEvent> EventDictionary = new()
    {
        // Enemy events
        { new ArachnophobiaEvent() },
        { new BeeEvent() },
        { new ButlerEvent() },
        { new CrawlerEvent() }, //v2.2.3
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
        { new SpikeTrapEvent() }, //v50
        { new TimeAnomalyEvent() },
        { new TimeDilationEvent() },
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
        Plugin.Mls.LogInfo("Checking for compatible mods..");
        //Print all plugins
        //Plugin.Mls.LogInfo($"{String.Join(", ", BepInEx.Bootstrap.Chainloader.PluginInfos.Keys.ToList().ToArray())}");

        Dictionary<string, List<HullEvent>> modEvents = new() {
            { "evaisa.lethalthings", [new BoombaEvent()] },
            { "Kittenji.HerobrineMod", [new HerobrineEvent()] },
            { "me.loaforc.facilitymeltdown", [new MeltdownEvent()] },
            { "DBJ.ShyGuyPatcherPatcher", [new ShyGuyEvent()] },
            { "com.potatoepet.AdvancedCompany", new List<HullEvent> { new AC_BunnyEvent(), new AC_ControllerEvent(), new AC_RGBShoesEvent() }  }
        };
        foreach (var modEventPair in modEvents) {
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(modEventPair.Key)) {
                Plugin.Mls.LogInfo($"{BepInEx.Bootstrap.Chainloader.PluginInfos[modEventPair.Key].Metadata.Name} found! Enabling events: {string.Join(", ", modEventPair.Value.Select(e => e.ID()))}");
                foreach (var e in modEventPair.Value) {
                    EventDictionary.Add(e);
                }
            } else {
                Plugin.Mls.LogDebug($"{modEventPair.Key} not present! Not loading associated events..");
            }
        }
        modEventsLoaded = true;
    }
    public static void AddCustomEvents() {
        if (customEventsLoaded) return;
        Plugin.Mls.LogInfo("Checking for custom events..");
        
        CustomEventLoader.LoadCustomEvents();
        CustomEventLoader.DebugLoadCustomEvents();
        
        customEventsLoaded = true;
    }
    public static void ExecuteEvents(SelectableLevel newLevel) {
        HullManager.LogBox("EVENT EXECUTION");

        levelModifier = new LevelModifier(newLevel);

        UpdateLevelSettings();
        RefreshDaysPassed();

        List<HullEvent> chosenEvents = new();

        int eventCount = Plugin.IncreaseEventCountPerDay ? Plugin.EventCount + DaysPassed : Plugin.EventCount;
        Plugin.Mls.LogInfo($"Round events: {eventCount}");

        Plugin.Mls.LogInfo($"Start event selection..");
        
        RandomSelector.InitializeWeights();

        while (chosenEvents.Count() < eventCount) {
            var newEvent = RandomSelector.GetRandomGameEvent();
            if (newEvent == null) {
                Plugin.Mls.LogInfo($"Event selection failed. No events left to execute..");
                break;
            }
            var hullEvent = EventDictionary.FirstOrDefault(e => e.ID() == newEvent);
            if (hullEvent == null) {
                Plugin.Mls.LogWarning($"Couldn't find event {newEvent} in event dictionary!");
                continue;
            }
            Plugin.Mls.LogInfo($"Got event: {hullEvent.ID()}");
            bool success = hullEvent.Execute(newLevel, levelModifier);
            if (success) {
                chosenEvents.Add(hullEvent);
            } else {
                Plugin.Mls.LogInfo($"Skipping event: {hullEvent.ID()}");
            }
        }

        Plugin.Mls.LogInfo($"Selected events: {chosenEvents.Count()}");
        Plugin.Mls.LogInfo($"Nothing events: {chosenEvents.Where(e => e.ID().Equals("Nothing")).Count()}");
        chosenEvents.RemoveAll(e => e.ID().Equals("Nothing"));
        Plugin.Mls.LogInfo($"Active events: {chosenEvents.Count()} [{string.Join(", ", chosenEvents.Select(e => e.ID()))}]");

        levelModifier.ApplyModificationsToLevel();

        HullManager.SendChatEventMessages();

        PrintDebugLogs(newLevel);
    }

    private static int RefreshDaysPassed() {
        DaysPassed = HullManager.Instance.timeOfDay.quotaVariables.deadlineDaysAmount - HullManager.Instance.timeOfDay.daysUntilDeadline;
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
        if (levelModifier == null) {
            Plugin.Mls.LogInfo("No levelModifier found. No modifications to revert.");
            return;
        }
        levelModifier.UndoModificationsEarly();
        levelModifier = null;
    }
}