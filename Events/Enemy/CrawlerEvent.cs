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
        { "Heavy thumping inside the facility" },
        { "Does putting two halves together MAKE US WHOLE AGAIN?" },
        { "" }
    };
    public static List<string> shortMessagesList = new() {
        { "THUMP" },
        { "STOMP" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if(!levelModifier.IsEnemySpawnable(Util.getEnemyByType(typeof(CrawlerAI)))) {
            return false;
        }
        levelModifier.AddEnemyComponentRarity(Util.getEnemyByType(typeof(CrawlerAI)), 100);
        levelModifier.AddEnemyComponentMaxCount(Util.getEnemyByType(typeof(CrawlerAI)), 4);
        levelModifier.AddEnemyComponentPower(Util.getEnemyByType(typeof(CrawlerAI)), 1);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}