using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using HullBreakerCompany.Hull;
using static UnityEngine.UIElements.UIR.Implementation.UIRStylePainter;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

namespace HullBreakerCompany;

public class LevelModifier(SelectableLevel level) {
    private SelectableLevel targetLevel = level;

    private readonly Dictionary<string, int> enemyComponentRarityModifications = [];
    private readonly Dictionary<string, int> enemyComponentRarityBackups = [];
    private readonly Dictionary<string, int> enemyComponentMaxCountModifications = [];
    private readonly Dictionary<string, int> enemyComponentMaxCountBackups = [];
    private readonly Dictionary<string, int> enemyComponentPowerModifications = [];
    private readonly Dictionary<string, int> enemyComponentPowerBackups = [];
    private int targetLevelEnemyRarityTotal = 0;

    private readonly Dictionary<string, int> outsideEnemyComponentRarityModifications = [];
    private readonly Dictionary<string, int> outsideEnemyComponentRarityBackups = [];
    private readonly Dictionary<string, int> daytimeEnemyComponentRarityModifications = [];
    private readonly Dictionary<string, int> daytimeEnemyComponentRarityBackups = [];

    private readonly Dictionary<string, int> spawnableScrapRarityModifications = [];
    private readonly Dictionary<string, int> spawnableScrapRarityBackups = [];
    private int targetLevelScrapRarityTotal = 0;

    private int maxEnemyPower;
    private int maxOutsideEnemyPower;
    private int maxDaytimeEnemyPower;

    private int enemySpawnChanceThroughoutDay;
    private AnimationCurve enemySpawnChanceThroghoutDayBackup;
    private int outsideEnemySpawnChanceThroughoutDay;
    private AnimationCurve outsideEnemySpawnChanceThroghoutDayBackup;
    private int daytimeEnemySpawnChanceThroughoutDay;
    private AnimationCurve daytimeEnemySpawnChanceThroghoutDayBackup;

    private float landmines;
    private AnimationCurve landminesBackup;
    private float turrets;
    private AnimationCurve turretsBackup;

    private float timeScale = 0;

    public LevelModifier(SelectableLevel level) {
        targetLevel = level;
    }

    private void OnSceneUnloaded(Scene scene) {
        Plugin.Mls.LogInfo($"Unloading scene: {scene.name}");
        SelectableLevel currentLevel = RoundManager.Instance.currentLevel;
        if (currentLevel == null) {
            Plugin.Mls.LogInfo($"Scene does not contain a level!");
            return;
        }
        if (currentLevel.levelID == 3) {
            Plugin.Mls.LogInfo($"Scene contains company building level!");
            return;
        }
        if (currentLevel != targetLevel) {    
            Plugin.Mls.LogWarning($"currentLevel ({currentLevel.PlanetName}) levelModifier ({targetLevel.PlanetName}) mismatch!");
            return;
        }
        List<string> scenesToIgnore = ["MainMenu", "InitScene", "SampleSceneRelay"];
        if (scenesToIgnore.Any(str => str == scene.name)) {
            Plugin.Mls.LogInfo("Ignoring scene. Not reverting any modifications!");
            return;
        }
        UndoModifications();

        EventsHandler.Reset();
    }

