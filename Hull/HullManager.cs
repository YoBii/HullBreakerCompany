using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Event;
using UnityEngine;

namespace HullBreakerCompany.hull;
internal class HullManager : MonoBehaviour
{
    public TimeOfDay timeOfDay;
    
    public void Update()
    {
        if (timeOfDay == null)
        {
            timeOfDay = FindFirstObjectByType<TimeOfDay>();
        }
        else
        {
            timeOfDay.quotaVariables.baseIncrease = 256;
        }
        
    }
    
    public static HullManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void ExecuteAfterDelay(Action action, float delay)
    {
        StartCoroutine(DelayedExecution(action, delay));
    }
    
    public void RepeatingExecute(Action action, float delay, float interval)
    {
        StartCoroutine(RepeatingExecution(action, delay, interval));
    }

    private IEnumerator RepeatingExecution(Action action, float delay, float interval)
    {
        yield return new WaitForSeconds(delay);
        while (true)
        {
            action.Invoke();
            yield return new WaitForSeconds(interval);
        }
    }

    private IEnumerator DelayedExecution(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }
    
    public static void SendChatEventMessage(HullEvent hEvent)
    {
        if (HUDManager.Instance != null && hEvent != null)
        {
            var message = !Plugin.UseShortChatMessages ? hEvent.GetMessage() : hEvent.GetShortMessage();
            
            HUDManager.Instance.AddTextToChatOnServer(message);
        } else {
            Plugin.Mls.LogInfo("Could not find HUDManager instance" +  "\n" + hEvent.GetMessage());
        }
    }
    
    public static void SendChatEventMessage(string message)
    {
        if (HUDManager.Instance != null && message != null)
        {
            HUDManager.Instance.AddTextToChatOnServer(message);
        } else {
            Plugin.Mls.LogInfo("Could not find HUDManager instance" +  "\n" + message);
        }
    }
    
    public static Dictionary<string, SelectableLevel> store = new ();
    public static void SaveLevel(string id, SelectableLevel level)
    {

    SelectableLevel copy = ScriptableObject.CreateInstance<SelectableLevel>();
    copy.name = level.name + " [HULLBREAKER COPY]";
    
    copy.planetPrefab = level.planetPrefab;
    copy.sceneName = level.sceneName;
    copy.levelID = level.levelID;
    copy.lockedForDemo = level.lockedForDemo;
    copy.spawnEnemiesAndScrap = level.spawnEnemiesAndScrap;
    copy.PlanetName = level.PlanetName;
    copy.LevelDescription = level.LevelDescription;
    copy.videoReel = level.videoReel;
    copy.riskLevel = level.riskLevel;
    copy.timeToArrive = level.timeToArrive;
    copy.OffsetFromGlobalTime = level.OffsetFromGlobalTime;
    copy.DaySpeedMultiplier = level.DaySpeedMultiplier;
    copy.planetHasTime = level.planetHasTime;
    copy.randomWeathers = level.randomWeathers.ToArray(); 
    copy.overrideWeather = level.overrideWeather;
    copy.overrideWeatherType = level.overrideWeatherType;
    copy.currentWeather = level.currentWeather;
    copy.factorySizeMultiplier = level.factorySizeMultiplier;
    copy.dungeonFlowTypes = level.dungeonFlowTypes.ToArray(); 
    copy.spawnableMapObjects = level.spawnableMapObjects.ToArray();
    copy.spawnableOutsideObjects = level.spawnableOutsideObjects.ToArray(); 
    copy.spawnableScrap = new List<SpawnableItemWithRarity>(level.spawnableScrap); 
    copy.minScrap = level.minScrap;
    copy.maxScrap = level.maxScrap;
    copy.minTotalScrapValue = level.minTotalScrapValue;
    copy.maxTotalScrapValue = level.maxTotalScrapValue;
    copy.levelAmbienceClips = level.levelAmbienceClips;
    copy.maxEnemyPowerCount = level.maxEnemyPowerCount;
    copy.maxOutsideEnemyPowerCount = level.maxOutsideEnemyPowerCount;
    copy.maxDaytimeEnemyPowerCount = level.maxDaytimeEnemyPowerCount;
    copy.Enemies = new List<SpawnableEnemyWithRarity>(level.Enemies); 
    copy.OutsideEnemies = new List<SpawnableEnemyWithRarity>(level.OutsideEnemies); 
    copy.DaytimeEnemies = new List<SpawnableEnemyWithRarity>(level.DaytimeEnemies); 
    copy.enemySpawnChanceThroughoutDay = new AnimationCurve(level.enemySpawnChanceThroughoutDay.keys); 
    copy.outsideEnemySpawnChanceThroughDay = new AnimationCurve(level.outsideEnemySpawnChanceThroughDay.keys); 
    copy.daytimeEnemySpawnChanceThroughDay = new AnimationCurve(level.daytimeEnemySpawnChanceThroughDay.keys); 
    copy.spawnProbabilityRange = level.spawnProbabilityRange;
    copy.daytimeEnemiesProbabilityRange = level.daytimeEnemiesProbabilityRange;
    copy.levelIncludesSnowFootprints = level.levelIncludesSnowFootprints;
    
    store[id] = copy;
}

    public static SelectableLevel RestoreLevel(string id)
    {
        if (!store.TryGetValue(id, out var level)) return null;
        store.Remove(id);
        if (level != null)
        {
            Destroy(level);
        }
        return level;
    }
}