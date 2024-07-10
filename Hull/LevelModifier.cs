using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HullBreakerCompany.Hull;
using UnityEngine.SceneManagement;

namespace HullBreakerCompany;

public class LevelModifier(SelectableLevel level) {
    private SelectableLevel targetLevel = level;

    private readonly Dictionary<string, int> enemyComponentRarityModifications = [];
    private readonly Dictionary<string, int> enemyComponentRarityBackups = [];
    private readonly Dictionary<string, int> enemyComponentMaxCountModifications = [];
    private readonly Dictionary<string, int> enemyComponentMaxCountBackups = [];
    private readonly Dictionary<string, float> enemyComponentPowerModifications = [];
    private readonly Dictionary<string, float> enemyComponentPowerBackups = [];
    private int targetLevelEnemyRarityTotal = 0;

    private readonly Dictionary<string, int> outsideEnemyComponentRarityModifications = [];
    private readonly Dictionary<string, int> outsideEnemyComponentRarityBackups = [];
    private readonly Dictionary<string, int> outsideEnemyComponentMaxCountModifications = [];
    private readonly Dictionary<string, int> outsideEnemyComponentMaxCountBackups = [];
    private readonly Dictionary<string, float> outsideEnemyComponentPowerModifications = [];
    private readonly Dictionary<string, float> outsideEnemyComponentPowerBackups = [];
    private int targetLevelOutsideEnemyRarityTotal = 0;

    private readonly Dictionary<string, int> daytimeEnemyComponentRarityModifications = [];
    private readonly Dictionary<string, int> daytimeEnemyComponentRarityBackups = [];
    private readonly Dictionary<string, int> daytimeEnemyComponentMaxCountModifications = [];
    private readonly Dictionary<string, int> daytimeEnemyComponentMaxCountBackups = [];
    private readonly Dictionary<string, float> daytimeEnemyComponentPowerModifications = [];
    private readonly Dictionary<string, float> daytimeEnemyComponentPowerBackups = [];
    private int targetLevelDaytimeEnemyRarityTotal = 0;

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
    private AnimationCurve landminesBackup = null;
    private float turrets;
    private AnimationCurve turretsBackup = null;
    private float spiketraps;
    private AnimationCurve spiketrapsBackup = null;

    private readonly Dictionary<string, int> traps = [];
    private readonly Dictionary<string, AnimationCurve> trapBackups = [];

    private bool active { get; } = false;

    private float timeScale = 0;

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

