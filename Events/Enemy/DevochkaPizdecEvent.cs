using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class DevochkaPizdecEvent : HullEvent
{
    public override string ID() => "DevochkaPizdec";
    public override int GetWeight() => 5;
    public override string GetDescription() => "Increased chance of phantom girl spawn";
    public static List<string> MessagesList = new() {
        { "Reports of paranormal activity" },
        { "Paranormal signature detected" },
        { "Workers were loosing their mind before they shut this place down. Extract what was left behind." },
        { "Don't loose your head now!" }
    };
    public static List<string> shortMessagesList = new() {
        { "PARANOID" },
        { "PLAYTIME "}
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable(EnemyUtil.getEnemyByType(typeof(DressGirlAI))))
        {
            return false;
        }
        levelModifier.AddEnemyComponentRarity(EnemyUtil.getEnemyByType(typeof(DressGirlAI)), 500);
        levelModifier.AddEnemyComponentMaxCount(EnemyUtil.getEnemyByType(typeof(DressGirlAI)), 4);
        levelModifier.AddEnemyComponentPower(EnemyUtil.getEnemyByType(typeof(DressGirlAI)), 1);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}