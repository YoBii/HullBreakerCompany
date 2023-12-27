using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class HackedTurretsEvent : HullEvent
{
    public override string ID() => "HackedTurrets";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Turrets dont work on the moon";
    public override string GetMessage() => "<color=white>Security systems offline</color>";
    public override string GetShortMessage() => "<color=white>SECURITY OFFLINE</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        if (HullManager.Instance == null)
        {
            Plugin.Mls.LogError("HullManager.Instance is null");
            return;
        }

        if (level == null)
        {
            Plugin.Mls.LogError("level is null");
            return;
        }
        
        HullManager.Instance.ExecuteAfterDelay(() => { HackTurrets(); }, 16f);
        HullManager.SendChatEventMessage(this);
    }
    
    private void HackTurrets()
    {
        Turret[] turrets = UnityEngine.Object.FindObjectsOfType<Turret>();
        foreach (Turret turret in turrets)
        {
            turret.ToggleTurretServerRpc(false);
        }
    }
}