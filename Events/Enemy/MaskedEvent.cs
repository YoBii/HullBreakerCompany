using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class MaskedEvent : HullEvent
{
    public override string ID() => "Masked";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Increases spawn chance of Masked.";
    public static List<string> MessagesList = new() {
        { "Reports of paranormal activity" },
        { "Paranormal signature detected" },
        { "Phantom of the Opera" },
        { "Who's the new guy?"}
    };
    public static List<string> shortMessagesList = new() {
        { "MASKED" },
        { "TRUST ISSUES" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable(EnemyUtil.getEnemyByType(typeof(MaskedPlayerEnemy)))) {
            return false;
        }
        levelModifier.AddEnemyComponentRarity(EnemyUtil.getEnemyByType(typeof(MaskedPlayerEnemy)), 500);
        levelModifier.AddEnemyComponentPower(EnemyUtil.getEnemyByType(typeof(MaskedPlayerEnemy)), 1);

        HullManager.AddChatEventMessage(this);
        return true;
    }
}