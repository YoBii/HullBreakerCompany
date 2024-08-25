using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Integrated.SCP;

public class ShyGuyEvent : HullEvent
{
    public override string ID() => "ShyGuy";
    public override int GetWeight() => 5;
    public override string GetDescription() => "Increases spawn chance of Shy Guy. Allows more than one to spawn.";
    public static List<string> MessagesList = new() {
        { "Do NOT look at him!" },
        { "Scopophobia.." },
        { "SCP-096" },
        { "When you're scared to take a look.." },
        { "No one who's seen their face has survived.." }
    };
    public static List<string> shortMessagesList = new() {
        { "SHY" }
    };
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable("ShyGuy"))
        {
            return false;
        }
        levelModifier.AddEnemyComponentRarity("ShyGuy", 100);
        levelModifier.AddEnemyComponentMaxCount("ShyGuy", 5);
        levelModifier.AddEnemyComponentPower("ShyGuy", 0);

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