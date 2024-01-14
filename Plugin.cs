using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using HullBreakerCompany.Events;
using HullBreakerCompany.Hull;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HullBreakerCompany
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private static Dictionary<int, SelectableLevelState> _levelStates = new();

        private static bool _loaded;
        public static ManualLogSource Mls;

        public static bool OneForAllIsActive;
        public static bool BountyIsActive;

        public static int DaysPassed;
        public static float BunkerEnemyScale;
        public static float LandMineTurretScale;
        public static bool UseShortChatMessages;
        public static bool EnableEventMessages;

        public static bool UseHullBreakerLevelSettings;
        public static bool UseDefaultGameSettings;

        public static int MaxEnemyPowerCount;
        public static int MaxOutsideEnemyPowerCount;
        public static int MaxDaytimeEnemyPowerCount;
        public static int MinScrap;
        public static int MaxScrap;
        public static int MinTotalScrapValue;
        public static int MaxTotalScrapValue;

        public static bool ChangeQuotaValue;
        public static int QuotaIncrease;

        public static bool IncreaseEventCountPerDay;
        public static int EventCount;

        public static List<SpawnableItemWithRarity> NotModifiedSpawnableItemsWithRarity = new();
        public static string CurrentMessage = "";

        public static Dictionary<String, Type> EnemyBase = new()
        {
            { "flowerman", typeof(FlowermanAI) },
            { "hoarderbug", typeof(HoarderBugAI) },
            { "springman", typeof(SpringManAI) },
            { "crawler", typeof(CrawlerAI) },
            { "sandspider", typeof(SandSpiderAI) },
            { "jester", typeof(JesterAI) },
            { "centipede", typeof(CentipedeAI) },
            { "blobai", typeof(BlobAI) },
            { "dressgirl", typeof(DressGirlAI) },
            { "pufferenemy", typeof(PufferAI) },
            { "eyelessdogs", typeof(MouthDogAI) },
            { "forestgiant", typeof(ForestGiantAI) },
            { "sandworm", typeof(SandWormAI) },
            { "baboonbird", typeof(BaboonBirdAI) },
            { "nutcrackerenemy", typeof(NutcrackerEnemyAI)},
            { "maskedplayerenemy", typeof(MaskedPlayerEnemy)}
        };

        public static List<HullEvent> EventDictionary = new()
        {
            { new FlowerManEvent() },
            //{ new TurretEvent() },
            { new LandMineEvent() },
            { new HoarderBugEvent() },
            { new SpringManEvent() },
            { new LizardsEvent() },
            { new ArachnophobiaEvent() },
            { new BeeEvent() },
            { new SlimeEvent() },
            { new DevochkaPizdecEvent() },
            { new EnemyBountyEvent() },
            //{ new OneForAllEvent() },
            { new OpenTheNoorEvent() },
            { new OnAPowderKegEvent() },
            { new OutSideEnemyDayEvent() },
            { new HellEvent() },
            { new NothingEvent() },
            { new HackedTurretsEvent() }, //v1.2.0
            { new BabkinPogrebEvent() }, //v1.2.0
            { new HullBreakEvent()}, //v1.3.5
            { new NutcrackerEvent()} //v1.3.8
        };
        
        public static List<HullEvent> CurrentEvents = new();

        readonly Harmony _harmony = new("HULLBREAKER");

        private void Awake()
        {
            Mls = BepInEx.Logging.Logger.CreateLogSource("HULLBREAKER " + PluginInfo.PLUGIN_VERSION);
            Mls.LogInfo("Ready to break hull; HullBreakerCompany");
            _harmony.PatchAll(typeof(Plugin));
            _harmony.PatchAll(typeof(EventsHandler));
            
            if (!_loaded) Initialize();
        }

        public void Start()
        {
            if (!_loaded) Initialize();
        }

        public void OnDestroy()
        {
            if (!_loaded) Initialize();
        }

        public void Initialize()
        {
            ConfigManager.SetConfigValue();

            GameObject hullManager = new GameObject("HullManager");
            DontDestroyOnLoad(hullManager);
            hullManager.hideFlags = (HideFlags)61;
            hullManager.AddComponent<HullManager>();
            
            Mls.LogInfo("HullManager created");

            CustomEventLoader.LoadCustomEvents();
            _loaded = true;
        }

        [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.LoadNewLevel))]
        [HarmonyPrefix]
        static bool ModifiedLoad(ref SelectableLevel newLevel)
        {
            CurrentEvents.Clear();
            Mls.LogInfo("Client is host: " + RoundManager.Instance.IsHost);

            if (!RoundManager.Instance.IsHost) return true;

            CustomEventLoader.DebugLoadCustomEvents();
            
            int levelID = newLevel.levelID;
            if (!_levelStates.ContainsKey(levelID))
            {
                _levelStates[levelID] = new SelectableLevelState(newLevel);
            }
            else
            {
                _levelStates[levelID].RestoreState(newLevel);
            }

            if (newLevel.levelID == 3)
            {
                Mls.LogInfo("Level is company, skipping");
                CurrentMessage = "Events not found";
                DaysPassed = 0;
                return true;
            }

            DaysPassed++;
            Mls.LogInfo($"Days passed: {DaysPassed}");

            BountyIsActive = false;
            OneForAllIsActive = false;
            ResetLevelUnits(newLevel);

            var nl = newLevel;
            var randomEvents = RandomSelector.GetRandomGameEvents();
            var enemyComponentRarity = new Dictionary<Type, int>();
            var outsideComponentRarity = new Dictionary<Type, int>();
            enemyComponentRarity.Clear();

            NotModifiedSpawnableItemsWithRarity.Clear();
            
            foreach (var item in nl.spawnableScrap)
            {
                NotModifiedSpawnableItemsWithRarity.Add(item);
            }

            if (EventCount != 0 && EnableEventMessages)
            {
                HUDManager.Instance.AddTextToChatOnServer("<color=red>NOTES ABOUT MOON:</color>\"");
            }

            //Event Execution
            foreach (string gameEvent in randomEvents)
            {
                try
                {
                    HullEvent hullEvent = EventDictionary.FirstOrDefault(e => e.ID() == gameEvent);
                    if (hullEvent == null) continue;

                    hullEvent.Execute(newLevel, enemyComponentRarity, outsideComponentRarity);
                    Mls.LogInfo($"Event: {gameEvent}");
                    
                    UpdateRarity(newLevel.Enemies, enemyComponentRarity);
                    UpdateRarity(newLevel.OutsideEnemies, outsideComponentRarity);
                    
                    CurrentEvents.Add(hullEvent);
                }
                catch (NullReferenceException ex)
                {
                    Mls.LogError(
                        $"NullReferenceException caught while processing event: {gameEvent}. Exception message: {ex.Message}. Caused : {ex.InnerException}");
                }
            }
            
            //debug logs
            HullManager.LogEnemyRarity(newLevel.Enemies, "\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b\u2b1bENEMIES RARITY\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b");
            HullManager.LogEnemyRarity(newLevel.DaytimeEnemies, "\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b\u2b1bDAYTIME ENEMIES RARITY\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b");
            HullManager.LogEnemyRarity(newLevel.OutsideEnemies, "\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b\u2b1bOUTSIDE ENEMIES RARITY\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b");
            
            
            if (!randomEvents.Contains("Bee"))
            {
                foreach (var unit in nl.DaytimeEnemies.Where(unit =>
                             unit.enemyType.enemyPrefab.GetComponent<RedLocustBees>() != null))
                {
                    unit.rarity = 22;
                    break;
                }
            }
            
            if (UseHullBreakerLevelSettings)
            {
                nl.maxEnemyPowerCount += 16;
                nl.maxOutsideEnemyPowerCount += 20;

                nl.maxScrap += Random.Range(6, 24);
                nl.maxTotalScrapValue += Random.Range(400, 800);

                nl.daytimeEnemySpawnChanceThroughDay = new AnimationCurve(new Keyframe(0f, 5f), new Keyframe(0.5f, 5f));
                nl.enemySpawnChanceThroughoutDay = new AnimationCurve(new Keyframe(0f, 256f));
            }
            else if (UseDefaultGameSettings)
            {
                Mls.LogInfo("Default settings");
            }
            else
            {
                nl.maxEnemyPowerCount = MaxEnemyPowerCount;
                nl.maxOutsideEnemyPowerCount = MaxOutsideEnemyPowerCount;
                nl.maxDaytimeEnemyPowerCount = MaxDaytimeEnemyPowerCount;
                nl.minScrap = MinScrap;
                nl.maxScrap = MaxScrap;
                nl.minTotalScrapValue = MinTotalScrapValue;
                nl.maxTotalScrapValue = MaxTotalScrapValue;
                nl.enemySpawnChanceThroughoutDay = new AnimationCurve(new Keyframe(0f, BunkerEnemyScale));
            }

            newLevel = nl;
            CurrentMessage = "Events not found";
            CurrentMessage = "<color=green>Notes about the <color=white>" + newLevel.PlanetName + ":\n\n";
            foreach (var hullEvent in CurrentEvents)
            {
                CurrentMessage += "<color=orange>" + hullEvent.ID() + ": <color=white> " + hullEvent.GetMessage() + "\n\n";
            }
            
            return true;
        }

        private static void UpdateRarity(List<SpawnableEnemyWithRarity> enemies, Dictionary<Type, int> componentRarity)
        {
            if (componentRarity.Count <= 0) return;

            foreach (var unit in enemies)
            {
                foreach (var componentRarityPair in componentRarity)
                {
                    if (unit.enemyType.enemyPrefab.GetComponent(componentRarityPair.Key) == null)
                        continue;
                    if (enemies.Any(e => e.enemyType == unit.enemyType))
                    {
                        unit.rarity = componentRarityPair.Value;
                        componentRarity.Remove(componentRarityPair.Key);
                    }
                    break;
                }
            }
        }
        
        public static void LevelUnits(SelectableLevel n, bool turret = false, bool landmine = false)
        {
            Mls.LogInfo($"Turret: {turret}, Landmine: {landmine}");
            var curve = new AnimationCurve(new Keyframe(0f, LandMineTurretScale),
                new Keyframe(1f, 25));

            foreach (var unit in n.spawnableMapObjects)
            {
                //var turretComponent = unit.prefabToSpawn.GetComponentInChildren<Turret>();
                var landmineComponent = unit.prefabToSpawn.GetComponentInChildren<Landmine>();

                if (landmineComponent != null)
                {
                    unit.numberToSpawn = curve;
                }
            }
        }

        private static void ResetLevelUnits(SelectableLevel level)
        {
            foreach (var unit in level.spawnableMapObjects)
            {
                var landmineComponent = unit.prefabToSpawn.GetComponentInChildren<Landmine>();
                if (landmineComponent != null)
                {
                    unit.numberToSpawn = new AnimationCurve(new Keyframe(0f, 2.5f));
                }
            }
        }

 
        

    }
}