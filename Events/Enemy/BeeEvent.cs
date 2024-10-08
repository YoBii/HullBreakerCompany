﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;
using UnityEngine.UIElements.Collections;

namespace HullBreakerCompany.Events.Enemy;

public class BeeEvent : HullEvent
{
    public BeeEvent() {
        ID = "Bee";
        Weight = 20;
        Description = "Increases bee hive spawns outside";
        MessagesList = new List<string>() {
            { "Dominated by hostile life form"},
            { "Dominant species detected"},
            { "Sticky golden goodness" },
            { "Extract the bee hives!" },
            { "Don't get stung by bees!" }
        };
        shortMessagesList = new List<string>() {
            { "BZZZZZ" },
            { "STICKY GOLD" }
        };
    }

    public override string GetID() => "Bee";
    public override int GetWeight() => 20;
    public override string GetDescription() => "Increases bee hive spawns outside";
    public static List<string> MessagesList = new() {
        { "Dominated by hostile life form"},
        { "Dominant species detected"},
        { "Sticky golden goodness" },
        { "Extract the bee hives!" },
        { "Don't get stung by bees!" }
    };
    public static List<string> shortMessagesList = new() {
        { "BZZZZZ" },
        { "STICKY GOLD" }
    };
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsDaytimeEnemySpawnable(Util.getEnemyByType(typeof(RedLocustBees)))) {
            return false;
        }
        levelModifier.AddDaytimeEnemyComponentRarity(Util.getEnemyByType(typeof(RedLocustBees)), 1000);
        levelModifier.AddDaytimeEnemyComponentPower(Util.getEnemyByType(typeof(RedLocustBees)), 0);
        levelModifier.AddDaytimeEnemyComponentMaxCount(Util.getEnemyByType(typeof(RedLocustBees)), 10);
        levelModifier.AddDaytimeEnemySpawnChanceThroughoutDay(32);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}