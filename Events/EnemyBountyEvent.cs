using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Event;

namespace HullBreakerCompany.Events;

public class EnemyBountyEvent : HullEvent
{
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        string message = "<color=white>Company pays money for killing the enemies!</color>";
        Plugin.BountyIsActive = true;
        HUDManager.Instance.AddTextToChatOnServer(message);
    }
}