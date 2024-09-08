using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class TimeDilationEvent : HullEvent
{
    public TimeDilationEvent() {
        ID = "TimeDilation";
        Weight = 5;
        Description = "Makes day longer i.e. time passes slower";
        MessagesList = new List<string>() {
            { "Gravity distorts space time. Take your time!" },
            { "You have all the time you need." },
            { "Gravitational anomaly distorts spacetime! Time passes at a slower rate." }
        };
        shortMessagesList = new List<string>() {
            { "TIME+" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        try {
            Random rnd = new Random();
            levelModifier.SetTimeScale((float) Math.Round(UnityEngine.Random.Range(0.5f, 0.75f), 2));
        } catch (Exception e) {
            Plugin.Mls.LogWarning(e.Message);
            return false;
        }
        HullManager.AddChatEventMessage(this);
        return true;
    }
}