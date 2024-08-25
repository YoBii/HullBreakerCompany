using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;
using UnityEngine.UIElements.Collections;

namespace HullBreakerCompany.Events.Integrated.Surfaced;

public class BruceAlmightyEvent : HullEvent
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
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsOutsideEnemySpawnable("Bruce"))
        {
            return false;
        }
        levelModifier.AddOutsideEnemyComponentRarity("Bruce", 1000);
        levelModifier.AddOutsideEnemyComponentMaxCount("Bruce", 3);
        levelModifier.AddOutsideEnemyComponentPower("Bruce", 0);
        levelModifier.AddOutsideEnemySpawnChanceThroughoutDay(64);
        if (Plugin.ColoredEventMessages)
        {
            HullManager.AddChatEventMessageColored(this, "red");
        }
        else
        {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}