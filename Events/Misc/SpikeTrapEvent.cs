using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class SpikeTrapEvent : HullEvent
{
    public override string ID() => "SpikeTrap";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Spawns additional spike traps inside.";
    public static List<string> MessagesList = new() {
        { "High security compound" },
        { "They rigged this place up"},
        { "They call this place Takeshi's castle"},
        { "Locals call this place Sen's fortress"},
        { "What is that noise?" }
    };
    public static List<string> shortMessagesList = new() {
        { "SPIKES" },
        { "TRAPS" }
    };
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsTrapUnitSpawnable(Util.getTrapUnitByType(typeof(SpikeRoofTrap)))) return false;
        levelModifier.AddTrapUnit(Util.getTrapUnitByType(typeof(SpikeRoofTrap)), Plugin.SpikeTrapScale);
        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "orange");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}