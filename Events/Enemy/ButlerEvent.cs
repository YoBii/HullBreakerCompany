using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class ButlerEvent : HullEvent
{
    public override string ID() => "Butler";
    public override int GetWeight() => 5;
    public override string GetDescription() => "Increases spawn chance of butler and there's more of them";
    public static List<string> MessagesList = new() {
        { "Reports of paranormal activity" },
        { "Paranormal signature detected" },
        { "Someone is taking care of this place" },
    };
    public static List<string> shortMessagesList = new() {
        { "BUTLER" },
        { "JANITOR" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable(Util.getEnemyByType(typeof(ButlerEnemyAI)))) {
            return false;
        }
        levelModifier.AddEnemyComponentRarity(Util.getEnemyByType(typeof(ButlerEnemyAI)), 100);
        levelModifier.AddEnemyComponentMaxCount(Util.getEnemyByType(typeof(ButlerEnemyAI)), 4);
        levelModifier.AddEnemyComponentPower(Util.getEnemyByType(typeof(ButlerEnemyAI)), 0);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}