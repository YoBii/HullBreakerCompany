using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;
using UnityEngine.UIElements.Collections;

namespace HullBreakerCompany.Events.Enemy;

public class BruceAlmighty : HullEvent
{
    public override string ID() => "BruceAlmighty";
    public override int GetWeight() => 20;
    public override string GetDescription() => "Increases Bruce spawn frequency. Spawns up to three. ";
    public static List<string> MessagesList = new() {
        { "Dominated by hostile life form"},
        { "Dominant species detected"},
        { "Tip: Avoid getting hurt" },
        { "Watch out for sharks!" },
        { "The land animals shouldn't harm you.. unless you present yourself as food" }
    };
    public static List<string> shortMessagesList = new() {
        { "BRUCE" },
        { "SNACKED" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsOutsideEnemySpawnable("Bruce")) {
            return false;
        }
        levelModifier.AddOutsideEnemyComponentRarity("Bruce", 1000);
        levelModifier.AddOutsideEnemyComponentMaxCount("Bruce", 3);
        levelModifier.AddOutsideEnemyComponentPower("Bruce", 0);
        levelModifier.AddOutsideEnemySpawnChanceThroughoutDay(64);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}