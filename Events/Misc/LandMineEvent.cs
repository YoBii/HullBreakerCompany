using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class LandMineEvent : HullEvent
{
    public override string ID() => "LandMine";
    public override int GetWeight() => 30;
    public override string GetDescription() => "Spawns additional landmines inside.";
    public static List<string> MessagesList = new() {
        { "High security compound" },
        { "They rigged this place up"},
        { "Watch your step!"},
        { "Expect mines" },
        { "Lots of mines might block your path" }
    };
    public static List<string> shortMessagesList = new() {
        { "LANDMINES" }
    };
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsTrapUnitSpawnable(Util.getTrapUnitByType(typeof(Landmine)))) return false;
        levelModifier.AddTrapUnit(Util.getTrapUnitByType(typeof(Landmine)), Plugin.LandmineScale);
        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "red");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}