using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class SeaMineEvent : HullEvent
{
    public override string ID() => "SeaMine";
    public override int GetWeight() => 15;
    public override string GetDescription() => "Spawns additional seamines inside. Uses LandmineScale config.";
    public static List<string> MessagesList = new() {
        { "High security compound" },
        { "They rigged this place up"},
        { "Watch your head!"},
        { "Expect mines" },
        { "Lots of mines might block your path" }
    };
    public static List<string> shortMessagesList = new() {
        { "SEAMINES" }
    };
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsTrapUnitSpawnable("Old Seamine")) return false;
        levelModifier.AddTrapUnit("Old Seamine", Plugin.LandmineScale);
        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "red");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}