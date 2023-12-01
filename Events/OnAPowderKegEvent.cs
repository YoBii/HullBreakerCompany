using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;

namespace HullBreakerCompany.Events;

public class OnAPowderKegEvent : HullEvent
{
    public override string ID() => "OnAPowderKeg";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Landmines can detonate at any time";
    public override string GetMessage() => "<color=red>CAUTION,</color> <color=white>landmines can detonate at any time</color>";
    public override string GetShortMessage() => "<color=red>ON A POWDER KEG</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        HullManager.Instance.ExecuteAfterDelay(() => { DetonateLandMine(); }, UnityEngine.Random.Range(30, 680));
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