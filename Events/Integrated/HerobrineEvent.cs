using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Integrated;

public class HerobrineEvent : HullEvent
{
    public override string ID() => "Herobrine";
    public override int GetWeight() => 5;
    public override string GetDescription() => "Guarantees Herobrine spawn. Herobrine doesn't reduce the amount of other enemies (doesn't add to global power)";
    public static List<string> MessagesList = new() {
        { "Removed." },
        { "Don't turn around.." }
    };
    public static List<string> shortMessagesList = new() {
        { "HAUNTED" },
        { "REMOVED" }
    };
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable("Herobrine"))
        {
            return false;
        }
        levelModifier.AddEnemyComponentRarity("Herobrine", 33);
        levelModifier.AddEnemyComponentPower("Herobrine", 0);
        levelModifier.AddEnemyComponentMaxCount("Herobrine", 2);
        if (Plugin.ColoredEventMessages)
        {
            HullManager.AddChatEventMessageColored(this, "red");
        }
        else
        {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}