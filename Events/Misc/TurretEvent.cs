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
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsTrapUnitSpawnable(Util.getTrapUnitByType(typeof(Turret)))) return false;
        levelModifier.AddTrapUnit(Util.getTrapUnitByType(typeof(Turret)), Plugin.TurretScale);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}