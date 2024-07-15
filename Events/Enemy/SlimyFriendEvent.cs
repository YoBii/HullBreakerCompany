using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class SlimyFriend : HullEvent
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
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable("SCP999Enemy")) {
            return false;
        }

        levelModifier.AddEnemyComponentRarity("SCP999Enemy", 100);
        levelModifier.AddEnemyComponentMaxCount("SCP999Enemy", 3);
        levelModifier.AddEnemyComponentPower("SCP999Enemy", 3);

        HullManager.AddChatEventMessage(this);
        return true;
    }
}