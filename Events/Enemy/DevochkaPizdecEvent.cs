using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class DevochkaPizdecEvent : HullEvent {
    public DevochkaPizdecEvent() {
        ID = "DevochkaPizdec";
        Weight = 5;
        Description = "Increases Ghostgirl (Dressgirl) spawn chance. Allows more than one to spawn.";
        MessagesList = new List<string>() {
            { "Reports of paranormal activity" },
            { "Paranormal signature detected" },
            { "Workers were losing their mind before they shut this place down. Extract what was left behind." },
            { "Don't lose your head now!" }
        };
        shortMessagesList = new List<string>() {
            { "PARANOID" },
            { "PLAYTIME" }
        };
    }

    public override bool Execute(SelectableLevel level, LevelModifier levelModifier) {
        if (!levelModifier.IsEnemySpawnable(Util.getEnemyByType(typeof(DressGirlAI)))) {
            return false;
        }
        levelModifier.AddEnemyComponentRarity(Util.getEnemyByType(typeof(DressGirlAI)), 100);
        levelModifier.AddEnemyComponentMaxCount(Util.getEnemyByType(typeof(DressGirlAI)), 4);
        levelModifier.AddEnemyComponentPower(Util.getEnemyByType(typeof(DressGirlAI)), 0);
        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "red");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}
