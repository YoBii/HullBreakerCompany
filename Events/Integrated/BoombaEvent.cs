using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Integrated;

public class BoombaEvent : HullEvent
{
    public BoombaEvent() {
        ID = "Boomba";
        Weight = 10;
        Description = "Increases spawn chance of Boomba and there's more of them. Also spawns more landmines.";
        MessagesList = new List<string>() {
            { "Cleaning crew's still around" },
            { "Mines left right and center" },
            { "Lots of mines and some.. are moving?" }
        };
        shortMessagesList = new List<string>() {
            { "BOOM-BA" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable("Boomba"))
        {
            return false;
        }
        levelModifier.AddEnemyComponentRarity("Boomba", 100);
        levelModifier.AddEnemyComponentMaxCount("Boomba", 10);
        levelModifier.AddEnemyComponentPower("Boomba", 0);

        if (levelModifier.IsTrapUnitSpawnable(Util.getTrapUnitByType(typeof(Landmine))))
        {
            levelModifier.AddTrapUnit(Util.getTrapUnitByType(typeof(Landmine)), Plugin.LandmineScale / 2);
        }
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