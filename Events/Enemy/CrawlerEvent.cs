using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class CrawlerEvent : HullEvent
{
    public override string ID() => "Crawler";
    public override int GetWeight() => 5;
    public override string GetDescription() => "Increases thumper (also Crawler or Halves) spawn chance. Allows multiple to spawn and lowers their power level.";
    public static List<string> MessagesList = new() {
        { "Dominated by hostile life form"},
        { "Dominant species detected"},
        { "Something is stomping heavily. Probably hostile." },
        { "To bonk or be bonked, that is the question" },
        { "Intense thumping inside the facility" },
        { "Does putting two halves together MAKE US WHOLE AGAIN?" },
        { "Prepare to get stomped on" }
    };
    public static List<string> shortMessagesList = new() {
        { "THUMP" },
        { "STOMP" }
    };
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if(!levelModifier.IsEnemySpawnable(Util.getEnemyByType(typeof(CrawlerAI)))) {
            return false;
        }
        levelModifier.AddEnemyComponentRarity(Util.getEnemyByType(typeof(CrawlerAI)), 100);
        levelModifier.AddEnemyComponentPower(Util.getEnemyByType(typeof(CrawlerAI)), 1);
        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "red");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}