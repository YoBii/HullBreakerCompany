using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;

namespace HullBreakerCompany.Events;

public class OnAPowderKegEvent : HullEvent
{
    public override string ID() => "OnAPowderKeg";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        const string message = "<color=red>CAUTION,</color> <color=white>landmines can detonate at any time</color>";
        HullManager.Instance.ExecuteAfterDelay(() => { DetonateLandMine(); }, UnityEngine.Random.Range(60, 700));
        HullManager.SendChatMessage(message);
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