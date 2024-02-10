using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HullBreakerCompany.Hull;

public class HullManager : MonoBehaviour
{
    public TimeOfDay timeOfDay;
    public static List<string> chatMessages = new();

    public void Update()
    {
        if (timeOfDay == null)
        {
            timeOfDay = FindFirstObjectByType<TimeOfDay>();
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
    
    public void AddMoney(int amount)
    {
        Terminal tl = FindObjectOfType<Terminal>();
        tl.groupCredits += amount;
        tl.SyncGroupCreditsServerRpc(tl.groupCredits, tl.numberOfItemsInDropship);
    }
    public void ExecuteAfterDelay(Action action, float delay)
    {
        StartCoroutine(DelayedExecution(action, delay));
    }

    private IEnumerator DelayedExecution(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }

    public static void AddChatEventMessage(HullEvent hullEvent)
    {
        if (HUDManager.Instance != null && hullEvent != null && Plugin.EnableEventMessages)
        {
            chatMessages.Add(Plugin.UseShortChatMessages
                ? hullEvent.GetShortMessage()
                : hullEvent.GetMessage());
        }
    }
    public static void AddChatEventMessage(string message, bool addAsFirst = false)
    {
        if (HUDManager.Instance != null && message != null && Plugin.EnableEventMessages)
        {
            if(!addAsFirst) {
                chatMessages.Add(message);
            } else {
                chatMessages.Insert(0, message);
            }
        }
    }
    public static void SendChatEventMessage(string message)
    {
        if (HUDManager.Instance != null && message != null && Plugin.EnableEventMessages)
        {
            HUDManager.Instance.AddTextToChatOnServer(message);
        }
    }
    public static void SendChatEventMessages() {
        if (HUDManager.Instance != null && Plugin.EnableEventMessages) {
            foreach(string message in chatMessages) {
                HUDManager.Instance.AddTextToChatOnServer(message);
            }
            chatMessages.Clear();
        }
    }
    public static void LogEnemyRarity(List<SpawnableEnemyWithRarity> enemies, string title)
    {
        Plugin.Mls.LogInfo("");
        Plugin.Mls.LogInfo(title);
        foreach (var unit in enemies)
        {
            Plugin.Mls.LogInfo($"{unit.enemyType.enemyPrefab.name} - {unit.rarity}");
        }
    }
}