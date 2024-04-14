using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void Start()
    {
        Plugin.Mls.LogDebug("Start: HullManager");
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
        if (HUDManager.Instance != null && Plugin.EnableEventMessages && chatMessages.Count > 0) {
            AddChatEventMessage("<color=red>NOTES ABOUT MOON:</color>", true);
            foreach (string message in chatMessages) {
                HUDManager.Instance.AddTextToChatOnServer(message);
            }
        }
        chatMessages.Clear();
    }
    public static void LogEnemies(List<SpawnableEnemyWithRarity> enemies, string title) {
        //Plugin.Mls.LogInfo("\u2581\u2582\u2583\u2584\u2585\u2586\u2587\u2588" + title + "\u2588\u2587\u2586\u2585\u2584\u2583\u2582\u2581");
        LogBoxHeader(title);
        
        Plugin.Mls.LogInfo(String.Format("╠{0, -24}╦{1, 12}╤{2, 12}╤{3, 12}╗", new string('\u2550', 23).Insert(title.Length + 2, "╩"), new string('\u2550', 12), new string('\u2550', 12), new string('\u2550', 12)));
        Plugin.Mls.LogInfo(string.Format("║ {0, -22} ║ {1, 10} │ {2, 10} │ {3, 10} ║", "Enemy", "Rarity", "MaxCount", "Power"));
        Plugin.Mls.LogInfo(String.Format("\u2560{0, -24}\u256c{1, 12}╪{2, 12}╪{3, 12}╣", new string ('\u2550', 24), new string('\u2550', 12), new string('\u2550', 12), new string('\u2550', 12)));
        
        var raritySum = 0;
        foreach (var unit in enemies) {
            try {
                Plugin.Mls.LogInfo(String.Format("║ {0, -22} ║ {1, 10} │ {2, 10} │ {3, 10} ║", unit.enemyType.enemyPrefab.name, unit.rarity, unit.enemyType.MaxCount, unit.enemyType.PowerLevel));
                raritySum += unit.rarity;

            } catch (Exception ex) {
                Plugin.Mls.LogError(ex.Message);
            }
        }
        Plugin.Mls.LogInfo(String.Format("\u2560{0, -24}\u256c{1, 12}╪{2, 12}╪{3, 12}╣", new string('\u2550', 24), new string('\u2550', 12), new string('\u2550', 12), new string('\u2550', 12)));
        Plugin.Mls.LogInfo(String.Format("║ {0, -22} ║ {1, 10} │ {2, 10} │ {3, 10} ║", "Sum", raritySum, "", ""));
        Plugin.Mls.LogInfo(String.Format("╚{0, -24}╩{1, 12}╧{2, 12}╧{3, 12}╝", new string('\u2550', 24), new string('\u2550', 12), new string('\u2550', 12), new string('\u2550', 12)));
    }
    public static void LogScrapRarity(List<SpawnableItemWithRarity> loot, string title) {
        //Plugin.Mls.LogInfo("\u2581\u2582\u2583\u2584\u2585\u2586\u2587\u2588" + title + "\u2588\u2587\u2586\u2585\u2584\u2583\u2582\u2581");
        LogBoxHeader(title);

        Plugin.Mls.LogInfo(String.Format("╠{0, -30}╤{1, 12}╗", new string('\u2550', 29).Insert(title.Length + 2, "╩"), new string('\u2550', 12)));
        Plugin.Mls.LogInfo(String.Format("║ {0, -28} │ {1, 10} ║", "Scrap item", "Rarity"));
        Plugin.Mls.LogInfo(String.Format("╠{0, -30}╪{1, 12}╣", new string('\u2550', 30), new string('\u2550', 12)));

        var raritySum = 0;
        foreach  (var item in loot) {
            if (item == null) continue;
            Plugin.Mls.LogInfo(String.Format("║ {0, -28} │ {1, 10} ║", item.spawnableItem.itemName, item.rarity));
            raritySum += item.rarity;
        }
        Plugin.Mls.LogInfo(String.Format("╠{0, -30}╪{1, 12}╣", new string('\u2550', 30), new string('\u2550', 12)));
        Plugin.Mls.LogInfo(String.Format("║ {0, -28} │ {1, 10} ║", "Sum", raritySum));
        Plugin.Mls.LogInfo(String.Format("╚{0, -30}╧{1, 12}╝", new string('\u2550', 30), new string('\u2550', 12)));

    }
    public static void LogBox(string title) {
        Plugin.Mls.LogInfo("╔" + new string('\u2550', title.Length + 2) + "╗");
        Plugin.Mls.LogInfo("║ " + title + " ║");
        Plugin.Mls.LogInfo("╚" + new string('\u2550', title.Length + 2) + "╝");
    }
    private static void LogBoxHeader(string title) {
        Plugin.Mls.LogInfo("╔" + new string('\u2550', title.Length + 2) + "╗");
        Plugin.Mls.LogInfo("║ " + title + " ║");
    }
}