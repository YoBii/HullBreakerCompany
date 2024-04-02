using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class HackedTurretsEvent : HullEvent
{
    public override string ID() => "HackedTurrets";
    public override int GetWeight() => 20;
    public override string GetDescription() => "All turrets are permanently disabled.";
    public static List<string> MessagesList = new() {
        { "Security systems offline" },
        { "Abandoned after their defense sytems were hacked" },
        { "The company disabled all turrets on this moon" }
    };
    public static List<string> shortMessagesList = new() {
        { "SECURITY OFFLINE" },
        { "TURRETS HACKED "}
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (HullManager.Instance == null)
        {
            Plugin.Mls.LogError("HullManager.Instance is null");
            return false;
        }

        if (level == null)
        {
            Plugin.Mls.LogError("level is null");
            return false;
        }

        HullManager.Instance.ExecuteAfterDelay(() => { HackTurrets(); }, 16f);
        HullManager.AddChatEventMessage(this);
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