using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;
using UnityEngine.UIElements.Collections;

namespace HullBreakerCompany.Events.Enemy;

public class BeeEvent : HullEvent
{
    public override string ID() => "Bee";
    public override int GetWeight() => 20;
    public override string GetDescription() => "Increases bee hive spawns outside";
    public static List<string> MessagesList = new() {
        { "Dominated by hostile life form"},
        { "Dominant species detected"},
        { "Sticky golden goodness" },
        { "Extract the bee hives!" },
        { "Don't get stung by bees!" }
    };
    public static List<string> shortMessagesList = new() {
        { "BZZZZZ" },
        { "STICKY GOLD" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsDaytimeEnemySpawnable(Util.getEnemyByType(typeof(RedLocustBees)))) {
            return false;
        }
        levelModifier.AddDaytimeEnemyComponentRarity(Util.getEnemyByType(typeof(RedLocustBees)), 1000);
        levelModifier.AddDaytimeEnemyComponentPower(Util.getEnemyByType(typeof(RedLocustBees)), 0);
        levelModifier.AddDaytimeEnemyComponentMaxCount(Util.getEnemyByType(typeof(RedLocustBees)), 10);
        levelModifier.AddDaytimeEnemySpawnChanceThroughoutDay(32);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}