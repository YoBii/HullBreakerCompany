using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class BarberEvent : HullEvent
{
    public BarberEvent() {
        ID = "Barber";
        Weight = 5;
        Description = "Increases spawn chance of barber and there's more of them";
        MessagesList = new List<string>() {
            { "Reports of paranormal activity" },
            { "Paranormal signature detected" },
            { "In dire need of a haircut..?"},
            { "Snip-snap" }
        };
        shortMessagesList = new List<string>() {
            { "BARBER" },
            { "SCISSORS" }
        };
    }

    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable(Util.getEnemyByType(typeof(ButlerEnemyAI)))) {
            return false;
        }
        levelModifier.AddEnemyComponentRarity(Util.getEnemyByType(typeof(ButlerEnemyAI)), 100);
        levelModifier.AddEnemyComponentMaxCount(Util.getEnemyByType(typeof(ButlerEnemyAI)), 5);
        levelModifier.AddEnemyComponentPower(Util.getEnemyByType(typeof(ButlerEnemyAI)), 0);
        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "red");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}
