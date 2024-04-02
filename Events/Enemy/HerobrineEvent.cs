using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

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
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable("Herobrine")) {
            return false;
        }
        levelModifier.AddEnemyComponentRarity("Herobrine", 1000);
        levelModifier.AddEnemyComponentPower("Herobrine", 1);
        levelModifier.AddMaxEnemyPower(1);

        HullManager.AddChatEventMessage(this);
        return true;
    }
}