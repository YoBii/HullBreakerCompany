using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class LizardsEvent : HullEvent
{
    public override string ID() => "Lizards";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Increases Puffer spawn chance and there's more of them.";
    public static List<string> MessagesList = new() {
        { "Dominated by hostile life form" },
        { "Dominant species detected" },
        { "Don't get too close!" },
        { "Be aware of toxic spore clouds" },
        { "Don't inhale the spores!" }
    };
    public static List<string> shortMessagesList = new() {
        { "SPORE DUST" }
    };
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
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