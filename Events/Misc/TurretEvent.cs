using HullBreakerCompany.Hull;
using System.Collections.Generic;

namespace HullBreakerCompany.Events.Misc;

public class TurretEvent : HullEvent
{
    public TurretEvent()
    {
        ID = "Turret";
        Weight = 20;
        Description = "Spawns additional turrets inside.";
        MessagesList = new List<string>() {
            { "High security compound" },
            { "It's right around the corner!" },
            { "Keep moving from cover to cover!" },
            { "The company recommends bringing a bulletproof vest!" }
        };
        shortMessagesList = new List<string>() {
            { "TURRETS" },
            { "BULLET HELL" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsTrapUnitSpawnable(Util.getTrapUnitByType(typeof(Turret)))) return false;
        levelModifier.AddTrapUnit(Util.getTrapUnitByType(typeof(Turret)), Plugin.TurretScale);
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