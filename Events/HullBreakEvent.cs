using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;
using Random = UnityEngine.Random;


namespace HullBreakerCompany.Events;

public class HullBreakEvent : HullEvent
{
    public override string ID() => "HullBreak";
    public override int GetWeight() => 5;
    public override string GetDescription() => "Getting money for visiting this moon";
    public override bool Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        bonus_credits = Random.Range(Plugin.HullBreakEventCreditsMin, Plugin.HullBreakEventCreditsMax);
        HullManager.Instance.AddMoney(bonus_credits);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}