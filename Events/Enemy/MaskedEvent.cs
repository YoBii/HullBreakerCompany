using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class MaskedEvent : HullEvent
{
    public MaskedEvent() {
        ID = "Masked";
        Weight = 10;
        Description = "Increases spawn chance of Masked.";
        MessagesList = new List<string>() {
            { "Reports of paranormal activity" },
            { "Paranormal signature detected" },
            { "Phantom of the Opera" },
            { "Who's the new guy?" }
        };
        shortMessagesList = new List<string>() {
            { "MASKED" },
            { "TRUST ISSUES" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable(Util.getEnemyByType(typeof(MaskedPlayerEnemy)))) {
            return false;
        }
        levelModifier.AddEnemyComponentRarity(Util.getEnemyByType(typeof(MaskedPlayerEnemy)), 100);
        levelModifier.AddEnemyComponentPower(Util.getEnemyByType(typeof(MaskedPlayerEnemy)), 1);

        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "red");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}