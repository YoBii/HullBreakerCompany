using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Integrated.SCP;

public class ShyGuyEvent : HullEvent
{
    public ShyGuyEvent() {
        ID = "ShyGuy";
        Weight = 5;
        Description = "Increases spawn chance of Shy Guy. Allows more than one to spawn.";
        MessagesList = new List<string>() {
            { "Do NOT look at him!" },
            { "Scopophobia.." },
            { "SCP-096" },
            { "When you're scared to take a look.." },
            { "No one who's seen their face has survived.." }
        };
        shortMessagesList = new List<string>() {
            { "SHY" }
        };
    }
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