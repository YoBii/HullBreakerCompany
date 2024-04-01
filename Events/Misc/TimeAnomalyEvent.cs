using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class TimeAnomaly : HullEvent
{
    public override string ID() => "TimeAnomaly";
    public override int GetWeight() => 5;
    public override string GetDescription() => "Makes day longer / time pass slower";
    public static List<string> MessagesList = new() {
        { "Gravity distorts space time. Take your time!" },
        { "You have all the time you need." },
        { "Gravitional anomaly distorts spacetime! Time passes at a slower rate." }
    };
    public static List<string> shortMessagesList = new() {
        { "TIME+" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        try {
            levelModifier.SetTimeScale(0.5f);
        } catch (Exception e) {
            Plugin.Mls.LogWarning(e.Message);
            return false;
        }
        HullManager.AddChatEventMessage(this);
        return true;
    }
}