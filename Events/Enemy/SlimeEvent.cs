using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class SlimeEvent : HullEvent
{
    public SlimeEvent() {
        ID = "Slime";
        Weight = 10;
        Description = "Increases spawn chance of Hygrodere (Blob/Slime) and there's more of them.";
        MessagesList = new List<string>() {
            { "Dominated by hostile life form" },
            { "Dominant species detected" },
            { "Don't get lost in the sauce" },
            { "Shapeless creature detected. Water content 99.9%" }
        };
        shortMessagesList = new List<string>() {
            { "SLIPPERY FLOOR" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable(Util.getEnemyByType(typeof(BlobAI)))) {
            return false;
        }

        levelModifier.AddEnemyComponentRarity(Util.getEnemyByType(typeof(BlobAI)), 100);
        levelModifier.AddEnemyComponentMaxCount(Util.getEnemyByType(typeof(BlobAI)), 5);
        levelModifier.AddEnemyComponentPower(Util.getEnemyByType(typeof(BlobAI)), 0);

        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "red");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}