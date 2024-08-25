using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class NutcrackerEvent : HullEvent
{
    public override string ID() => "Nutcracker";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Increases spawn chance of Nutcracker.";
    public static List<string> MessagesList = new() {
        { "Reports of paranormal activity" },
        { "Paranormal signature detected" },
        { "Don't you move.." },
        { "Keep your head down" },
        { "Protect your nuts!" }
    };
    public static List<string> shortMessagesList = new() {
        { "THIS IS NUTS" }
    };
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
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