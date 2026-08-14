using HullBreakerCompany.Hull;
using System.Collections.Generic;

namespace HullBreakerCompany.Events.Misc;

public class NothingEvent : HullEvent
{
    public NothingEvent()
    {
        ID = "Nothing";
        Weight = 100;
        Description = "Nothing happens - This event means there is no event.\nAdjust weight to set overall chance of getting an event vs. not getting an event for each time an event is randomly selected.";

        MessagesList = new List<string>() {
            { "<color=white>...</color>" },
            { "<color=white>---</color>" },
            { "<color=white>   </color>" }
        };
    }
    public override string GetMessage() => GetActiveMessages()[UnityEngine.Random.Range(0, GetActiveMessages().Count)];
    public override string GetShortMessage() => GetActiveMessages()[UnityEngine.Random.Range(0, GetActiveMessages().Count)];
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        // simply omit the message
        // HullManager.SendChatEventMessage(this);
        return true;
    }
}