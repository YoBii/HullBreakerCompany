using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class ArachnophobiaEvent : HullEvent
{
    public ArachnophobiaEvent() {
        ID = "Arachnophobia";
        Weight = 10;
        Description = "Increases bunker spider (SandSpider) spawn chance. Allows more than one to spawn.";
        MessagesList = new List<string>() {
            { "Dominated by hostile life form"},
            { "Dominant species detected"},
            { "This place is crawling.." },
            { "They have 8 eyes.." },
            { "Natural spider habitat" }
        };
        shortMessagesList = new List<string>() {
            { "EIGHT EYES" },
            { "EIGHT LEGS" }
        };  
    }
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if(!levelModifier.IsEnemySpawnable(Util.getEnemyByType(typeof(SandSpiderAI)))) {
            return false;
        }
        levelModifier.AddEnemyComponentRarity(Util.getEnemyByType(typeof(SandSpiderAI)), 100);
        levelModifier.AddEnemyComponentMaxCount(Util.getEnemyByType(typeof(SandSpiderAI)), 5);
        levelModifier.AddEnemyComponentPower(Util.getEnemyByType(typeof(SandSpiderAI)), 0);
        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "red");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}