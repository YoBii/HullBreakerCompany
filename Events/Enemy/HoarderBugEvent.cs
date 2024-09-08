using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class HoarderBugEvent : HullEvent
{
    public HoarderBugEvent() {
        ID = "HoarderBug";
        Weight = 30;
        Description = "Spawns a lot of Hoarderbugs while still allowing other enemies to spawn.";
        MessagesList = new List<string>() {
            { "Residues of pesticide detected in the atmosphere" },
            { "Scrap is.. moving?" },
            { "Dominant species detected" },
            { "The competition is already here" },
            { "The hoarding bug mafia is in charge of this place" },
            { "They are stealing our scrap!" },
            { "Friend..?" }
        };
        shortMessagesList = new List<string>() {
            { "CAPTURE THE SCRAP" },
            { "MOVING SCRAP" },
            { "COMPETITION" },
            { "MAFIA" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable(Util.getEnemyByType(typeof(HoarderBugAI)))) {
            return false;
        }
        levelModifier.AddEnemyComponentRarity(Util.getEnemyByType(typeof(HoarderBugAI)), 100);
        levelModifier.AddEnemyComponentMaxCount(Util.getEnemyByType(typeof(HoarderBugAI)), 10);
        levelModifier.AddEnemyComponentPower(Util.getEnemyByType(typeof(HoarderBugAI)), 0);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}