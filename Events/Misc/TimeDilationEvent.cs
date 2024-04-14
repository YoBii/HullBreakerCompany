using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class TimeDilationEvent : HullEvent
{
    public override string ID() => "TimeDilation";
    public override int GetWeight() => 5;
    public override string GetDescription() => "Makes day longer i.e. time passes slower";
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