using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class ShyGuyEvent : HullEvent
{
    public override string ID() => "ShyGuy";
    public override int GetWeight() => 5;
    public override string GetDescription() => "Increases spawn chance of Shy Guy. Allows more than one to spawn.";
    public static List<string> MessagesList = new() {
        { "Do NOT look at him!" },
        { "When you're scared to take a look.." },
        { "No one who's seen their face has survived.." }
    };
    public static List<string> shortMessagesList = new() {
        { "SHY" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable("ShyGuy")) {
            return false;
        }
        levelModifier.AddEnemyComponentRarity("ShyGuy", 500);
        levelModifier.AddEnemyComponentMaxCount("ShyGuy", 5);
        levelModifier.AddEnemyComponentPower("ShyGuy", 1);
        levelModifier.AddMaxEnemyPower(4);

        HullManager.AddChatEventMessage(this);
        return true;
    }
}