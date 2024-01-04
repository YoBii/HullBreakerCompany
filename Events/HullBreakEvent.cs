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
    public override string GetMessage() => "<color=white>KPI report: underperforming</color>\n<color=green>" + bonus_credits + "</color><color=white> credits received. Prove your worth to the company!</color>";
    public override string GetShortMessage() => "<color=white>BONUS CREDITS: </color><color=green>" + bonus_credits + "</color>";
    private int bonus_credits;
    public override void Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        bonus_credits = Random.Range(50, 200);

        HullManager.Instance.AddMoney(bonus_credits);
        HullManager.SendChatEventMessage(this);
    }
}