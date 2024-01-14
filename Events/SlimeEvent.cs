﻿using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class SlimeEvent : HullEvent
{
    public override string ID() => "Slime";
    public override int GetWeight() => 20;
    public override string GetDescription() => "Increased chance of slime spawn";
    public override string GetMessage() => "<color=white>Inhabited with slime</color>";
    public override string GetShortMessage() => "<color=white>SO SLIMY</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        if (level.Enemies.All(unit => unit.enemyType.enemyPrefab.GetComponent<BlobAI>() == null)) return;
        
        enemyComponentRarity.Add(typeof(BlobAI), 48);
        HullManager.SendChatEventMessage(this);
    }
}