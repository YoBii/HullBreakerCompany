using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using HullBreakerCompany.Event;
using HullBreakerCompany.Events;
using HullBreakerCompany.hull;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HullBreakerCompany
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private static bool _loaded;
        public static ManualLogSource Mls;
        
        //Events
        public static bool OneForAllIsActive;
        public static bool BountyIsActive;
        
        Harmony _harmony = new("HULLBREAKER");

        private void Awake()
        {
            Mls = BepInEx.Logging.Logger.CreateLogSource("HULLBREAKER");
            Mls.LogInfo("Ready to break hull; HullBreakerCompany");
            _harmony.PatchAll(typeof(Plugin));
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
            Mls.LogInfo("Client is host: " + RoundManager.Instance.IsHost);
            if (!RoundManager.Instance.IsHost) return true;
                
            //Events & stopCoroutine
            BountyIsActive = false;
            OneForAllIsActive = false;
            ResetLevelUnits(newLevel);
            
            var n = newLevel;
            var randomEvents = RandomEnumSelector.GetRandomGameEvents();
            var componentRarity = new Dictionary<Type, int>();
            componentRarity.Clear();
            var eventDictionary = new Dictionary<GameEvents, HullEvent>
            {
                { GameEvents.FlowerMan, new FlowerManEvent() },
                { GameEvents.Turret, new TurretEvent() },
                { GameEvents.LandMine, new LandMineEvent() },
                { GameEvents.HoarderBug, new HoarderBugEvent() },
                { GameEvents.SpringMan, new SpringManEvent() },
                { GameEvents.Lizards, new LizardsEvent() },
                { GameEvents.Arachnophobia, new ArachnophobiaEvent() },
                { GameEvents.Bee, new BeeEvent() },
                { GameEvents.Slime, new SlimeEvent() },
                { GameEvents.DevochkaPizdec, new DevochkaPizdecEvent() },
                { GameEvents.EnemyBounty, new EnemyBountyEvent() },
                { GameEvents.OneForAll, new OneForAllEvent() },
                { GameEvents.OpenTheNoor, new OpenTheNoorEvent() },
                { GameEvents.OnAPowderKeg, new OnAPowderKegEvent() },
                { GameEvents.OutSideEnemyDay, new OutSideEnemyDayEvent()},
                { GameEvents.Hell, new HellEvent()},
                { GameEvents.Nothing, new NothingEvent()}
                
            };
            
            HUDManager.Instance.AddTextToChatOnServer("<color=red>NOTES ABOUT MOON:</color>\"");
            
            n.maxScrap += Random.Range(10, 30);
            n.maxTotalScrapValue += 800;
            n.outsideEnemySpawnChanceThroughDay = new AnimationCurve((Keyframe[])new Keyframe[3]
            {
                new (0f, -64f),
                new (32f, -64f),
                new (32f, 16f)
            });

            if (!randomEvents.Contains(GameEvents.Hell))
            {
                componentRarity.Add(typeof(JesterAI), 1);
            }
            if (!randomEvents.Contains(GameEvents.SpringMan))
            {
                componentRarity.Add(typeof(SpringManAI), 10);
            }
            
            foreach (GameEvents gameEvent in randomEvents)
            {
                try
                {
                    if (eventDictionary.TryGetValue(gameEvent, out HullEvent hullEvent))
                    {
                        hullEvent.Execute(newLevel, componentRarity);
                        Mls.LogInfo($"Event: {gameEvent}");
                    }

                    if (componentRarity.Count <= 0) continue;
                    foreach (var unit in newLevel.Enemies)
                    {
                        foreach (var componentRarityPair in componentRarity.Where(componentRarityPair => unit.enemyType.enemyPrefab.GetComponent(componentRarityPair.Key) != null))
                        {
                            unit.rarity = componentRarityPair.Value;
                            break;
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
            n.enemySpawnChanceThroughoutDay = new AnimationCurve(new Keyframe(0f, 256f));

            newLevel = n;

            return true;
        }
        public static void LevelUnits(SelectableLevel n, bool turret = false, bool landmine = false)
        {
            
            Mls.LogInfo($"Turret: {turret}, Landmine: {landmine}");
            var curve = new AnimationCurve(new Keyframe(0f, 64f),
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
            
            HullManager.SendChatMessage("<color=green>Workers get paid for killing enemy</color>");
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
            
            HullManager.SendChatMessage("<color=red>One of the workers died, the ship will go into orbit in an hour</color>");
        }

    }
}