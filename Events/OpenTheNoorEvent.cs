using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class OpenTheNoorEvent : HullEvent
{
    public override string ID() => "OpenTheNoor";
    public override int GetWeight() => 20;
    public override string GetDescription() => "All big doors are locked in the level";
    public override string GetMessage() => "<color=white>All big doors are locked in the level</color>";
    public override string GetShortMessage() => "<color=white>OPEN THE NOOR...</color>";
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

        HullManager.Instance.ExecuteAfterDelay(() => { CloseBigDoors(); }, 16f);
        HullManager.SendChatEventMessage(this);
    }
    
    private void CloseBigDoors()
    {
        TerminalAccessibleObject[] doorLocks = UnityEngine.Object.FindObjectsOfType<TerminalAccessibleObject>();
        foreach (TerminalAccessibleObject doorLock in doorLocks)
        {
            doorLock.SetDoorOpenServerRpc(false);
        }
    }
}