using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class ArachnophobiaEvent : HullEvent
{
    public override string ID() => "Arachnophobia";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Increases bunker spider (SandSpider) spawn chance. Allows more than one to spawn.";
    public static List<string> MessagesList = new() {
        { "Dominated by hostile life form"},
        { "Dominant species detected"},
        { "This place is crawling.." },
        { "They have 8 eyes.." },
        { "Natural spider habitat" }
    };
    public static List<string> shortMessagesList = new() {
        { "EIGHT EYES" },
        { "EIGHT LEGS" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if(!levelModifier.IsEnemySpawnable(EnemyUtil.getEnemyByType(typeof(SandSpiderAI)))) {
            return false;
        }
        levelModifier.AddEnemyComponentRarity(EnemyUtil.getEnemyByType(typeof(SandSpiderAI)), 100);
        levelModifier.AddEnemyComponentMaxCount(EnemyUtil.getEnemyByType(typeof(SandSpiderAI)), 5);
        levelModifier.AddEnemyComponentPower(EnemyUtil.getEnemyByType(typeof(SandSpiderAI)), 1);
        levelModifier.AddMaxEnemyPower(4);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}