using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class FlowerManEvent : HullEvent
{
    public override string ID() => "FlowerMan";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Increases Bracken (Flowerman) spawn chance. Allows more than one to spawn.";
    public static List<string> MessagesList = new() {
        { "Reports of paranormal activity" },
        { "Paranormal signature detected" },
        { "Something is lurking in the shadows" },
        { "Are you afraid of the dark?" },
        { "Always behind you.." },
        { "White eyes watching you.." },
        { "Sometimes you don't want to win a staring contest!" }
    };
    public static List<string> shortMessagesList = new() {
        { "STALKER" },
        { "FEAR OF THE DARK" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable(EnemyUtil.getEnemyByType(typeof(FlowermanAI)))) {
            return false;
        }
        levelModifier.AddEnemyComponentRarity(EnemyUtil.getEnemyByType(typeof(FlowermanAI)), 100);
        levelModifier.AddEnemyComponentMaxCount(EnemyUtil.getEnemyByType(typeof(FlowermanAI)), 4);
        levelModifier.AddEnemyComponentPower(EnemyUtil.getEnemyByType(typeof(FlowermanAI)), 0);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}