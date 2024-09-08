using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;
using UnityEngine.UIElements.Collections;

namespace HullBreakerCompany.Events.Integrated.Surfaced;

public class BruceAlmightyEvent : HullEvent
{
    public BruceAlmightyEvent() {
        ID = "BruceAlmighty";
        Weight = 20;
        Description = "Increases Bruce spawn frequency. Spawns up to three.";
        MessagesList = new List<string>() {
            { "Dominated by hostile life form"},
            { "Dominant species detected"},
            { "Tip: Avoid getting hurt" },
            { "Watch out for sharks!" },
            { "The land animals won't harm you.. unless you present yourself as food" }
        };
        shortMessagesList = new List<string>() {
            { "BRUCE" },
            { "SNACKED" }
        };
    }
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