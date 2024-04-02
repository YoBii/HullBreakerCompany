using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class NothingEvent : HullEvent
{
    public override string ID() => "Nothing";
    public override int GetWeight() => 100;
    public override string GetDescription() => "Nothing happens - This event means there is no event.\nAdjust weight to set overall chance of getting an event vs. not getting an event for each time an event is randomly selected.";

    public static List<string> MessagesList = new()
    {
        { "<color=white>...</color>" },
        { "<color=white>---</color>" },
        { "<color=white>   </color>" }
    };
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        // simply omit the message
        // HullManager.SendChatEventMessage(this);
        return true;
    }
}