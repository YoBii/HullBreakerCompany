using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class SpringManEvent : HullEvent
{
    public override string ID() => "SpringMan";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Increases spawn chance of Coil-Head (SpringMan)";
    public static List<string> MessagesList = new() {
        { "Reports of paranormal activity" },
        { "Paranormal signature detected" },
        { "They'll only kill you when you're not looking" },
        { "Don't loose your head now!" },
        { "Now this is a staring contest you do not want to loose!" }
    };
    public static List<string> shortMessagesList = new() {
        { "SPRING" },
        { "BOING!" }
    };
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
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