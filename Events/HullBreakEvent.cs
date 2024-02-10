using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;
using Random = UnityEngine.Random;


namespace HullBreakerCompany.Events;

public class HullBreakEvent : HullEvent
{
    public override string ID() => "HullBreak";
    public override int GetWeight() => 5;
    public override string GetDescription() => "Getting money for visiting this moon";
    public static List<String> MessagesList = new() {
        { "KPI report: underperforming\nUse </color><color=green>[AMOUNT]</color><color=white> credits to prove your worth to the company!" },
        { "The company appreciates your loyalty. You receive </color><color=green>[AMOUNT]</color><color=white> credits" },
        { "Bonus payment received: </color><color=green>[AMOUNT]</color><color=white> credits" }
    };
    public static List<String> shortMessagesList = new() {
        { "BONUS CREDITS: </color><color=green>[AMOUNT]"},
        { "BONUS PAYMENT: </color><color=green>[AMOUNT]"}
    };
    public override string GetMessage() {
        String str = "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
        return str.Replace("[AMOUNT]", bonus_credits.ToString());
    }
    public override string GetShortMessage() {
        String str = "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
        return str.Replace("[AMOUNT]", bonus_credits.ToString());
    }
    private static int bonus_credits;
    public override bool Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        bonus_credits = Random.Range(Plugin.HullBreakEventCreditsMin, Plugin.HullBreakEventCreditsMax);
        HullManager.Instance.AddMoney(bonus_credits);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}