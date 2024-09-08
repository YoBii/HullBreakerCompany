using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class NutcrackerEvent : HullEvent
{
    public NutcrackerEvent() {
        ID = "Nutcracker";
        Weight = 10;
        Description = "Increases spawn chance of Nutcracker.";
        MessagesList = new List<string>() {
            { "Reports of paranormal activity" },
            { "Paranormal signature detected" },
            { "Don't you move.." },
            { "Keep your head down" },
            { "Protect your nuts!" }
        };
        shortMessagesList = new List<string>() {
            { "THIS IS NUTS" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable(Util.getEnemyByType(typeof(NutcrackerEnemyAI)))) {
            return false;
        }

        levelModifier.AddEnemyComponentRarity(Util.getEnemyByType(typeof(NutcrackerEnemyAI)), 100);
        levelModifier.AddEnemyComponentPower(Util.getEnemyByType(typeof(NutcrackerEnemyAI)), 1);

        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "red");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}