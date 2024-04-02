using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class ButlerEvent : HullEvent
{
    public override string ID() => "Butler";
    public override int GetWeight() => 10;
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
        return false; // until v50 releases in stable
        //if (!levelModifier.IsEnemySpawnable(EnemyUtil.getEnemyByType(typeof(ButlerEnemyAI)))) {
        //    return false;
        //}
        //levelModifier.AddEnemyComponentRarity(EnemyUtil.getEnemyByType(typeof(ButlerEnemyAI)), 500);
        //levelModifier.AddEnemyComponentMaxCount(EnemyUtil.getEnemyByType(typeof(ButlerEnemyAI)), 4);
        //levelModifier.AddEnemyComponentPower(EnemyUtil.getEnemyByType(typeof(ButlerEnemyAI)), 1);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}