﻿using System;
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
        public static bool BountyFirstKill;

        public static int DaysPassed;
        public static float BunkerEnemyScale;
        public static float LandmineScale;
        public static float TurretScale;
        public static bool UseShortChatMessages;
        public static bool EnableEventMessages;

        public static bool UseHullBreakerLevelSettings;
        public static bool UseVanillaGameSettings;

        public static int MaxEnemyPowerCount;
        public static int MaxOutsideEnemyPowerCount;
        public static int MaxDaytimeEnemyPowerCount;
        public static bool IncreaseEventCountPerDay;
        public static int EventCount;
        public static int BountyRewardMin;
        public static int BountyRewardMax;
        public static int HullBreakEventCreditsMin;
        public static int HullBreakEventCreditsMax;
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
            
            //Begin modified Load
            Plugin.Mls.LogInfo($"Attempting to load and modify new level. ID: {newLevel.levelID}, Scene: {newLevel.sceneName}, Planet: {newLevel.PlanetName}");

            if (newLevel.levelID == 3) {
                Plugin.Mls.LogInfo("Level is company.");
                Plugin.Mls.LogInfo("Skipping modifications..");
                return true;
            }

            //Event Execution
            EventsManager.ExecuteEvents(newLevel);
                    
                    CurrentEvents.Add(hullEvent);
                }
                catch (NullReferenceException ex)
                {
                    Mls.LogError(
                        $"NullReferenceException caught while processing event: {gameEvent}. Exception message: {ex.Message}. Caused : {ex.InnerException}");
                }
            }

            if (EventCount != 0 && EnableEventMessages) //check if configs allows events
            {
                //count how many active events are NothingEvent
                int nothingEvent_count = roundEvents.Where(e => e.Equals("Nothing")).Count();
                Mls.LogInfo($"Round events: " + roundEvents.Count);
                Mls.LogInfo($"Nothing events: " + nothingEvent_count);
                
                //Only print Notes to game chat when there's at least one Event that's not NothingEvent
                if (roundEvents.Count > nothingEvent_count) {
                    HullManager.AddChatEventMessage("<color=red>NOTES ABOUT MOON:</color>", true);
                    HullManager.SendChatEventMessages();
                } 
            }
            
            //debug logs
            HullManager.LogEnemyRarity(newLevel.Enemies, "\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b\u2b1bENEMIES RARITY\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b");
            HullManager.LogEnemyRarity(newLevel.DaytimeEnemies, "\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b\u2b1bDAYTIME ENEMIES RARITY\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b");
            HullManager.LogEnemyRarity(newLevel.OutsideEnemies, "\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b\u2b1bOUTSIDE ENEMIES RARITY\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b\u2b1b");
            
            
            if (UseHullBreakerLevelSettings)
            {
                nl.maxEnemyPowerCount += 16;
                nl.maxOutsideEnemyPowerCount += 20;

                nl.daytimeEnemySpawnChanceThroughDay = new AnimationCurve(new Keyframe(0f, 5f), new Keyframe(0.5f, 5f));
                nl.enemySpawnChanceThroughoutDay = new AnimationCurve(new Keyframe(0f, 256f));
            }
            else if (UseVanillaGameSettings)
            {
                Mls.LogInfo("Vanilla settings");
            }
            else
            {
                nl.maxEnemyPowerCount = MaxEnemyPowerCount;
                nl.maxOutsideEnemyPowerCount = MaxOutsideEnemyPowerCount;
                nl.maxDaytimeEnemyPowerCount = MaxDaytimeEnemyPowerCount;
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
        
        public static void addLandminesToLevelUnits(SelectableLevel level, float overrideAmount = 0f)
        {
            float amount = LandmineScale;
            if (overrideAmount > 0f)
                amount = overrideAmount;
            Mls.LogInfo($"Adding Landmines: {amount}");

            foreach (var mapObject in level.spawnableMapObjects)
            {
                if ((UnityEngine.Object)(object)mapObject.prefabToSpawn.GetComponentInChildren<Landmine>() != null)
                {
                    mapObject.numberToSpawn = new AnimationCurve(new Keyframe(0f, amount));
                }
            }
        }

        public static void addTurretsToLevelUnits(SelectableLevel n, float overrideAmount = 0f)
        {
            float amount = TurretScale;
            if (overrideAmount > 0f)
                amount = overrideAmount;
            Mls.LogInfo($"Adding Turrets: {amount}");
            foreach (var mapObject in n.spawnableMapObjects)
            {
                if (mapObject.prefabToSpawn.GetComponentInChildren<Turret>() != null)
                {
                    mapObject.numberToSpawn = new AnimationCurve(new Keyframe(0f, amount));
                }
            }
        }


        private static void ResetLevelUnits(SelectableLevel level)
        {
            foreach (var mapObject in level.spawnableMapObjects)
            {
                if (mapObject.prefabToSpawn.GetComponentInChildren<Landmine>() != null)
                {
                    mapObject.numberToSpawn = new AnimationCurve(new Keyframe(0f, 2.5f));
                }
                if (mapObject.prefabToSpawn.GetComponentInChildren<Turret>() != null)
                {
                    mapObject.numberToSpawn = new AnimationCurve(new Keyframe(0f, 2.5f));
                }
            }
        }

    }
}