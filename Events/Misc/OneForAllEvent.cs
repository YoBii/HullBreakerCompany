using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class OneForAllEvent : HullEvent
{
    public override string ID() => "OneForAll";
    public override int GetWeight() => 10;
    public override string GetDescription() => "The ship will leave within two hours after one of the workers has died.";
    public override string GetMessage() => "<color=white>You have been selected for team building measures!</color>";
    public override string GetShortMessage() => "<color=red>ONE FOR ALL</color>";
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