using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Enemy;

public class DevochkaPizdecEvent : HullEvent
{
    public override string ID() => "DevochkaPizdec";
    public override int GetWeight() => 5;
    public override string GetDescription() => "Increases Ghostgirl (Dressgirl) spawn chance. Allows more than one to spawn.";
    public static List<string> MessagesList = new() {
        { "Reports of paranormal activity" },
        { "Paranormal signature detected" },
        { "Workers were loosing their mind before they shut this place down. Extract what was left behind." },
        { "Don't loose your head now!" }
    };
    public static List<string> shortMessagesList = new() {
        { "PARANOID" },
        { "PLAYTIME "}
    };
    public override string GetMessage() => MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)];
    public override string GetShortMessage() => shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)];
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable(Util.getEnemyByType(typeof(DressGirlAI))))
        {
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