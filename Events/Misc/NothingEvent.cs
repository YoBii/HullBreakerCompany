using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class NothingEvent : HullEvent
{
    public NothingEvent() {
        ID = "Nothing";
        Weight = 100;
        Description = "Nothing happens - This event means there is no event.\nAdjust weight to set overall chance of getting an event vs. not getting an event for each time an event is randomly selected.";

        MessagesList = new List<string>() {
            { "<color=white>...</color>" },
            { "<color=white>---</color>" },
            { "<color=white>   </color>" }
        };
    }
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        // simply omit the message
        // HullManager.SendChatEventMessage(this);
        return true;
    }
}