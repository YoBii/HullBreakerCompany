using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Integrated;

public class HerobrineEvent : HullEvent
{
    public HerobrineEvent() {
        ID = "Herobrine";
        Weight = 5;
        Description = "Guarantees Herobrine spawn. Herobrine doesn't reduce the amount of other enemies (doesn't add to global power)";
        MessagesList = new List<string>() {
            { "Removed." },
            { "Don't turn around.." }
        };
        shortMessagesList = new List<string>() {
            { "HAUNTED" },
            { "REMOVED" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable("Herobrine"))
        {
            return false;
        }
        levelModifier.AddEnemyComponentRarity("Herobrine", 33);
        levelModifier.AddEnemyComponentPower("Herobrine", 0);
        levelModifier.AddEnemyComponentMaxCount("Herobrine", 2);
        if (Plugin.ColoredEventMessages)
        {
            HullManager.AddChatEventMessageColored(this, "red");
        }
        else
        {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}