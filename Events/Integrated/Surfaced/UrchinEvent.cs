using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;
using UnityEngine.UIElements.Collections;

namespace HullBreakerCompany.Events.Integrated.Surfaced;

public class UrchinEvent : HullEvent
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
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsDaytimeEnemySpawnable("Urchin"))
        {
            return false;
        }
        levelModifier.AddDaytimeEnemyComponentRarity("Urchin", 10000);
        levelModifier.AddDaytimeEnemyComponentMaxCount("Urchin", 10);
        levelModifier.AddDaytimeEnemySpawnChanceThroughoutDay(10);
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