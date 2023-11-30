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
        private static bool _loaded;
        public static ManualLogSource Mls;
        
        public static bool OneForAllIsActive;
        public static bool BountyIsActive;
        
        public static int DaysPassed;
        public static float BunkerEnemyScale;
        public static float LandMineTurretScale;
        public static bool UseShortChatMessages;
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
        };
        
        public static List<HullEvent> eventDictionary = new()
        {
            { new FlowerManEvent() },
            { new TurretEvent() },
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

            var customEvents = LoadEventDataFromCfgFiles();
            if (customEvents.Count != 0) {
                foreach (var hullEvent in customEvents)
                {
                    CustomEvent customEvent = new CustomEvent();
                    customEvent.SetID(hullEvent["EventID"]);
                    customEvent.SetWeight(int.Parse(hullEvent["EventWeight"]));
                    customEvent.Rarity = int.Parse(hullEvent["EnemyRarity"]);
                    customEvent.SpawnList = hullEvent["SpawnableEnemies"].Split(',').ToList();
                    customEvent.SetMessage(hullEvent["InGameMessage"]);
                    customEvent.SetShortMessage(hullEvent["InGameShortMessage"]);

                    eventDictionary.Add(customEvent);
                }
            }
            BunkerEnemyScale = ConfigManager.GetBunkerEnemyScale();
            LandMineTurretScale = ConfigManager.GetLandMineTurretScale();
            UseShortChatMessages = ConfigManager.GetUseShortChatMessages();
        }

        public void OnDestroy()
        {
            if (!_loaded)
            {
                var hullManager = new GameObject("HullManager");
                DontDestroyOnLoad(hullManager);
                hullManager.AddComponent<HullManager>();
                
                Mls.LogInfo("HullManager created");
                
                _loaded = true;
            }
        }

        [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.LoadNewLevel))]
        [HarmonyPrefix]
        static bool ModifiedLoad(ref SelectableLevel newLevel)
        {
            //Debug
            DebugLoadCustomEvents();
            
            Mls.LogInfo("Client is host: " + RoundManager.Instance.IsHost);
            if (!RoundManager.Instance.IsHost) return true;
            if (newLevel.levelID == 3)
            {
                Mls.LogInfo("Level is company, skipping");
                DaysPassed = 0;
                return true;
            }
            
            DaysPassed++;
            Mls.LogInfo($"Days passed: {DaysPassed}");
            
            //Events & stopCoroutine
            BountyIsActive = false;
            OneForAllIsActive = false;
            ResetLevelUnits(newLevel);
            
            var n = newLevel;
            var randomEvents = RandomSelector.GetRandomGameEvents();
            var componentRarity = new Dictionary<Type, int>();
            componentRarity.Clear();
            
            NotModifiedSpawnableItemsWithRarity.Clear();
            foreach (var item  in n.spawnableScrap)
            {
                NotModifiedSpawnableItemsWithRarity.Add(item);
            }
            
            HUDManager.Instance.AddTextToChatOnServer("<color=red>NOTES ABOUT MOON:</color>\"");
            
            n.maxScrap += Random.Range(10, 30);
            n.maxTotalScrapValue += 800;
            n.outsideEnemySpawnChanceThroughDay = new AnimationCurve(new Keyframe[3]
            {
                new (0f, -64f),
                new (32f, -64f),
                new (32f, 16f)
            });

            if (!randomEvents.Contains("Hell"))
            {
                componentRarity.Add(typeof(JesterAI), Random.Range(1, 16));
            }
            if (!randomEvents.Contains("Bee"))
            {
                foreach (var unit in n.DaytimeEnemies.Where(unit => unit.enemyType.enemyPrefab.GetComponent<RedLocustBees>() != null))
                {
                    unit.rarity = 22;
                    break;
                }
            }
            if (!randomEvents.Contains("SpringMan"))
            {
                componentRarity.Add(typeof(SpringManAI), Random.Range(10, 32));
            }
            
            foreach (string gameEvent in randomEvents)
            {
                try
                {
                    HullEvent hullEvent = eventDictionary.FirstOrDefault(e => e.ID() == gameEvent);
                    if (hullEvent == null) continue;
                    
                    hullEvent.Execute(newLevel, componentRarity);
                    Mls.LogInfo($"Event: {gameEvent}");
                    
                    if (componentRarity.Count <= 0) continue;
                    for (int i = newLevel.Enemies.Count - 1; i >= 0; i--)
                    {
                        var unit = newLevel.Enemies[i];
                        for (int j = componentRarity.Count - 1; j >= 0; j--)
                        {
                            var componentRarityPair = componentRarity.ElementAt(j);
                            if (unit.enemyType.enemyPrefab.GetComponent(componentRarityPair.Key) != null)
                            {
                                unit.rarity = componentRarityPair.Value;
                                componentRarity.Remove(componentRarityPair.Key);
                                break;
                            }
                        }
                    }
                }
                catch (NullReferenceException ex)
                {
                    Mls.LogError($"NullReferenceException caught while processing event: {gameEvent}. Exception message: {ex.Message}");
                    Mls.LogError("Try set false BepInEx.cfg [ChainLoader] HideManagerGameObject");
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
            
            n.maxEnemyPowerCount += 2000;
            n.maxOutsideEnemyPowerCount += 20;
            n.maxDaytimeEnemyPowerCount += 200;

            n.daytimeEnemySpawnChanceThroughDay = new AnimationCurve(new Keyframe(0f, 5f), new Keyframe(0.5f, 5f));
            n.enemySpawnChanceThroughoutDay = new AnimationCurve(new Keyframe(0f, BunkerEnemyScale));

            newLevel = n;

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
                unit.numberToSpawn = new AnimationCurve(new Keyframe(0f, 4f));
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

        public List<Dictionary<string, string>> LoadEventDataFromCfgFiles()
        {
            string directoryPath = BepInEx.Paths.BepInExRootPath + @"\HullEvents";
    
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
            foreach (var hullEvent in eventDictionary)
            {
                if (hullEvent is CustomEvent customEvent)
                {
                    Mls.LogInfo($"Event ID: {customEvent.ID()}");
                    Mls.LogInfo($"Spawnable Enemies: {string.Join(", ", customEvent.SpawnList)}");
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