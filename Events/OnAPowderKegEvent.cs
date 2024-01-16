﻿using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class OnAPowderKegEvent : HullEvent
{
    public override string ID() => "OnAPowderKeg";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Landmines can detonate at any time";
    public override string GetMessage() => "<color=white>Explosions detected</color>";
    public override string GetShortMessage() => "<color=red>ON A POWDER KEG</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        Plugin.addLandminesToLevelUnits(level, Plugin.LandmineScale * 2f / 3f);
        HullManager.Instance.ExecuteAfterDelay(() => { DetonateLandMine(); }, UnityEngine.Random.Range(10, 300));
        HullManager.SendChatEventMessage(this);
    }

    private void DetonateLandMine()
    {
        Landmine[] landmines = UnityEngine.Object.FindObjectsOfType<Landmine>();
        foreach (Landmine landmine in landmines)
        {
            landmine.ExplodeMineServerRpc();
        }
    }
}