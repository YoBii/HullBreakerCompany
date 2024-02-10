using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class SpringManEvent : HullEvent
{
    public override string ID() => "SpringMan";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Increased chance of spring man spawning (coil-head)";
    public static List<String> MessagesList = new() {
        { "Detected signs of paranormal activity" },
        { "Several anomalies detected" },
        { "Something's moving but not alive.." },
        { "Don't loose your head now!" },
        { "Build awareness, never ignore what's coming staright at you!" },
        { "Keep eye contact or you'll loose it!" }
    };
    public static List<String> shortMessagesList = new() {
        { "PARANORMAL" },
        { "HAUNTED" },
        { "BOING!" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        if (level.Enemies.All(unit => unit.enemyType.enemyPrefab.GetComponent<SpringManAI>() == null)) {
            Plugin.Mls.LogWarning($"Can't spawn SpringManAI on this moon.");
            return false;
        }
        
        enemyComponentRarity.Add(typeof(SpringManAI), 256);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}