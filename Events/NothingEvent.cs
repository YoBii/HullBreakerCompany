using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class NothingEvent : HullEvent
{
    public override string ID() => "Nothing";
    public override int GetWeight() => 20;
    public override string GetDescription() => "Nothing happens";

    public static List<String> MessagesList = new()
    {
        { "<color=white>...</color>" },
        { "<color=white>---</color>" },
        { "<color=white>DAMAGED...</color>" },
        { "<color=white>???</color>" },
    };
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override void Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        // simply omit the message
        // HullManager.SendChatEventMessage(this);
    }
}