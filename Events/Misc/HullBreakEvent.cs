using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;
using Random = UnityEngine.Random;


namespace HullBreakerCompany.Events.Misc;

public class HullBreakEvent : HullEvent
{
    public HullBreakEvent() {
        ID = "HullBreak";
        Weight = 10;
        Description = "The company sends a bonus payment. Take a break from stressfull events.";
        MessagesList = new() {
            { "KPI report: underperforming\nYou are granted </color><color=green>[AMOUNT]</color><color=white> credits. Use them to prove your worth to the company!" },
            { "The company appreciates your loyalty. You receive </color><color=green>[AMOUNT]</color><color=white> credits" },
            { "Bonus payment received: </color><color=green>[AMOUNT]</color><color=white> credits. Keep up the good work!" }
        };
        shortMessagesList = new() {
            { "BONUS CREDITS: </color><color=green>[AMOUNT]"},
            { "BONUS PAYMENT: </color><color=green>[AMOUNT]"}
        };
    }
    private static int bonus_credits;
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
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        bonus_credits = Random.Range(Plugin.HullBreakEventCreditsMin, Plugin.HullBreakEventCreditsMax);
        HullManager.Instance.AddMoney(bonus_credits);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}