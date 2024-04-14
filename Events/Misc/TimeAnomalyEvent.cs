using System;
using System.Collections.Generic;
using FacilityMeltdown.API;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class TimeAnomalyEvent : HullEvent
{
    public override string ID() => "TimeAnomaly";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Makes day shorter i.e. time passes faster";
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
            System.Random rnd = new();
            levelModifier.SetTimeScale((float) Math.Round(UnityEngine.Random.Range(1.5f, 2f), 2));
        } catch (Exception e) {
            Plugin.Mls.LogWarning(e.Message);
            return false;
        }
        HullManager.AddChatEventMessage(this);
        return true;
    }
}