    private void ApplyEnemyComponentRarity(bool undo = false) {
        if (enemyComponentRarityModifications.Count <= 0) return;

        if (targetLevelEnemyRarityTotal == 0) {
            targetLevelEnemyRarityTotal = targetLevel.Enemies.Sum(enemy => enemy.rarity);
        }

        foreach (var rarityPair in enemyComponentRarityModifications) {
            foreach (var enemy in targetLevel.Enemies.Where(enemy => enemy.enemyType.enemyPrefab.name.Equals(rarityPair.Key, StringComparison.OrdinalIgnoreCase))) {
                if (undo) {
                    enemy.rarity = enemyComponentRarityBackups[rarityPair.Key];
                } else {
                    enemyComponentRarityBackups.TryAdd(rarityPair.Key, enemy.rarity);
                    enemy.rarity = (int) Math.Floor((float) rarityPair.Value / 100 * (targetLevelEnemyRarityTotal - enemy.rarity));
                }
                Plugin.Mls.LogInfo($"Setting rarity of {enemy.enemyType.enemyPrefab.name} to {enemy.rarity}");
                break;
            }
        }
    }
    private void ApplyEnemyComponentMaxCount(bool undo = false) {
        if (enemyComponentMaxCountModifications.Count <= 0) return;

        foreach (var maxCountPair in enemyComponentMaxCountModifications) {
            foreach (var enemy in targetLevel.Enemies.Where(enemy => enemy.enemyType.enemyPrefab.name.Equals(maxCountPair.Key, StringComparison.OrdinalIgnoreCase))) {
                if (!undo) enemyComponentMaxCountBackups.TryAdd(maxCountPair.Key, enemy.enemyType.MaxCount);
                enemy.enemyType.MaxCount = undo ? enemyComponentMaxCountBackups[maxCountPair.Key] : maxCountPair.Value;
                Plugin.Mls.LogInfo($"Setting maxCount of {enemy.enemyType.enemyPrefab.name} to {enemy.enemyType.MaxCount}");
                break;
            }
        }
    }
    private void ApplyEnemyComponentPower(bool undo = false) {
        if (enemyComponentPowerModifications.Count <= 0) return;

        foreach (var powerPair in enemyComponentPowerModifications) {
            foreach (var enemy in targetLevel.Enemies.Where(enemy => enemy.enemyType.enemyPrefab.name.Equals(powerPair.Key, StringComparison.OrdinalIgnoreCase))) {
                if (!undo) enemyComponentPowerBackups.TryAdd(powerPair.Key, (float) Math.Round(enemy.enemyType.PowerLevel));
                enemy.enemyType.PowerLevel = undo ? enemyComponentPowerBackups[powerPair.Key] : powerPair.Value;
                Plugin.Mls.LogInfo($"Setting power of {enemy.enemyType.enemyPrefab.name} to {enemy.enemyType.PowerLevel}");
                break;
            }
        }
    }
    private void ApplyOutsideEnemyComponentRarity(bool undo = false) {
        if (outsideEnemyComponentRarityModifications.Count <= 0) return;

        if (targetLevelOutsideEnemyRarityTotal == 0) {
            targetLevelOutsideEnemyRarityTotal = targetLevel.OutsideEnemies.Sum(enemy => enemy.rarity);
        }

        foreach (var rarityPair in outsideEnemyComponentRarityModifications) {
            foreach (var enemy in targetLevel.OutsideEnemies.Where(enemy => enemy.enemyType.enemyPrefab.name.Equals(rarityPair.Key, StringComparison.OrdinalIgnoreCase))) {
                if (undo) {
                    enemy.rarity = outsideEnemyComponentRarityBackups[rarityPair.Key];
                } else {
                    outsideEnemyComponentRarityBackups.TryAdd(rarityPair.Key, enemy.rarity);
                    enemy.rarity = (int) Math.Floor((float) rarityPair.Value / 100 * (targetLevelOutsideEnemyRarityTotal - enemy.rarity));
                }
                Plugin.Mls.LogInfo($"Setting rarity of {enemy.enemyType.enemyPrefab.name} to {enemy.rarity}");
                break;
            }
        }
    }
    private void ApplyOutsideEnemyComponentMaxCount(bool undo = false) {
        if (outsideEnemyComponentMaxCountModifications.Count <= 0) return;
        foreach (var maxCountPair in outsideEnemyComponentMaxCountModifications) {
            foreach (var enemy in targetLevel.OutsideEnemies.Where(enemy => enemy.enemyType.enemyPrefab.name.Equals(maxCountPair.Key, StringComparison.OrdinalIgnoreCase))) {
                if (!undo) outsideEnemyComponentMaxCountBackups.TryAdd(maxCountPair.Key, enemy.enemyType.MaxCount);
                enemy.enemyType.MaxCount = undo ? outsideEnemyComponentMaxCountBackups[maxCountPair.Key] : maxCountPair.Value;
                Plugin.Mls.LogInfo($"Setting maxCount of {enemy.enemyType.enemyPrefab.name} to {enemy.enemyType.MaxCount}");
                break;
            }
        }
    }
    private void ApplyOutsideEnemyComponentPower(bool undo = false) {
        if (outsideEnemyComponentPowerModifications.Count <= 0) return;

        foreach (var powerPair in outsideEnemyComponentPowerModifications) {
            foreach (var enemy in targetLevel.OutsideEnemies.Where(enemy => enemy.enemyType.enemyPrefab.name.Equals(powerPair.Key, StringComparison.OrdinalIgnoreCase))) {
                if (!undo) outsideEnemyComponentPowerBackups.TryAdd(powerPair.Key, (float) Math.Round(enemy.enemyType.PowerLevel));
                enemy.enemyType.PowerLevel = undo ? outsideEnemyComponentPowerBackups[powerPair.Key] : powerPair.Value;
                Plugin.Mls.LogInfo($"Setting power of {enemy.enemyType.enemyPrefab.name} to {enemy.enemyType.PowerLevel}");
                break;
            }
        }
    }
    private void ApplyDaytimeEnemyComponentRarity(bool undo = false) {
        if (daytimeEnemyComponentRarityModifications.Count <= 0) return;
        if (targetLevelDaytimeEnemyRarityTotal == 0) {
            targetLevelDaytimeEnemyRarityTotal = targetLevel.DaytimeEnemies.Sum(enemy => enemy.rarity);
        }
        foreach (var rarityPair in daytimeEnemyComponentRarityModifications) {
            foreach (var enemy in targetLevel.DaytimeEnemies.Where(enemy => enemy.enemyType.enemyPrefab.name.Equals(rarityPair.Key, StringComparison.OrdinalIgnoreCase))) {
                if (undo) {
                    enemy.rarity = daytimeEnemyComponentRarityBackups[rarityPair.Key];
                } else {
                    daytimeEnemyComponentRarityBackups.TryAdd(rarityPair.Key, enemy.rarity);
                    enemy.rarity = (int) Math.Floor((float) rarityPair.Value / 100 * (targetLevelDaytimeEnemyRarityTotal - enemy.rarity));
                }
                Plugin.Mls.LogInfo($"Setting rarity of {enemy.enemyType.enemyPrefab.name} to {enemy.rarity}");
                break;
            }
        }
    }
    private void ApplyDaytimeEnemyComponentMaxCount(bool undo = false) {
        if (daytimeEnemyComponentMaxCountModifications.Count <= 0) return;

        foreach (var maxCountPair in daytimeEnemyComponentMaxCountModifications) {
            foreach (var enemy in targetLevel.DaytimeEnemies.Where(enemy => enemy.enemyType.enemyPrefab.name.Equals(maxCountPair.Key, StringComparison.OrdinalIgnoreCase))) {
                if (!undo) daytimeEnemyComponentMaxCountBackups.TryAdd(maxCountPair.Key, enemy.enemyType.MaxCount);
                enemy.enemyType.MaxCount = undo ? daytimeEnemyComponentMaxCountBackups[maxCountPair.Key] : maxCountPair.Value;
                Plugin.Mls.LogInfo($"Setting maxCount of {enemy.enemyType.enemyPrefab.name} to {enemy.enemyType.MaxCount}");
                break;
            }
        }
    }
    private void ApplyDaytimeEnemyComponentPower(bool undo = false) {
        if (daytimeEnemyComponentPowerModifications.Count <= 0) return;

        foreach (var powerPair in daytimeEnemyComponentPowerModifications) {
            foreach (var enemy in targetLevel.DaytimeEnemies.Where(enemy => enemy.enemyType.enemyPrefab.name.Equals(powerPair.Key, StringComparison.OrdinalIgnoreCase))) {
                if (!undo) daytimeEnemyComponentPowerBackups.TryAdd(powerPair.Key, (float) Math.Round(enemy.enemyType.PowerLevel));
                enemy.enemyType.PowerLevel = undo ? daytimeEnemyComponentPowerBackups[powerPair.Key] : powerPair.Value;
                Plugin.Mls.LogInfo($"Setting power of {enemy.enemyType.enemyPrefab.name} to {enemy.enemyType.PowerLevel}");
                break;
            }
        }
    }
    private void ApplyLootRarity(bool undo = false) {
        if (spawnableScrapRarityModifications.Count <= 0) return;

        if (targetLevelScrapRarityTotal == 0) {
            targetLevelScrapRarityTotal = targetLevel.spawnableScrap.Sum(scrap => scrap.rarity);
        }

        foreach (var rarityPair in spawnableScrapRarityModifications) {
            foreach (var item in targetLevel.spawnableScrap.Where(item => item.spawnableItem.itemName.Equals(rarityPair.Key, StringComparison.OrdinalIgnoreCase))) {
                if (undo) {
                    item.rarity = spawnableScrapRarityBackups[rarityPair.Key];
                } else {
                    spawnableScrapRarityBackups.TryAdd(rarityPair.Key, item.rarity);
                    item.rarity = (int) Math.Floor((float) rarityPair.Value / 100 * (targetLevelScrapRarityTotal - item.rarity));
                }
                Plugin.Mls.LogInfo($"Setting rarity of {item.spawnableItem.itemName} to {item.rarity}");
                break;
            }
        }
    }
    private void ApplyMaxEnemyPower(bool undo = false) {
        if (maxEnemyPower <= 0) return;
        if (!undo) {
            int addPower = maxEnemyPower;
            maxEnemyPower = targetLevel.maxEnemyPowerCount;
            targetLevel.maxEnemyPowerCount += addPower;
        } else {
            targetLevel.maxEnemyPowerCount = maxEnemyPower;
        }
        Plugin.Mls.LogInfo($"Setting global max enemy power to {targetLevel.maxEnemyPowerCount}");
    }
    private void ApplyMaxOutsideEnemyPower(bool undo = false) {
        if (maxOutsideEnemyPower <= 0) return;
        if (!undo) {
            int addPower = maxOutsideEnemyPower;
            maxOutsideEnemyPower = targetLevel.maxOutsideEnemyPowerCount;
            targetLevel.maxOutsideEnemyPowerCount += addPower;
        } else {
            targetLevel.maxOutsideEnemyPowerCount = maxOutsideEnemyPower;
        }
        Plugin.Mls.LogInfo($"Setting global outside max enemy power to {targetLevel.maxOutsideEnemyPowerCount}");
    }
    private void ApplyMaxDaytimeEnemyPower(bool undo = false) {
        if (maxDaytimeEnemyPower <= 0) return;
        if (!undo) {
            int addPower = maxDaytimeEnemyPower;
            maxDaytimeEnemyPower = targetLevel.maxDaytimeEnemyPowerCount;
            targetLevel.maxDaytimeEnemyPowerCount += addPower;
        } else {
            targetLevel.maxDaytimeEnemyPowerCount = maxDaytimeEnemyPower;
        }
        Plugin.Mls.LogInfo($"Setting global daytime max enemy power to {targetLevel.maxDaytimeEnemyPowerCount}");
    }
    private void ApplyEnemySpawnChanceThroughoutDay(bool undo = false) {
        if (this.enemySpawnChanceThroughoutDay <= 0) return;
        if (!undo) {
            enemySpawnChanceThroghoutDayBackup = targetLevel.enemySpawnChanceThroughoutDay;
            targetLevel.enemySpawnChanceThroughoutDay = new AnimationCurve(new Keyframe(0f, enemySpawnChanceThroughoutDay));
            Plugin.Mls.LogInfo($"Setting global enemy spawn chance to {enemySpawnChanceThroughoutDay}");
        } else {
            targetLevel.enemySpawnChanceThroughoutDay = enemySpawnChanceThroghoutDayBackup;
            Plugin.Mls.LogInfo($"Reverting global enemy spawn chance");
        }
    }
    private void ApplyOutsideEnemySpawnChanceThroughoutDay(bool undo = false) {
        if (this.outsideEnemySpawnChanceThroughoutDay <= 0) return;
        if (!undo) {
            outsideEnemySpawnChanceThroghoutDayBackup = targetLevel.outsideEnemySpawnChanceThroughDay;
            targetLevel.outsideEnemySpawnChanceThroughDay = new AnimationCurve(new Keyframe(0f, outsideEnemySpawnChanceThroughoutDay));
            Plugin.Mls.LogInfo($"Setting global outside enemy spawn chance to {outsideEnemySpawnChanceThroughoutDay}");
        } else {
            targetLevel.outsideEnemySpawnChanceThroughDay = outsideEnemySpawnChanceThroghoutDayBackup;
            Plugin.Mls.LogInfo($"Reverting global outside enemy spawn chance");
        }
    }
    private void ApplyDaytimeEnemySpawnChanceThroughoutDay(bool undo = false) {
        if (this.daytimeEnemySpawnChanceThroughoutDay <= 0) return;
        if (!undo) {
            daytimeEnemySpawnChanceThroghoutDayBackup = targetLevel.daytimeEnemySpawnChanceThroughDay;
            targetLevel.daytimeEnemySpawnChanceThroughDay = new AnimationCurve(new Keyframe(0f, daytimeEnemySpawnChanceThroughoutDay));
            Plugin.Mls.LogInfo($"Setting global daytime enemy spawn chance to {daytimeEnemySpawnChanceThroughoutDay}");
        } else {
            targetLevel.daytimeEnemySpawnChanceThroughDay = daytimeEnemySpawnChanceThroghoutDayBackup;
            Plugin.Mls.LogInfo($"Reverting global daytime enemy spawn chance");
        }
    }
    private void ApplyTimeScale(bool undo = false) {  
        if (timeScale == 0) return;
        if (!undo) {
            var backupTime = TimeOfDay.Instance.globalTimeSpeedMultiplier; 
            TimeOfDay.Instance.globalTimeSpeedMultiplier *= timeScale;
            timeScale = backupTime;
            Plugin.Mls.LogInfo($"Setting TimeSpeedMultiplier to {TimeOfDay.Instance.globalTimeSpeedMultiplier}: Timescale {(int) Math.Round(timeScale * 100)}%");
        } else {
            TimeOfDay.Instance.globalTimeSpeedMultiplier = timeScale;
            Plugin.Mls.LogInfo($"Reset TimeSpeedMultiplier: {timeScale}");
        }
    }
    private void ApplyUnitModification<T>(float amount, AnimationCurve backup) {  
        foreach (var mapObject in targetLevel.spawnableMapObjects) {
            if (mapObject.prefabToSpawn.GetComponentInChildren<T>() != null) {
                if (backup == null) {
                    if (amount <= 0) return;
                    backup = mapObject.numberToSpawn;
                    mapObject.numberToSpawn = new AnimationCurve(new Keyframe(0f, amount));
                    Plugin.Mls.LogInfo($"Overriding {mapObject.prefabToSpawn.name} amount: {amount}");
                } else {
                    mapObject.numberToSpawn = backup;
                    backup = null;
                    Plugin.Mls.LogInfo($"Resetting {mapObject.prefabToSpawn.name} amount");
                }
            }
        }
    }
    private void ApplyUnitModification(bool undo = false) {
        foreach (var trap in traps) {
            string readable_name;
            if (trap.Key == Util.getTrapUnitByType(typeof(Turret))) readable_name = "Turret";
            else readable_name = trap.Key;
            foreach (var mapObject in targetLevel.spawnableMapObjects) {
                if (mapObject.prefabToSpawn.name == trap.Key) {
                    if (!undo) {
                        if (trap.Value <= 0) return;
                        trapBackups.Add(trap.Key, mapObject.numberToSpawn);
                        mapObject.numberToSpawn = new AnimationCurve(new Keyframe(0f, (float)trap.Value));
                        Plugin.Mls.LogInfo($"Overriding {readable_name} amount: {(trap.Value)}");
                    } else {
                        if (trapBackups[trap.Key] == null) {
                            Plugin.Mls.LogError($"Backup for {readable_name} ({trap.Key}) not found!");
                            return;
                        }
                        mapObject.numberToSpawn = trapBackups[trap.Key];
                        Plugin.Mls.LogInfo($"Restoring original {readable_name} spawn curve");
                    }
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
    public void AddEnemyComponentPower(string enemy, float power) {
        enemyComponentPowerModifications.TryAdd(enemy, power);
    }
    /// <summary>
    /// Adds an enemy to the levelModifier to modify its rarity.
    /// </summary>
    /// <param name="enemy">The enemyPrefab name</param>
    /// <param name="percentOfTotalRarity">specified as percentage of the levels total enemy rarity</param>
    public void AddOutsideEnemyComponentRarity(string enemy, int percentOfTotalRarity) {
    outsideEnemyComponentRarityModifications.TryAdd(enemy, percentOfTotalRarity);
    }
    public void AddOutsideEnemyComponentMaxCount(string enemy, int maxCount) {
        outsideEnemyComponentMaxCountModifications.TryAdd(enemy, maxCount);
    }
    public void AddOutsideEnemyComponentPower(string enemy, float power) {
        outsideEnemyComponentPowerModifications.TryAdd(enemy, power);
    }
    /// <summary>
    /// Adds an enemy to the levelModifier to modify its rarity.
    /// </summary>
    /// <param name="enemy">The enemyPrefab name</param>
    /// <param name="percentOfTotalRarity">specified as percentage of the levels total enemy rarity</param>
    public void AddDaytimeEnemyComponentRarity(string enemy, int percentOfTotalRarity) {
        daytimeEnemyComponentRarityModifications.TryAdd(enemy, percentOfTotalRarity);
    }
    public void AddDaytimeEnemyComponentMaxCount(string enemy, int maxCount) {
        daytimeEnemyComponentMaxCountModifications.TryAdd(enemy, maxCount);
    }
    public void AddDaytimeEnemyComponentPower(string enemy, float power) {
        daytimeEnemyComponentPowerModifications.TryAdd(enemy, power);
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
    public void AddTrapUnit(string unit, int value) {
        if (traps.ContainsKey(unit)) {
            traps[unit] += value;
            return;
        }
        traps.Add(unit, value);
    }
    public void AddLandmines(float amount) {
        landmines += amount;
    }
    public void AddTurrets(float amount) {
        turrets += amount;
    }
    public void AddSpikeTraps(float amount) {
        spiketraps += amount;
    }
    public bool SetTimeScale(float value) {
        if (timeScale != 0) {
            throw new Exception("Timescale already set!");
        }
        timeScale = value;
        return true;
    }
    public bool IsEnemySpawnable(string enemyName) {
        if (!IsTargetLevelSet()) return false;
        if (targetLevel.Enemies.Any(unit => unit.enemyType.enemyPrefab.name.Equals(enemyName, StringComparison.OrdinalIgnoreCase))) {
            return true;
        } else {
            Plugin.Mls.LogWarning($"Can't spawn enemy {enemyName} on this moon.");
            return false;
        }
    }
    public bool IsOutsideEnemySpawnable(string enemyName) {
        if (!IsTargetLevelSet()) return false;
        if (targetLevel.OutsideEnemies.Any(unit => unit.enemyType.enemyPrefab.name.Equals(enemyName, StringComparison.OrdinalIgnoreCase))) {
            return true;
        } else {
            Plugin.Mls.LogWarning($"Can't spawn outside {enemyName} on this moon.");
            return false;
        }
    }
    public bool IsDaytimeEnemySpawnable(string enemyName) {
        if (!IsTargetLevelSet()) return false;
        if (targetLevel.DaytimeEnemies.Any(unit => unit.enemyType.enemyPrefab.name.Equals(enemyName, StringComparison.OrdinalIgnoreCase))) {
            return true;
        } else {
            Plugin.Mls.LogWarning($"Can't spawn daytime enemy {enemyName} on this moon.");
            return false;
        }
    }
    public bool IsTrapUnitSpawnable(string unit) {
        if (!IsTargetLevelSet()) { return false; }
        if (targetLevel.spawnableMapObjects.Any(mapObject => mapObject.prefabToSpawn.name == unit)) {
            return true;
        } else {
            Plugin.Mls.LogWarning($"Can't spawn {unit} on this moon.");
            return false;
        }
    }
    public bool IsUnitSpawnable<T>() {
        if (!IsTargetLevelSet()) return false;
        if (targetLevel.spawnableMapObjects.Any(mapObject => mapObject.prefabToSpawn.GetComponentInChildren<T>() != null)) {
            return true;
        } else {
            Plugin.Mls.LogWarning($"Can't spawn {nameof(T)} on this moon.");
            return false;
        }
    }
    public bool IsScrapSpawnable(string itemName, bool logging = true) {
        if (!IsTargetLevelSet()) return false;
        if (targetLevel.spawnableScrap.Any(item => item.spawnableItem.itemName.Equals(itemName, StringComparison.OrdinalIgnoreCase))) {
            return true;
        } else if (logging) {
            Plugin.Mls.LogWarning($"Can't spawn scrap {itemName} on this moon.");
        }
        return false;
    }
    public void ApplyModificationsToLevel() {
        if (!IsTargetLevelSet()) return;
        HullManager.LogBox("APPLYING LEVEL MODIFICATIONS");

        // Enemy mods
        ApplyEnemyComponentRarity();
        ApplyEnemyComponentMaxCount();
        ApplyEnemyComponentPower();
        ApplyEnemySpawnChanceThroughoutDay();
        ApplyMaxEnemyPower();

        // Outside enemy mods
        ApplyOutsideEnemyComponentRarity();
        ApplyOutsideEnemyComponentMaxCount();
        ApplyOutsideEnemyComponentPower();
        ApplyOutsideEnemySpawnChanceThroughoutDay();
        ApplyMaxOutsideEnemyPower();
        
        // Daytime enemy mods
        ApplyDaytimeEnemyComponentRarity();
        ApplyDaytimeEnemyComponentMaxCount();
        ApplyDaytimeEnemyComponentPower();
        ApplyDaytimeEnemySpawnChanceThroughoutDay();
        ApplyMaxDaytimeEnemyPower();

        // Loot mods
        ApplyLootRarity();

        // Global mods
        ApplyTimeScale();

        // Unit mods
        //ApplyUnitModification<Landmine>(landmines, landminesBackup);
        //ApplyUnitModification<Turret>(turrets, turretsBackup);
        //ApplyUnitModification<SpikeRoofTrap>(spiketraps, spiketrapsBackup);
        ApplyUnitModification();

        // Add SceneManager event listener to undo our modifications later
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    public void UndoModifications() {
        if (!IsTargetLevelSet()) return;
        HullManager.LogBox("REVERTING LEVEL MODIFICATIONS");
        Plugin.Mls.LogInfo($"Reverting all modifications to {targetLevel.PlanetName}!");

        // Enemy mods
        ApplyEnemyComponentRarity(true);
        ApplyEnemyComponentMaxCount(true);
        ApplyEnemyComponentPower(true);
        ApplyEnemySpawnChanceThroughoutDay(true);
        ApplyMaxEnemyPower(true);

        // Outside enemy mods
        ApplyOutsideEnemyComponentRarity(true);
        ApplyOutsideEnemyComponentMaxCount(true);
        ApplyOutsideEnemyComponentPower(true);
        ApplyOutsideEnemySpawnChanceThroughoutDay(true);
        ApplyMaxOutsideEnemyPower(true);

        // Daytime enemy mods
        ApplyDaytimeEnemyComponentRarity(true);
        ApplyDaytimeEnemyComponentMaxCount(true);
        ApplyDaytimeEnemyComponentPower(true);
        ApplyDaytimeEnemySpawnChanceThroughoutDay(true);
        ApplyMaxDaytimeEnemyPower(true);

        // Loot mods
        ApplyLootRarity(true);

        // Global mods

        ApplyTimeScale(true);

        // Unit mods
        //ApplyUnitModification<Landmine>(landmines, landminesBackup);
        //ApplyUnitModification<Turret>(turrets, turretsBackup);
        //ApplyUnitModification<SpikeRoofTrap>(spiketraps, spiketrapsBackup);
        ApplyUnitModification(true);


        // Remove SceneManager event listener
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    public void UndoModificationsEarly() {
        SelectableLevel currentLevel = RoundManager.Instance.currentLevel;
        if (currentLevel == null) {
            Plugin.Mls.LogWarning($"Trying to revert modifications when current level is null!");
            return;
        }
        if (currentLevel.levelID == 3) {
            Plugin.Mls.LogWarning($"Trying to revert modifications but current level is company building!");
            return;
        }
        if (currentLevel != targetLevel) {
            Plugin.Mls.LogWarning($"Tyring to undo modifications but currentLevel ({currentLevel.PlanetName}) levelModifier ({targetLevel.PlanetName}) don't match!");
            return;
        }
        UndoModifications();
        EventsHandler.Reset();
    }
}