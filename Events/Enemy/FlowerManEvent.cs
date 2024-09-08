using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class FlowerManEvent : HullEvent {
    public FlowerManEvent() {
        ID = "FlowerMan";
        Weight = 10;
        Description = "Increases Bracken (Flowerman) spawn chance. Allows more than one to spawn.";
        MessagesList = new List<string>() {
            { "Reports of paranormal activity" },
            { "Paranormal signature detected" },
            { "Something is lurking in the shadows" },
            { "Are you afraid of the dark?" },
            { "Always behind you.." },
            { "White eyes watching you.." },
            { "Sometimes you don't want to win a staring contest!" }
        };
        shortMessagesList = new List<string>() {
            { "STALKER" },
            { "FEAR OF THE DARK" }
        };
    }

    public override bool Execute(SelectableLevel level, LevelModifier levelModifier) {
        if (!levelModifier.IsEnemySpawnable(Util.getEnemyByType(typeof(FlowermanAI)))) {
            return false;
        }
        levelModifier.AddEnemyComponentRarity(Util.getEnemyByType(typeof(FlowermanAI)), 100);
        levelModifier.AddEnemyComponentMaxCount(Util.getEnemyByType(typeof(FlowermanAI)), 4);
        levelModifier.AddEnemyComponentPower(Util.getEnemyByType(typeof(FlowermanAI)), 0);
        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "red");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}
