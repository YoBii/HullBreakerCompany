using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using HullBreakerCompany.Events;
using HullBreakerCompany.Hull;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace HullBreakerCompany
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private static bool _loaded;
        public static ManualLogSource Mls;

        public static bool compatibilityLQ = false;

        public static float BunkerEnemyScale;
        public static float LandmineScale;
        public static float SpikeTrapScale;
        public static float TurretScale;
        public static bool UseShortChatMessages;
        public static bool EnableEventMessages;

        public static bool UseHullBreakerLevelSettings;
        public static bool UseVanillaGameSettings;
        public static string LevelSettings;

        public static int MaxEnemyPowerCount;
        public static int MaxOutsideEnemyPowerCount;
        public static int MaxDaytimeEnemyPowerCount;
        public static bool IncreaseEventCountPerDay;
        public static int EventCount;
        public static int BountyRewardMin;
        public static int BountyRewardMax;
        public static int BountyRewardLimit;
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
            //ConfigManager.RefreshConfig();

            GameObject hullManager = new GameObject("HullManager");           
            DontDestroyOnLoad(hullManager);
            hullManager.hideFlags = (HideFlags)61;
            hullManager.AddComponent<HullManager>();

            SceneManager.sceneUnloaded += AfterGameInit;
            
            Mls.LogInfo("HullManager created");

            _loaded = true;
        }

        private void AfterGameInit(Scene scene) {
            if (scene.name != "InitScene") {
                return;
            }

            // Check for LQ. If present apply patch so we undo our changes just before LQ does
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(LethalQuantities.PluginInfo.PLUGIN_GUID)) {
                Mls.LogInfo("Lethal Quantities found! Applying compatibility patch!");
                Harmony.CreateAndPatchAll(typeof(EventsManager));
            }

            // Check mod and custom events
            EventsManager.AddModEvents();
            EventsManager.AddCustomEvents();

            // Refresh config and weights
            ConfigManager.RefreshConfig();
            ConfigManager.GetWeights();

            // Unload this
            SceneManager.sceneUnloaded -= AfterGameInit;
        }

        [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.LoadNewLevel))]
        [HarmonyPrefix]
        [HarmonyPriority(50)] // make sure we come last
        static bool ModifiedLoad(ref SelectableLevel newLevel) {
            Mls.LogInfo("Client is host: " + RoundManager.Instance.IsHost);
            
            if (!RoundManager.Instance.IsHost) {
                Mls.LogInfo("LoadNewLevel called as client. Stopping..");
                return true;
            }

            HullManager.LogBox("MODIFIED LEVEL LOAD");

            Plugin.Mls.LogInfo($"Attempting to load and modify new level. ID: {newLevel.levelID}, Scene: {newLevel.sceneName}, Planet: {newLevel.PlanetName}");

            // Skip if level is company (Gordion)
            if (newLevel.levelID == 3) {
                Plugin.Mls.LogInfo("Level is company.");
                Plugin.Mls.LogInfo("Skipping modifications..");
                return true;
            }
            
            ConfigManager.RefreshConfig();
            
            //Event Execution
            EventsManager.ExecuteEvents(newLevel);
            
            return true;
        }
        private static void LQCompaitiblityPatch() {
            if (!compatibilityLQ) {
                if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(LethalQuantities.PluginInfo.PLUGIN_GUID)) {
                    Mls.LogInfo("Lethal Quantities found! Applying compatibility patch!");
                    Harmony.CreateAndPatchAll(typeof(EventsManager));
                    compatibilityLQ = true;
                }
            }
        }
    }
}