using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Integrated;

public class BoombaEvent : HullEvent
{
    public override string ID() => "Boomba";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Increases spawn chance of Boomba and there's more of them. Also spawns more landmines.";
    public static List<string> MessagesList = new() {
        { "Cleaning crew's still around" },
        { "Mines left right and center" },
        { "Lots of mines and some.. are moving?" }
    };
    public static List<string> shortMessagesList = new() {
        { "BOOM-BA" }
    };
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
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