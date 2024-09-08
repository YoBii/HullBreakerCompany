using System;
using System.Collections.Generic;
using FacilityMeltdown.API;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class TimeAnomalyEvent : HullEvent
{
    public TimeAnomalyEvent() {
        ID = "TimeAnomaly";
        Weight = 10;
        Description = "Makes day shorter i.e. time passes faster";
        MessagesList = new List<string>() {
            { "Gravity distorts space time. You better hurry!" },
            { "Let's go! In and out. Twenty minute adventure." },
            { "Gravitational anomaly nearby! Time passes at a faster rate." }
        };
        shortMessagesList = new List<string>() {
            { "TIME-" }
        };
    }
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