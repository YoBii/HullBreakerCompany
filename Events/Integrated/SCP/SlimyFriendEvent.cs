using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Integrated.SCP;

public class SlimyFriendEvent : HullEvent
{
    public override string ID() => "SlimyFriend";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Increases spawn chance of SCP-999 and there's more of them.";
    public static List<string> MessagesList = new() {
        { "Dominated by friendly life form"},
        { "Passive species detected"},
        { "Large, amorphous, gelatinous mass of translucent orange slime" },
        { "Inhabited by seemingly playful - dog-like - creature. " },
    };
    public static List<string> shortMessagesList = new() {
        { "HYDRATE" }
    };
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable("SCP999Enemy"))
        {
            return false;
        }

        levelModifier.AddEnemyComponentRarity("SCP999Enemy", 100);
        levelModifier.AddEnemyComponentMaxCount("SCP999Enemy", 3);
        levelModifier.AddEnemyComponentPower("SCP999Enemy", 3);

        if (Plugin.ColoredEventMessages)
        {
            HullManager.AddChatEventMessageColored(this, "red");
        }
        else
        {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}