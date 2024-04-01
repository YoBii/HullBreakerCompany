using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class HoarderBugEvent : HullEvent
{
    public override string ID() => "HoarderBug";
    public override int GetWeight() => 30;
    public override string GetDescription() => "Increased chance of hoarder bug spawn";
    public static List<string> MessagesList = new() {
        { "Residues of pestice detected in the atmosphere" },
        { "Scrap is.. moving?"},
        { "Dominant species detected"},
        { "The competition is already here" },
        { "The hoarding bug mafia is in charge of this place" },
        { "They are stealing our scrap!" },
        { "Friend..?" }
    };
    public static List<string> shortMessagesList = new() {
        { "CAPTURE THE SCRAP" },
        { "MOVING SCRAP" },
        { "COMPETITION" },
        { "MAFIA" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable(EnemyUtil.getEnemyByType(typeof(HoarderBugAI)))) {
            return false;
        }
        levelModifier.AddEnemyComponentRarity(EnemyUtil.getEnemyByType(typeof(HoarderBugAI)), 500);
        levelModifier.AddEnemyComponentMaxCount(EnemyUtil.getEnemyByType(typeof(HoarderBugAI)), 10);
        levelModifier.AddMaxEnemyPower(10);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}