    private void ApplyEnemyComponentRarity(SelectableLevel level, Dictionary<string, int> rarityPairs, Dictionary<string, int> rarityBackups, bool restore = false) {
        if (rarityPairs.Count <= 0) return;

        if (targetLevelEnemyRarityTotal == 0) {
            targetLevelEnemyRarityTotal = level.Enemies.Sum(enemy => enemy.rarity);
        }

        foreach (var rarityPair in rarityPairs) {
            foreach (var enemy in level.Enemies.Where(enemy => enemy.enemyType.enemyPrefab.name == rarityPair.Key)) {
                if (restore) {
                    enemy.rarity = rarityBackups[rarityPair.Key];
                } else {
                    rarityBackups.TryAdd(rarityPair.Key, enemy.rarity);
                    enemy.rarity = (int) Math.Floor((float) rarityPair.Value / 100 * (targetLevelEnemyRarityTotal - enemy.rarity));
                }
                Plugin.Mls.LogInfo($"Setting rarity of {enemy.enemyType.enemyPrefab.name} to {enemy.rarity}");
                break;
            }
        }
    }
    private void ApplyEnemyComponentMaxCount(SelectableLevel level, Dictionary<string, int> maxCountPairs, Dictionary<string, int> maxCountBackups,  bool restore = false) {
        if (maxCountPairs.Count <= 0) return;

        foreach (var maxCountPair in maxCountPairs) {
            foreach (var enemy in level.Enemies.Where(enemy => enemy.enemyType.enemyPrefab.name == maxCountPair.Key)) {
                if (!restore) maxCountBackups.TryAdd(maxCountPair.Key, enemy.enemyType.MaxCount);
                enemy.enemyType.MaxCount = restore ? maxCountBackups[maxCountPair.Key] : maxCountPair.Value;
                Plugin.Mls.LogInfo($"Setting maxCount of {enemy.enemyType.enemyPrefab.name} to {enemy.enemyType.MaxCount}");
                break;
            }
        }
    }
    private void ApplyEnemyComponentPower(SelectableLevel level, Dictionary<string, int> powerPairs, Dictionary<string, int> powerBackups, bool restore = false) {
        if (powerPairs.Count <= 0) return;

        foreach (var powerPair in powerPairs) {
            foreach (var enemy in level.Enemies.Where(enemy => enemy.enemyType.enemyPrefab.name == powerPair.Key)) {
                if (!restore) powerBackups.TryAdd(powerPair.Key, enemy.enemyType.PowerLevel);
                enemy.enemyType.PowerLevel = restore ? powerBackups[powerPair.Key] : powerPair.Value;
                Plugin.Mls.LogInfo($"Setting power of {enemy.enemyType.enemyPrefab.name} to {enemy.enemyType.PowerLevel}");
                break;
            }
        }
    }
    private void ApplyOutsideEnemyComponentRarity(SelectableLevel level, Dictionary<string, int> rarityPairs, Dictionary<string, int> rarityBackups, bool restore = false) {
        if (rarityPairs.Count <= 0) return;

        foreach (var rarityPair in rarityPairs) {
            foreach (var enemy in level.OutsideEnemies.Where(enemy => enemy.enemyType.enemyPrefab.name == rarityPair.Key)) {
                if (!restore) rarityBackups.TryAdd(rarityPair.Key, enemy.rarity);
                enemy.rarity = restore ? rarityBackups[rarityPair.Key] : rarityPair.Value;
                Plugin.Mls.LogInfo($"Setting rarity of {enemy.enemyType.enemyPrefab.name} to {enemy.rarity}");
                break;
            }
        }
    }
    private void ApplyDaytimeEnemyComponentRarity(SelectableLevel level, Dictionary<string, int> rarityPairs, Dictionary<string, int> rarityBackups, bool restore = false) {
        if (rarityPairs.Count <= 0) return;

        foreach (var rarityPair in rarityPairs) {
            foreach (var enemy in level.DaytimeEnemies.Where(enemy => enemy.enemyType.enemyPrefab.name == rarityPair.Key)) {
                if (!restore) rarityBackups.TryAdd(rarityPair.Key, enemy.rarity);
                enemy.rarity = restore ? rarityBackups[rarityPair.Key] : rarityPair.Value;
                Plugin.Mls.LogInfo($"Setting rarity of {enemy.enemyType.enemyPrefab.name} to {enemy.rarity}");
                break;
            }
        }
    }
    private void ApplyLootRarity(SelectableLevel level, Dictionary<string, int> rarityPairs, Dictionary<string, int> rarityBackups, bool restore = false) {
        if (rarityPairs.Count <= 0) return;

        if (targetLevelScrapRarityTotal == 0) {
            targetLevelScrapRarityTotal = level.spawnableScrap.Sum(scrap => scrap.rarity);
        }

        foreach (var rarityPair in rarityPairs) {
            foreach (var item in level.spawnableScrap.Where(item => item.spawnableItem.itemName == rarityPair.Key)) {
                if (restore) {
                    item.rarity = rarityBackups[rarityPair.Key];
                } else {
                    rarityBackups.TryAdd(rarityPair.Key, item.rarity);
                    item.rarity = (int) Math.Floor((float) rarityPair.Value / 100 * (targetLevelScrapRarityTotal - item.rarity));
                }
                Plugin.Mls.LogInfo($"Setting rarity of {item.spawnableItem.itemName} to {item.rarity}");
                break;
            }
        }
    }
    private void ApplyMaxEnemyPower(SelectableLevel level, int power, bool restore = false) {
        if (power <= 0) return;
        if (!restore) maxEnemyPower = level.maxEnemyPowerCount; 
        level.maxEnemyPowerCount = restore ? power : level.maxEnemyPowerCount + power;
        Plugin.Mls.LogInfo($"Setting global max enemy power to {level.maxEnemyPowerCount}");
    }
    private void ApplyMaxOutsideEnemyPower(SelectableLevel level, int power, bool restore = false) {
        if (power <= 0) return;
        if (!restore) maxOutsideEnemyPower = level.maxOutsideEnemyPowerCount;
        level.maxOutsideEnemyPowerCount = restore ? power : level.maxOutsideEnemyPowerCount + power;
        Plugin.Mls.LogInfo($"Setting global max enemy power to {level.maxOutsideEnemyPowerCount}");
    }
    private void ApplyMaxDaytimeEnemyPower(SelectableLevel level, int power, bool restore = false) {
        if (power <= 0) return;
        if (!restore) maxDaytimeEnemyPower = level.maxDaytimeEnemyPowerCount;
        level.maxDaytimeEnemyPowerCount = restore ? power : level.maxDaytimeEnemyPowerCount + power;
        Plugin.Mls.LogInfo($"Setting global max enemy power to {level.maxDaytimeEnemyPowerCount}");
    }
    private void ApplyEnemySpawnChanceThroughoutDay(SelectableLevel level, int value, bool revert = false) {
        if (value <= 0) return;
        if (!revert) {
            enemySpawnChanceThroghoutDayBackup = level.enemySpawnChanceThroughoutDay;
            level.enemySpawnChanceThroughoutDay = new AnimationCurve(new Keyframe(0f, value));
            Plugin.Mls.LogInfo($"Setting global enemy spawn chance to {value}");
        } else {
            level.enemySpawnChanceThroughoutDay = enemySpawnChanceThroghoutDayBackup;
            Plugin.Mls.LogInfo($"Reverting global enemy spawn chance");
        }
    }
    private void ApplyOutsideEnemySpawnChanceThroughoutDay(SelectableLevel level, int value, bool revert = false) {
        if ( value <= 0) return;
        if (!revert) {
            outsideEnemySpawnChanceThroghoutDayBackup = level.outsideEnemySpawnChanceThroughDay;
            level.outsideEnemySpawnChanceThroughDay = new AnimationCurve(new Keyframe(0f, value));
            Plugin.Mls.LogInfo($"Setting global outside enemy spawn chance to {value}");
        } else {
            level.outsideEnemySpawnChanceThroughDay = outsideEnemySpawnChanceThroghoutDayBackup;
            Plugin.Mls.LogInfo($"Reverting global outside enemy spawn chance");
        }
    }
    private void ApplyDaytimeEnemySpawnChanceThroughoutDay(SelectableLevel level, int value, bool revert = false) {
        if (value <= 0) return;
        if (!revert) {
            daytimeEnemySpawnChanceThroghoutDayBackup = level.daytimeEnemySpawnChanceThroughDay;
            level.daytimeEnemySpawnChanceThroughDay = new AnimationCurve(new Keyframe(0f, value));
            Plugin.Mls.LogInfo($"Setting global daytime enemy spawn chance to {value}");
        } else {
            level.daytimeEnemySpawnChanceThroughDay = daytimeEnemySpawnChanceThroghoutDayBackup;
            Plugin.Mls.LogInfo($"Reverting global daytime enemy spawn chance");
        }
    }
    private void ApplyTimeScale(SelectableLevel level, float value, bool revert = false) {  
        if (value == 0) return;
        if (!revert) {
            //HullManager.Instance.timeOfDay.globalTimeSpeedMultiplier *= value;
            TimeOfDay.Instance.globalTimeSpeedMultiplier *= value;
            Plugin.Mls.LogInfo($"Increasing TimeSpeedMultiplier: * {value}");
        } else {
            //HullManager.Instance.timeOfDay.globalTimeSpeedMultiplier /= value;
            TimeOfDay.Instance.globalTimeSpeedMultiplier /= value;
            Plugin.Mls.LogInfo($"Decreasing TimeSpeedMultiplier: / {value}");
        }
    }
    public void ApplyUnitModifications(SelectableLevel level, float landmineAmount, float turretAmount) {
        if (landmineAmount > 0) {
            Plugin.Mls.LogInfo($"Adding Landmines: {landmineAmount}");

            foreach (var mapObject in level.spawnableMapObjects) {
                if (mapObject.prefabToSpawn.GetComponentInChildren<Landmine>() != null) {
                    landminesBackup = mapObject.numberToSpawn;
                    mapObject.numberToSpawn = new AnimationCurve(new Keyframe(0f, landmineAmount));
                }
            }
        }

        if (turretAmount > 0) {
            Plugin.Mls.LogInfo($"Adding Turrets: {turretAmount}");

            foreach (var mapObject in level.spawnableMapObjects) {
                if (mapObject.prefabToSpawn.GetComponentInChildren<Turret>() != null) {
                    turretsBackup = mapObject.numberToSpawn;
                    mapObject.numberToSpawn = new AnimationCurve(new Keyframe(0f, turretAmount));
                }
            }
        }
    }
    private void UndoUnitModifications(SelectableLevel level) {
        foreach (var mapObject in level.spawnableMapObjects) {
            if (mapObject.prefabToSpawn.GetComponentInChildren<Landmine>() != null) {
                if (landminesBackup != null) {
                    mapObject.numberToSpawn = landminesBackup;
                    Plugin.Mls.LogInfo($"Reverting added landmines");
                }
            }
            if (mapObject.prefabToSpawn.GetComponentInChildren<Turret>() != null) {
                if (turretsBackup != null) {
                    mapObject.numberToSpawn = turretsBackup;
                    Plugin.Mls.LogInfo($"Reverting added turrets");
                }
            }
        }
    }
    private bool IsTargetLevelSet() {
        if (targetLevel == null) {
            Plugin.Mls.LogWarning("Target level not set!");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Adds an enemy to the levelModifier to modify its rarity.
    /// </summary>
    /// <param name="enemy">The enemyPrefab name</param>
    /// <param name="percentOfTotalRarity">specified as percentage of the levels total enemy rarity</param>
    public void AddEnemyComponentRarity(string enemy, int percentOfTotalRarity) {
    enemyComponentRarityModifications.TryAdd(enemy, percentOfTotalRarity);
    }
    public void AddEnemyComponentMaxCount(string enemy, int maxCount) {
        enemyComponentMaxCountModifications.TryAdd(enemy, maxCount);
    }
    public void AddEnemyComponentPower(string enemy, int power) {
        enemyComponentPowerModifications.TryAdd(enemy, power);
    }
    public void AddDaytimeEnemyRarity(string enemy, int rarity) {
        daytimeEnemyComponentRarityModifications.TryAdd(enemy, rarity);
    }
    public void AddOutsideEnemyRarity(string enemy, int rarity) {
        outsideEnemyComponentRarityModifications.TryAdd(enemy, rarity);
    }

    /// <summary>
    /// Adds scrap to the levelModifier to modify its rarity.
    /// </summary>
    /// <param name="item">The scrap item name</param>
    /// <param name="percentOfTotalRarity">specified as percentage of the levels total scrap rarity</param>
    public void AddSpawnableScrapRarity(string item, int percentOfTotalRarity) {
        spawnableScrapRarityModifications.TryAdd(item, percentOfTotalRarity);
    }

    /// <summary>
    /// Adds a dictionary of scrap to the levelModifier to modify their rarity.
    /// </summary>
    /// <param name="scrapToSpawn">scrap item to add by name and respective rarity as percentage of the levels total scrap rarity</param>
    public void AddSpawnableScrapRarityDict(Dictionary<string, int> scrapToSpawn) {
        foreach (var scrap in scrapToSpawn) {
            AddSpawnableScrapRarity(scrap.Key, scrap.Value);
        }
    }
    public void AddMaxEnemyPower(int power) {
        maxEnemyPower = power;
    }
    public void AddMaxOutsideEnemyPower(int power) {
        maxOutsideEnemyPower = power;
    }
    public void AddMaxDaytimeEnemyPower(int power) {
        maxDaytimeEnemyPower = power;
    }
    public void AddEnemySpawnChanceThroughoutDay(int value) {
        enemySpawnChanceThroughoutDay += value;
    }
    public void AddOutsideEnemySpawnChanceThroughoutDay(int value) {
        outsideEnemySpawnChanceThroughoutDay += value;
    }
    public void AddDaytimeEnemySpawnChanceThroughoutDay(int value) {
        daytimeEnemySpawnChanceThroughoutDay += value;
    }
    public void AddLandmines(float amount) {
        landmines += amount;
    }
    public void AddTurrets(float amount) {
        turrets += amount;
    }
    public bool SetTimeScale(float value) {
        if (timeScale != 0) {
            throw new Exception("Timescale already set!");
        }
        timeScale = value;
        return true;
    }
    public void ApplyModificationsToLevel() {
        if (!IsTargetLevelSet()) return;
        HullManager.LogBox("APPLYING LEVEL MODIFICATIONS");

        // Enemy mods
        ApplyEnemyComponentRarity(targetLevel, enemyComponentRarityModifications, enemyComponentRarityBackups);
        ApplyEnemyComponentMaxCount(targetLevel, enemyComponentMaxCountModifications, enemyComponentMaxCountBackups);
        ApplyEnemyComponentPower(targetLevel, enemyComponentPowerModifications, enemyComponentPowerBackups);
        ApplyEnemySpawnChanceThroughoutDay(targetLevel, enemySpawnChanceThroughoutDay);

        // Outside enemy mods
        ApplyOutsideEnemyComponentRarity(targetLevel, outsideEnemyComponentRarityModifications, outsideEnemyComponentRarityBackups);
        ApplyOutsideEnemySpawnChanceThroughoutDay(targetLevel, outsideEnemySpawnChanceThroughoutDay);
        
        // Daytime enemy mods
        ApplyDaytimeEnemyComponentRarity(targetLevel, daytimeEnemyComponentRarityModifications, daytimeEnemyComponentRarityBackups);
        ApplyDaytimeEnemySpawnChanceThroughoutDay(targetLevel, daytimeEnemySpawnChanceThroughoutDay);

        // Loot mods
        ApplyLootRarity(targetLevel, spawnableScrapRarityModifications, spawnableScrapRarityBackups);

        // Global mods
        ApplyMaxEnemyPower(targetLevel, maxEnemyPower);
        ApplyMaxOutsideEnemyPower(targetLevel, maxOutsideEnemyPower);
        ApplyMaxDaytimeEnemyPower(targetLevel, maxDaytimeEnemyPower);

        ApplyTimeScale(targetLevel, timeScale);

        // Unit mods
        ApplyUnitModifications(targetLevel, landmines, turrets);

        // Add SceneManager event listener to undo our modifications later
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    public void UndoModifications() {
        if (!IsTargetLevelSet()) return;
        HullManager.LogBox("REVERTING LEVEL MODIFICATIONS");
        Plugin.Mls.LogInfo($"Reverting all modifications to {targetLevel.PlanetName}!");

        // Enemy mods
        ApplyEnemyComponentRarity(targetLevel, enemyComponentRarityModifications, enemyComponentRarityBackups, true);
        ApplyEnemyComponentMaxCount(targetLevel, enemyComponentMaxCountModifications, enemyComponentMaxCountBackups, true);
        ApplyEnemyComponentPower(targetLevel, enemyComponentPowerModifications, enemyComponentPowerBackups, true);
        ApplyEnemySpawnChanceThroughoutDay(targetLevel, enemySpawnChanceThroughoutDay, true);

        // Outside enemy mods
        ApplyOutsideEnemyComponentRarity(targetLevel, outsideEnemyComponentRarityModifications, outsideEnemyComponentRarityBackups, true);
        ApplyOutsideEnemySpawnChanceThroughoutDay(targetLevel, outsideEnemySpawnChanceThroughoutDay, true);

        // Daytime enemy mods
        ApplyDaytimeEnemyComponentRarity(targetLevel, daytimeEnemyComponentRarityModifications, daytimeEnemyComponentRarityBackups, true);
        ApplyDaytimeEnemySpawnChanceThroughoutDay(targetLevel, daytimeEnemySpawnChanceThroughoutDay, true);

        // Loot mods
        ApplyLootRarity(targetLevel, spawnableScrapRarityModifications, spawnableScrapRarityBackups, true);

        // Global mods
        ApplyMaxEnemyPower(targetLevel, maxEnemyPower, true);
        ApplyMaxOutsideEnemyPower(targetLevel, maxOutsideEnemyPower, true);
        ApplyMaxDaytimeEnemyPower(targetLevel, maxDaytimeEnemyPower, true);

        ApplyTimeScale(targetLevel, timeScale, true);

        // Unit mods
        UndoUnitModifications(targetLevel);

        // Remove SceneManager event listener
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    public bool IsEnemySpawnable(string enemyName) {
        if (!IsTargetLevelSet()) return false;
        if (targetLevel.Enemies.Any(unit => unit.enemyType.enemyPrefab.name.Equals(enemyName))) {
            return true;
        } else {
            Plugin.Mls.LogWarning($"Can't spawn enemy {enemyName} on this moon.");
            return false;
        }
    }
    public bool IsOutsideEnemySpawnable(string enemyName) {
        if (!IsTargetLevelSet()) return false;
        if (targetLevel.OutsideEnemies.Any(unit => unit.enemyType.enemyPrefab.name.Equals(enemyName))) {
            return true;
        } else {
            Plugin.Mls.LogWarning($"Can't spawn outside {enemyName} on this moon.");
            return false;
        }
    }
    public bool IsDaytimeEnemySpawnable(string enemyName) {
        if (!IsTargetLevelSet()) return false;
        if (targetLevel.DaytimeEnemies.Any(unit => unit.enemyType.enemyPrefab.name.Equals(enemyName))) {
            return true;
        } else {
            Plugin.Mls.LogWarning($"Can't spawn daytime enemy {enemyName} on this moon.");
            return false;
        }
    }
    public bool IsScrapSpawnable(string itemName) {
        if (!IsTargetLevelSet()) return false;
        if (targetLevel.spawnableScrap.Any(item => item.spawnableItem.itemName.Equals(itemName))) {
            return true;
        } else {
            Plugin.Mls.LogWarning($"Can't spawn scrap {itemName} on this moon.");
            return false;
        }
    }

    public void undoModificationsEarly() {
        SelectableLevel currentLevel = RoundManager.Instance.currentLevel;
        if (currentLevel == null) {
            Plugin.Mls.LogInfo($"Scene does not contain a level!");
            return;
        }
        if (currentLevel.levelID == 3) {
            Plugin.Mls.LogInfo($"Scene contains company building level!");
            return;
        }
        if (currentLevel != targetLevel) {
            Plugin.Mls.LogWarning($"currentLevel ({currentLevel.PlanetName}) levelModifier ({targetLevel.PlanetName}) mismatch!");
            return;
        }
        UndoModifications();
        EventsHandler.Reset();
    }
}