﻿using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class FlowerManEvent : HullEvent
{
    public override string ID() => "FlowerMan";
    public override int GetWeight() => 20;
    public override string GetDescription() => "Increased chance of flowerman spawn";
    public static List<String> MessagesList = new() {
        { "Detected signs of paranormal activity" },
        { "Several anomalies detected" },
        { "Lurking in the shadows" },
        { "Fear of the dark" },
        { "Always behind you.." },
        { "When you're scared to take a look.." },
        { "Whatever you do, do not stare!" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>PARANORMAL</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        if (level.Enemies.All(unit => unit.enemyType.enemyPrefab.GetComponent<FlowermanAI>() == null)) return;
        
        enemyComponentRarity.Add(typeof(FlowermanAI), 256);
        HullManager.SendChatEventMessage(this);
    }
}