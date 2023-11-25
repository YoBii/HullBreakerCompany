using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using HullBreakerCompany.hull;
using LC_API;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HullBreakerCompany
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private static bool _loaded;
        public static ManualLogSource Mls;
        private static bool _bountyIsActive;
        Harmony _harmony = new(MyPluginInfo.PLUGIN_GUID);

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
                GameObject gameObject = new GameObject("QuotaChanger");
                DontDestroyOnLoad(gameObject);
                gameObject.AddComponent<GameChanger>();
                _loaded = true;
            }
        }

        [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.LoadNewLevel))]
        [HarmonyPrefix]
        static bool ModifiedLoad(ref SelectableLevel newLevel)
        {
            Mls.LogInfo("Client is host: " + RoundManager.Instance.IsHost);
            if (!RoundManager.Instance.IsHost) return true;
                
            _bountyIsActive = false;
            var n = newLevel;
            var randomEvents = RandomEnumSelector.GetRandomGameEvents();
            var componentRarity = new Dictionary<Type, int>();
            HUDManager.Instance.AddTextToChatOnServer("<color=red>NOTES ABOUT THE MOON:</color>\"");

            n.maxScrap += Random.Range(10, 30);
            n.maxTotalScrapValue += 800;
            
            foreach (GameEvents gameEvent in randomEvents)
            {
                string message = null;
                bool turret = false, landmine = false;
                
                switch (gameEvent)
                {
                    case GameEvents.FlowerMan:
                        message = "<color=white>So many eyes in the dark, carefully</color>";
                        componentRarity.Add(typeof(FlowermanAI), 512);
                        break;
                    case GameEvents.Turret:
                        message = "<color=white>Alert, turrets detected</color>";
                        turret = true;
                        break;
                    case GameEvents.SpringMan:
                        message = "<color=white>It's impossible not to look at them</color>";
                        componentRarity.Add(typeof(SpringManAI), 512);
                        break;
                    case GameEvents.HoarderBug:
                        message = "<color=white>Keep an eye on the loot, Hoarding Bugs nearby</color>";
                        componentRarity.Add(typeof(HoarderBugAI), 512);
                        break;
                    case GameEvents.LandMine:
                        message = "<color=white>Watch your step, there's a lot of landmines</color>";
                        landmine = true;
                        break;
                    case GameEvents.DevochkaPizdec:
                        message = "<color=white>A lot of people are going crazy here</color>";
                        componentRarity.Add(typeof(DressGirlAI), 512);
                        break;
                    case GameEvents.OutSideEnemyDay:
                        message = "<color=white>Enemies on the surface during the daytime</color>";
                        n.minScrap = 28;
                        n.maxScrap = 28;
                        n.outsideEnemySpawnChanceThroughDay = new AnimationCurve(new Keyframe(0f, 512f));
                        break;
                    case GameEvents.Lizards:
                        message = "<color=white>Horrible smell from toxic lizards</color>";
                        componentRarity.Add(typeof(PufferAI), 512);
                        break;
                    case GameEvents.Arachnophobia:
                        message = "<color=white>Possible habitat of spiders</color>";
                        componentRarity.Add(typeof(SandSpiderAI), 512);
                        break;
                    case GameEvents.Slime:
                        message = "<color=white>Inhabited with slime</color>";
                        componentRarity.Add(typeof(BlobAI), 512);
                        break;
                    case GameEvents.Bee:  
                        message = "<color=white>Possibly a large number of bee hives</color>";
                        foreach (var unit in newLevel.DaytimeEnemies.Where(unit => unit.enemyType.enemyPrefab.GetComponent<RedLocustBees>() != null))
                        {
                            unit.rarity = 512;
                            break;
                        }
                        break;
                    case GameEvents.EnemyBounty:
                        message = "<color=white>Company pays money for killing the enemies!</color>";
                        _bountyIsActive = true;
                        break;
                    case GameEvents.Nothing:
                        List<string> nothingMessages = new List<string>
                        {
                            "<color=white>A calm day in space. No threats detected</color>",
                            "<color=white>Peaceful day. Good time for exploration</color>",
                            "<color=white>Nothing unusual detected. Proceed with your mission</color>"
                        };
                        int randomIndex = Random.Range(0, nothingMessages.Count);
                        message = nothingMessages[randomIndex];
                        break;
                }

                HUDManager.Instance.AddTextToChatOnServer(message);

                if (componentRarity.Count > 0)
                {
                    foreach (var unit in newLevel.Enemies)
                    {
                        foreach (var componentRarityPair in componentRarity.Where(componentRarityPair => unit.enemyType.enemyPrefab.GetComponent(componentRarityPair.Key) != null))
                        {
                            unit.rarity = componentRarityPair.Value;
                            break;
                        }
                    }
                }

                if (turret || landmine)
                {
                    LevelUnits(n, turret, landmine);
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
            n.enemySpawnChanceThroughoutDay = new AnimationCurve(new Keyframe(0f, 512f));

            newLevel = n;

            return true;
        }

        private static SelectableLevel LevelUnits(SelectableLevel n, bool turret = false, bool landmine = false)
        {
            
            Mls.LogInfo($"Turret: {turret}, Landmine: {landmine}");
            var curve = new AnimationCurve(new Keyframe(0f, 100f),
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

            return n;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.KillEnemyServerRpc))]
        static void EnemyBounty()
        {
            Mls.LogInfo($"Enemy killed, bounty is active: {_bountyIsActive}");
            if (!_bountyIsActive) return;
            Terminal tl = FindObjectOfType<Terminal>();
            tl.groupCredits += 30;
            tl.SyncGroupCreditsServerRpc(tl.groupCredits, tl.numberOfItemsInDropship);
            HUDManager.Instance.AddTextToChatOnServer($"<color=green>Workers get paid for killing enemy</color>");
        }

    }
}