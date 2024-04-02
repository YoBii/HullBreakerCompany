using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;
using Random = UnityEngine.Random;


namespace HullBreakerCompany.Events.Misc;

public class HullBreakEvent : HullEvent
{
    public override string ID() => "HullBreak";
    public override int GetWeight() => 10;
    public override string GetDescription() => "The company sends a bonus payment. Take a break from stressfull events.";
    public static List<string> MessagesList = new() {
        { "KPI report: underperforming\nUse </color><color=green>[AMOUNT]</color><color=white> credits to prove your worth to the company!" },
        { "The company appreciates your loyalty. You receive </color><color=green>[AMOUNT]</color><color=white> credits" },
        { "Bonus payment received: </color><color=green>[AMOUNT]</color><color=white> credits. Keep up the good work!" }
    };
    public static List<string> shortMessagesList = new() {
        { "BONUS CREDITS: </color><color=green>[AMOUNT]"},
        { "BONUS PAYMENT: </color><color=green>[AMOUNT]"}
    };
    public override string GetMessage()
    {
        string str = "<color=white>" + MessagesList[Random.Range(0, MessagesList.Count)] + "</color>";
        return str.Replace("[AMOUNT]", bonus_credits.ToString());
    }
    public override string GetShortMessage()
    {
        string str = "<color=white>" + shortMessagesList[Random.Range(0, shortMessagesList.Count)] + "</color>";
        return str.Replace("[AMOUNT]", bonus_credits.ToString());
    }
    private static int bonus_credits;
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        bonus_credits = Random.Range(Plugin.HullBreakEventCreditsMin, Plugin.HullBreakEventCreditsMax);
        HullManager.Instance.AddMoney(bonus_credits);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}