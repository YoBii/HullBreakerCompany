using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class OneForAllEvent : HullEvent
{
    public OneForAllEvent() {
        ID = "OneForAll";
        Weight = 10;
        Description = "The ship will leave within two hours after one of the workers has died.";
        MessagesList = new List<string>() {
            { "<color=white>You have been selected for team building measures!</color>" }
        };
        shortMessagesList = new List<string>() {
            { "<color=red>ONE FOR ALL</color>" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        EventsHandler.OneForAllIsActive = true;
        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "red");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}