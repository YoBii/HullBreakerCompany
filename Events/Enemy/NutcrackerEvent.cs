using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class NutcrackerEvent : HullEvent
{
    public override string ID() => "Nutcracker";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Increases spawn chance of Nutcracker.";
    public static List<string> MessagesList = new() {
        { "Reports of paranormal activity" },
        { "Paranormal signature detected" },
        { "Don't you move.." },
        { "Keep your head down" },
        { "Protect your nuts!" }
    };
    public static List<string> shortMessagesList = new() {
        { "THIS IS NUTS" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable(EnemyUtil.getEnemyByType(typeof(NutcrackerEnemyAI)))) {
            return false;
        }

        levelModifier.AddEnemyComponentRarity(EnemyUtil.getEnemyByType(typeof(NutcrackerEnemyAI)), 100);
        levelModifier.AddEnemyComponentPower(EnemyUtil.getEnemyByType(typeof(NutcrackerEnemyAI)), 1);

        HullManager.AddChatEventMessage(this);
        return true;
    }
}