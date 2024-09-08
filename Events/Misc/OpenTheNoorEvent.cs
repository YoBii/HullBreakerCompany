using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class OpenTheNoorEvent : HullEvent
{
    public OpenTheNoorEvent() {
        ID = "OpenTheNoor";
        Weight = 15;
        Description = "All security doors spawn in closed state.";
        MessagesList = new List<string>() {
            { "High security compound" },
            { "You shall not pass!" },
            { "Hodor - Hold the door!" },
            { "They locked this place down" },
            { "Abandoned on lockdown. All security doors are closed." }
        };
        shortMessagesList = new List<string>() {
            { "HODOR" },
            { "OPEN THE NOOR" }
        };
    }
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

        HullManager.Instance.ExecuteAfterDelay(() => { CloseBigDoors(); }, 16f);
        HullManager.AddChatEventMessage(this);
        return true;
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