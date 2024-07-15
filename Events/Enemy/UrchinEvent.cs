using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;
using UnityEngine.UIElements.Collections;

namespace HullBreakerCompany.Events.Enemy;

public class Urchin : HullEvent
{
    public override string ID() => "Urchin";
    public override int GetWeight() => 20;
    public override string GetDescription() => "Increases Urching spawn frequency. Spawns up to ten early. ";
    public static List<string> MessagesList = new() {
        { "Dominated by hostile life form"},
        { "Dominant species detected"},
        { "Urchin Invasion" },
        { "Exponential growth" },
        { "They will not stop breeding" }
    };
    public static List<string> shortMessagesList = new() {
        { "SPIKY" },
        { "BALLS" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsDaytimeEnemySpawnable("Urchin")) {
            return false;
        }
        levelModifier.AddDaytimeEnemyComponentRarity("Urchin", 10000);
        levelModifier.AddDaytimeEnemyComponentMaxCount("Urchin", 10);
        //levelModifier.AddDaytimeEnemyComponentPower("Urchin", 0);
        levelModifier.AddDaytimeEnemySpawnChanceThroughoutDay(10);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}