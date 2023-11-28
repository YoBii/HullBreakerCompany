using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;

namespace HullBreakerCompany.Events;

public class OpenTheNoorEvent : HullEvent
{
    public override string ID() => "OpenTheNoor";
    public override int GetWeight() => 20;
    public override string GetDescription() => "All big doors are locked in the level";
    public override string GetMessage() => "<color=white>All big doors are locked in the level</color>";
    public override string GetShortMessage() => "<color=white>OPEN THE NOOR...</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        HullManager.Instance.ExecuteAfterDelay(() => { CloseBigDoors(); }, 10f);
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