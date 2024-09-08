using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class LizardsEvent : HullEvent
{
    public LizardsEvent() {
        ID = "Lizards";
        Weight = 10;
        Description = "Increases Puffer spawn chance and there's more of them.";
        MessagesList = new List<string>() {
            { "Dominated by hostile life form" },
            { "Dominant species detected" },
            { "Don't get too close!" },
            { "Be aware of toxic spore clouds" },
            { "Don't inhale the spores!" }
        };
        shortMessagesList = new List<string>() {
            { "SPORE DUST" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable(Util.getEnemyByType(typeof(PufferAI)))) {
            return false;
        }
        levelModifier.AddEnemyComponentRarity(Util.getEnemyByType(typeof(PufferAI)), 100);
        levelModifier.AddEnemyComponentMaxCount(Util.getEnemyByType(typeof(PufferAI)), 5);
        levelModifier.AddEnemyComponentPower(Util.getEnemyByType(typeof(PufferAI)), 0);
        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "red");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}