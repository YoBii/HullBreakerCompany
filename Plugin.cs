using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using HullBreakerCompany.Event;
using HullBreakerCompany.Events;
using HullBreakerCompany.hull;
using HullBreakerCompany.Hull;
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
        
        public static Dictionary<String, Type> EnemyBase = new ()
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
            { "foresgiant", typeof(ForestGiantAI) },
            { "sandworm", typeof(SandWormAI) },
            { "baboonbird", typeof(BaboonBirdAI)}
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
            { new OneForAllEvent() },
            { new OpenTheNoorEvent() },
            { new OnAPowderKegEvent() },
            { new OutSideEnemyDayEvent()},
            { new HellEvent()},
            { new NothingEvent()},
            { new HackedTurretsEvent()},
            { new BabkinPogrebEvent()}
                
        };
        
        Harmony _harmony = new("HULLBREAKER");

        private void Awake()
        {
            Mls = BepInEx.Logging.Logger.CreateLogSource("HULLBREAKER " + PluginInfo.PLUGIN_VERSION);
            Mls.LogInfo("Ready to break hull; HullBreakerCompany");
            _harmony.PatchAll(typeof(Plugin));
            
            if (_loaded) return;
            Initialize();
        }
        public void Start()
        {
            if (_loaded) return;
            Initialize();
        }

        public void OnDestroy()
        {
            if (_loaded) return;
            Initialize();
        }

        public void Initialize()
        {
            ConfigManager.SetConfigValue();
            
            GameObject hullManager = new GameObject("HullManager");
            DontDestroyOnLoad(hullManager);
            hullManager.hideFlags = (HideFlags)61;
            hullManager.AddComponent<HullManager>();
                
            Mls.LogInfo("HullManager created");
            
            var customEvents = LoadEventDataFromCfgFiles();
            if (customEvents.Count != 0) {
                foreach (var hullEvent in customEvents)
                {
                    CustomEvent customEvent = new CustomEvent();
                    customEvent.SetID(hullEvent["EventID"]);
                    customEvent.SetWeight(int.Parse(hullEvent["EventWeight"]));
                    customEvent.Rarity = int.Parse(hullEvent["EnemyRarity"]);
                    if (hullEvent.ContainsKey("SpawnableEnemies"))
                    {
                        customEvent.EnemySpawnList = hullEvent["SpawnableEnemies"].Split(',').ToList();
                    }

                    if (hullEvent.ContainsKey("SpawnableOutsideEnemies"))
                    {
                        customEvent.OutsideSpawnList = hullEvent["SpawnableOutsideEnemies"].Split(',').ToList();
                    }
                    customEvent.SetMessage(hullEvent["InGameMessage"]);
                    customEvent.SetShortMessage(hullEvent["InGameShortMessage"]);

                    EventDictionary.Add(customEvent);
                }
            }
            
            _loaded = true;
        }
        
        [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.LoadNewLevel))]
        [HarmonyPrefix]
        static bool ModifiedLoad(ref SelectableLevel newLevel)
        {
            DebugLoadCustomEvents();
            
            Mls.LogInfo("Client is host: " + RoundManager.Instance.IsHost);
            
            if (!RoundManager.Instance.IsHost) return true;

            int levelID = newLevel.levelID;
            if (!_levelStates.ContainsKey(levelID)) {
                _levelStates[levelID] = new SelectableLevelState(newLevel);
            } else {
                _levelStates[levelID].RestoreState(newLevel);
            }
            
            if (newLevel.levelID == 3)
            {
                Mls.LogInfo("Level is company, skipping");
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
            foreach (var item  in nl.spawnableScrap)
            {
                NotModifiedSpawnableItemsWithRarity.Add(item);
            }
            
            if (EventCount != 0) {
                HUDManager.Instance.AddTextToChatOnServer("<color=red>NOTES ABOUT MOON:</color>\"");
            }
            
            foreach (string gameEvent in randomEvents)
            {
                try
                {
                    HullEvent hullEvent = EventDictionary.FirstOrDefault(e => e.ID() == gameEvent);
                    if (hullEvent == null) continue;
                    
                    hullEvent.Execute(newLevel, enemyComponentRarity, outsideComponentRarity);
                    Mls.LogInfo($"Event: {gameEvent}");
                    
                    //Enemies
                    if (enemyComponentRarity.Count <= 0) continue;
                    for (int i = newLevel.Enemies.Count - 1; i >= 0; i--)
                    {
                        var unit = newLevel.Enemies[i];
                        for (int j = enemyComponentRarity.Count - 1; j >= 0; j--)
                        {
                            var enemyComponentRarityPair = enemyComponentRarity.ElementAt(j);
                            if (unit.enemyType.enemyPrefab.GetComponent(enemyComponentRarityPair.Key) == null) continue;
                            unit.rarity = enemyComponentRarityPair.Value;
                            enemyComponentRarity.Remove(enemyComponentRarityPair.Key);
                            break;
                        }
                    }
                    //Outside Enemies, should work
                    for (int i = newLevel.OutsideEnemies.Count - 1; i >= 0; i--)
                    {
                        var unit = newLevel.OutsideEnemies[i];
                        for (int j = enemyComponentRarity.Count - 1; j >= 0; j--)
                        {
                            var enemyComponentRarityPair = enemyComponentRarity.ElementAt(j);
                            if (unit.enemyType.enemyPrefab.GetComponent(enemyComponentRarityPair.Key) == null) continue;
                            unit.rarity = enemyComponentRarityPair.Value;
                            enemyComponentRarity.Remove(enemyComponentRarityPair.Key);
                            break;
                        }
                    }
                }
                catch (NullReferenceException ex)
                {
                    Mls.LogError($"NullReferenceException caught while processing event: {gameEvent}. Exception message: {ex.Message}. Caused : {ex.InnerException}");
                }
            }
            //debug logs
            Mls.LogInfo("\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b\u2b1bENEMIES RARITY\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b");
            foreach (var unit in newLevel.Enemies)
            {
                Mls.LogInfo($"{unit.enemyType.enemyPrefab.name} - {unit.rarity}");
            }
            Mls.LogInfo("\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b\u2b1bDAYTIME ENEMIES RARITY\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b");
            foreach (var unit in newLevel.DaytimeEnemies)
            {
                Mls.LogInfo($"{unit.enemyType.enemyPrefab.name} - {unit.rarity}");
            }
            Mls.LogInfo("\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b\u2b1bOUTSIDE ENEMIES RARITY\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b");
            foreach (var unit in newLevel.OutsideEnemies)
            {
                Mls.LogInfo($"{unit.enemyType.enemyPrefab.name} - {unit.rarity}");
            }
            
            if (!randomEvents.Contains("Hell"))
            {
                enemyComponentRarity.Add(typeof(JesterAI), Random.Range(1, 8));
            }
            if (!randomEvents.Contains("Bee"))
            {
                foreach (var unit in nl.DaytimeEnemies.Where(unit => unit.enemyType.enemyPrefab.GetComponent<RedLocustBees>() != null))
                {
                    unit.rarity = 22;
                    break;
                }
            }
            if (!randomEvents.Contains("SpringMan"))
            {
                enemyComponentRarity.Add(typeof(SpringManAI), Random.Range(10, 32));
            }

            if (UseHullBreakerLevelSettings)
            {
                nl.maxEnemyPowerCount += 16;
                nl.maxOutsideEnemyPowerCount += 20;
                
                nl.maxScrap += Random.Range(6, 24);
                nl.maxTotalScrapValue += Random.Range(400, 800);
                
                nl.daytimeEnemySpawnChanceThroughDay = new AnimationCurve(new Keyframe(0f, 5f), new Keyframe(0.5f, 5f));
                nl.enemySpawnChanceThroughoutDay = new AnimationCurve(new Keyframe(0f, BunkerEnemyScale));
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
            }
            
            newLevel = nl;

            return true;
        }
        public static void LevelUnits(SelectableLevel n, bool turret = false, bool landmine = false)
        {
            Mls.LogInfo($"Turret: {turret}, Landmine: {landmine}");
            var curve = new AnimationCurve(new Keyframe(0f, LandMineTurretScale),
                new Keyframe(1f, 25));

            foreach (var unit in n.spawnableMapObjects)
            {
                var turretComponent = unit.prefabToSpawn.GetComponentInChildren<Turret>();
                var landmineComponent = unit.prefabToSpawn.GetComponentInChildren<Landmine>();

                if ((turretComponent != null && turret) || (landmineComponent != null && landmine) || (turretComponent == null && landmineComponent == null))
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
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.KillEnemyServerRpc))]
        static void EnemyBounty()
        {
            Mls.LogInfo($"Enemy killed, bounty is active: {BountyIsActive}");
            if (!BountyIsActive) return;
            Terminal tl = FindObjectOfType<Terminal>();
            tl.groupCredits += 30;
            tl.SyncGroupCreditsServerRpc(tl.groupCredits, tl.numberOfItemsInDropship);
            
            HullManager.SendChatEventMessage("<color=green>Workers get paid for killing enemy</color>");
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerControllerB), "KillPlayerServerRpc")]
        static void OneForAll()
        {
            Mls.LogInfo($"Player killed, one for all is active: {OneForAllIsActive}");
            if (!OneForAllIsActive) return;
            HullManager gc = FindObjectOfType<HullManager>();
            gc.timeOfDay.votedShipToLeaveEarlyThisRound = true;
            gc.timeOfDay.SetShipLeaveEarlyServerRpc();
            
            HullManager.SendChatEventMessage("<color=red>One of the workers died, the ship will go into orbit in an hour</color>");
        }

        private static List<Dictionary<string, string>> LoadEventDataFromCfgFiles()
        {
            string directoryPath = Paths.BepInExRootPath + @"\HullEvents";
    
            if (!Directory.Exists(directoryPath))
            {
                Mls.LogError($"Directory does not exist: {directoryPath}");
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
                Mls.LogInfo($"Loaded event: {eventData["EventID"]}");
            }

            return allEventData;
        }

        private static void DebugLoadCustomEvents()
        {
            foreach (var hullEvent in EventDictionary)
            {
                if (hullEvent is CustomEvent customEvent)
                {
                    Mls.LogInfo($"Event ID: {customEvent.ID()}");
                    Mls.LogInfo($"Spawnable Enemies: {string.Join(", ", customEvent.EnemySpawnList)}");
                    Mls.LogInfo($"Spawnable Outside Enemies: {string.Join(", ", customEvent.OutsideSpawnList)}");
                    Mls.LogInfo($"Message: {customEvent.GetMessage()}");
                }
            }
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameNetworkManager), nameof(GameNetworkManager.StartHost))]
        static void ResetDayPassed()
        {
            DaysPassed = 0;
        }
    }
}