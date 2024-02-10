using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class OnAPowderKegEvent : HullEvent
{
    public override string ID() => "OnAPowderKeg";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Landmines can detonate at any time";
    public static List<String> MessagesList = new() {
        { "Reoccuring explosions" },
        { "Explosions detected" },
        { "They installed faulty mines" },
        { "Mines explode at the slightest vibration" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=red>ON A POWDER KEG</color>";
    public override bool Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        Plugin.addLandminesToLevelUnits(level, Plugin.LandmineScale * 2f / 3f);
        HullManager.Instance.ExecuteAfterDelay(() => { DetonateLandMine(); }, UnityEngine.Random.Range(10, 300));
        return true;
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