using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class OpenTheNoorEvent : HullEvent
{
    public override string ID() => "OpenTheNoor";
    public override int GetWeight() => 15;
    public override string GetDescription() => "All big doors are locked in the level";
    public static List<String> MessagesList = new() {
        { "High security compound" },
        { "You shall not pass!" },
        { "Hodor - Hold the door!" },
        { "They locked this place down" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>HIGH SECURITY</color>";
    public override bool Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
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