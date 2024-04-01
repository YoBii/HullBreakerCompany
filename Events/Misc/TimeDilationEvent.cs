using System;
using System.Collections.Generic;
using FacilityMeltdown.API;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class TimeDilationEvent : HullEvent
{
    public override string ID() => "TimeDilation";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Makes time pass faster";
    public static List<string> MessagesList = new() {
        { "Gravity distorts space time. You better hurry!" },
        { "Let's go! In and out. Twenty minute adventure." },
        { "Gravitional anomaly nearby! Time passes at a faster rate." }
    };
    public static List<string> shortMessagesList = new() {
        { "TIME-" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        try {
            levelModifier.SetTimeScale(2f);
        } catch (Exception e) {
            Plugin.Mls.LogWarning(e.Message);
            return false;
        }
        HullManager.AddChatEventMessage(this);
        return true;
    }
}