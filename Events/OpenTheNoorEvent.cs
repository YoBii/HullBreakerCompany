using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;

namespace HullBreakerCompany.Events;

public class OpenTheNoorEvent : HullEvent
{
    public override string ID() => "OpenTheNoor";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        const string message = "<color=white>All big doors are locked in the level</color>";
        HullManager.Instance.ExecuteAfterDelay(() => { CloseBigDoors(); }, 10f);
        HullManager.SendChatMessage(message);
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