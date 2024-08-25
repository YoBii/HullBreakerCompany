﻿using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class MaskedEvent : HullEvent
{
    public override string ID() => "Masked";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Increases spawn chance of Masked.";
    public static List<string> MessagesList = new() {
        { "Reports of paranormal activity" },
        { "Paranormal signature detected" },
        { "Phantom of the Opera" },
        { "Who's the new guy?"}
    };
    public static List<string> shortMessagesList = new() {
        { "MASKED" },
        { "TRUST ISSUES" }
    };
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable(Util.getEnemyByType(typeof(MaskedPlayerEnemy)))) {
            return false;
        }
        levelModifier.AddEnemyComponentRarity(Util.getEnemyByType(typeof(MaskedPlayerEnemy)), 100);
        levelModifier.AddEnemyComponentPower(Util.getEnemyByType(typeof(MaskedPlayerEnemy)), 1);

        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "red");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}