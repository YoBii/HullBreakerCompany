using HullBreakerCompany.Hull;
using System.Collections.Generic;
using Random = UnityEngine.Random;


namespace HullBreakerCompany.Events.Misc;

public class HullBreakEvent : HullEvent
{
    public HullBreakEvent()
    {
        ID = "HullBreak";
        Weight = 10;
        Description = "The company sends a bonus payment. Take a break from stressfull events.";
        MessagesList = new() {
            { "KPI report: underperforming\nYou are granted </color><color=green>{amount}</color><color=white> credits. Use them to prove your worth to the company!" },
            { "The company appreciates your loyalty. You receive </color><color=green>{amount}</color><color=white> credits" },
            { "Bonus payment received: </color><color=green>{amount}</color><color=white> credits. Keep up the good work!" }
        };
        shortMessagesList = new() {
            { "BONUS CREDITS: </color><color=green>{amount}"},
            { "BONUS PAYMENT: </color><color=green>{amount}"}
        };
    }
    private static int bonus_credits;
    public override string GetMessage()
    {
        List<string> messages = GetActiveMessages();
        string str = "<color=white>" + messages[Random.Range(0, messages.Count)] + "</color>";
        return str.Replace("{amount}", bonus_credits.ToString());
    }
    public override string GetShortMessage()
    {
        List<string> shortMessages = GetActiveShortMessages();
        string str = "<color=white>" + shortMessages[Random.Range(0, shortMessages.Count)] + "</color>";
        return str.Replace("{amount}", bonus_credits.ToString());
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        bonus_credits = Random.Range(Plugin.HullBreakEventCreditsMin, Plugin.HullBreakEventCreditsMax);
        HullManager.Instance.AddMoney(bonus_credits);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}