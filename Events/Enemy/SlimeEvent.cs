﻿using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class SlimeEvent : HullEvent
{
    public override string ID() => "Slime";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Increases spawn chance of Hygrodere (Blob/Slime) and there's more of them.";
    public static List<string> MessagesList = new() {
        { "Dominated by hostile life form"},
        { "Dominant species detected"},
        { "Don't get lost in the sauce" },
        { "Shapeless creature detected. Water content 99.9%" }
    };
    public static List<string> shortMessagesList = new() {
        { "SLIPPERY FLOOR" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable(Util.getEnemyByType(typeof(BlobAI)))) {
            return false;
        }

        levelModifier.AddEnemyComponentRarity(Util.getEnemyByType(typeof(BlobAI)), 100);
        levelModifier.AddEnemyComponentMaxCount(Util.getEnemyByType(typeof(BlobAI)), 5);
        levelModifier.AddEnemyComponentPower(Util.getEnemyByType(typeof(BlobAI)), 0);

        HullManager.AddChatEventMessage(this);
        return true;
    }
}