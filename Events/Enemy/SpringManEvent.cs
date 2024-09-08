using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class SpringManEvent : HullEvent
{
    public SpringManEvent() {
        ID = "SpringMan";
        Weight = 10;
        Description = "Increases spawn chance of Coil-Head (SpringMan)";
        MessagesList = new List<string>() {
            { "Reports of paranormal activity" },
            { "Paranormal signature detected" },
            { "They'll only kill you when you're not looking" },
            { "Don't lose your head now!" },
            { "Now this is a staring contest you do not want to lose!" }
        };
        shortMessagesList = new List<string>() {
            { "SPRING" },
            { "BOING!" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable(Util.getEnemyByType(typeof(SpringManAI)))) {
            return false;
        }
        levelModifier.AddEnemyComponentRarity(Util.getEnemyByType(typeof(SpringManAI)), 100);
        levelModifier.AddEnemyComponentMaxCount(Util.getEnemyByType(typeof(SpringManAI)), 5);
        levelModifier.AddEnemyComponentPower(Util.getEnemyByType(typeof(SpringManAI)), 0);

        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "red");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}