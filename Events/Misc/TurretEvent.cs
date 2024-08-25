﻿using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class TurretEvent : HullEvent
{
    public override string ID() => "Turret";
    public override int GetWeight() => 20;
    public override string GetDescription() => "Spawns additional turrets inside.";
    public static List<string> MessagesList = new() {
        { "High security compound" },
        { "It's right around the corner!" },
        { "Keep moving from cover to cover!" },
        { "The company recommends bringing a bulletproof vest!" }
    };
    public static List<string> shortMessagesList = new() {
        { "TURRETS" },
        { "BULLET HELL" }
    };
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsTrapUnitSpawnable(Util.getTrapUnitByType(typeof(Turret)))) return false;
        levelModifier.AddTrapUnit(Util.getTrapUnitByType(typeof(Turret)), Plugin.TurretScale);
        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "red");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}