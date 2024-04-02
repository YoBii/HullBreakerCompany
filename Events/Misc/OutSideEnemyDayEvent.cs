using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;
using UnityEngine;

namespace HullBreakerCompany.Events.Misc;

public class OutSideEnemyDayEvent : HullEvent
{
    public override string ID() => "OutSideEnemyDay";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Increased amount of outside enemies during daytime. Eclipsed without eclipse.";
    public static List<string> MessagesList = new() {
        { "This place was shrouded in darkness for weeks" },
        { "A prolonged eclipse has disrupted wild life's circadian rhythm" },
        { "Highly populated surface area" },
        { "Due to frequent weather events wildlife roams the surface during daytime" }
    };
    public static List<string> shortMessagesList = new() {
        { "SILENCE SEASON" },
        { "OUTSIDE PARTY" },
        { "ECLIPSED?" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        levelModifier.AddOutsideEnemySpawnChanceThroughoutDay(512);

        HullManager.AddChatEventMessage(this);
        return true;
    }
}