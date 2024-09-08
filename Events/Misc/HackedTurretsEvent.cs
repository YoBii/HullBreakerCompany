using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class HackedTurretsEvent : HullEvent
{
    public HackedTurretsEvent() {
        ID = "HackedTurrets";
        Weight = 20;
        Description = "All turrets are permanently disabled.";
        MessagesList = new List<string>() {
        { "Security systems offline" },
        { "Abandoned after their defense systems were hacked" },
        { "The company disabled all turrets on this moon" }
    };
        shortMessagesList = new List<string>() {
        { "SECURITY OFFLINE" },
        { "TURRETS HACKED" }
    };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (HullManager.Instance == null) {
            Plugin.Mls.LogError("HullManager.Instance is null");
            return false;
        }
        if (level == null) {
            Plugin.Mls.LogError("level is null");
            return false;
        }

        if (!levelModifier.IsTrapUnitSpawnable(Util.getTrapUnitByType(typeof(Turret)))) return false;
        levelModifier.AddTrapUnit(Util.getTrapUnitByType(typeof(Turret)), Plugin.TurretScale / 3 * 2);

        HullManager.Instance.ExecuteAfterDelay(HackTurrets, 16f);
        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "green");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
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