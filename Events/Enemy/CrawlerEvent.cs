using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class CrawlerEvent : HullEvent
{
    public CrawlerEvent () {
        ID = "Crawler";
        Weight = 5;
        Description = "Increases thumper (also Crawler or Halves) spawn chance. Allows multiple to spawn and lowers their power level.";
        MessagesList = new List<string>() { 
            { "Dominated by hostile life form"},
            { "Dominant species detected"},
            { "Heavy stomping inside. Probably hostile." },
            { "To bonk or be bonked, that is the question" },
            { "Intense thumping inside the facility" },
            { "Does putting two halves together MAKE US WHOLE AGAIN?" },
        };
        shortMessagesList = new List<string>() {
            { "THUMP" },
            { "STOMP" }
        };
    }
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