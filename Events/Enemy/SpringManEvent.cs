using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class SpringManEvent : HullEvent
{
    public override string ID() => "SpringMan";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Increases spawn chance of Coil-Head (SpringMan)";
    public static List<string> MessagesList = new() {
        { "Reports of paranormal activity" },
        { "Paranormal signature detected" },
        { "They'll only kill you when you're not looking" },
        { "Don't loose your head now!" },
        { "Now this is a staring contest you do not want to loose!" }
    };
    public static List<string> shortMessagesList = new() {
        { "SPRING" },
        { "BOING!" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable(EnemyUtil.getEnemyByType(typeof(SpringManAI)))) {
            return false;
        }
        levelModifier.AddEnemyComponentRarity(EnemyUtil.getEnemyByType(typeof(SpringManAI)), 100);
        levelModifier.AddEnemyComponentMaxCount(EnemyUtil.getEnemyByType(typeof(SpringManAI)), 5);
        levelModifier.AddEnemyComponentPower(EnemyUtil.getEnemyByType(typeof(SpringManAI)), 1);
        levelModifier.AddMaxEnemyPower(5);

        HullManager.AddChatEventMessage(this);
        return true;
    }
}