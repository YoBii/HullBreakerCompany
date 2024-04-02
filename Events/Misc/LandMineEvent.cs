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
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        levelModifier.AddLandmines(Plugin.LandmineScale);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}