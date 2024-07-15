using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;
using UnityEngine.UIElements.Collections;

namespace HullBreakerCompany.Events.Enemy;

public class InvisibleGuest : HullEvent
{
    public override string ID() => "InvisibleGuest";
    public override int GetWeight() => 20;
    public override string GetDescription() => "Increases SCP966 spawn frequency. Spawns up to three. ";
    public static List<string> MessagesList = new() {
        { "Reports of paranormal activity" },
        { "Paranormal signature detected" },
        { "A feeling of heaviness might arise" },
        { "It's wearing you down.." },
        { "Did you see that?!?!" }
    };
    public static List<string> shortMessagesList = new() {
        { "SLUGGISH" },
        { "GUEST" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable("scp966")) {
            return false;
        }
        levelModifier.AddEnemyComponentRarity("scp966", 100);
        levelModifier.AddEnemyComponentMaxCount("scp966", 3);
        levelModifier.AddEnemyComponentPower("scp966", 1);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}