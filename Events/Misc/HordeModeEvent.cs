using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;
using UnityEngine;

namespace HullBreakerCompany.Events.Misc;

public class HordeModeEvent : HullEvent
{
    public HordeModeEvent() {
        ID = "HordeMode";
        Weight = 5;
        Description = "Increases inside enemy spawn rate. Spawns enemies earlier and more frequently.";
        MessagesList = new List<string>() { 
            { "Extreme activity levels!" },
            { "No one has ever returned from here.." },
            { "Caution! Activity level 9 9 9 9 9 9 9 9 9 9 9 9" },
            { "The company wishes the best of luck!" }
        };
        shortMessagesList = new List<string>() {
            { "HORDEMODE" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        // maybe check whether spawnchance field in levelModifier is set instead
        if (Plugin.UseHullBreakerLevelSettings) {
            Plugin.Mls.LogInfo("Spawn chance already increased by HullBreaker level settings");
            return false;
        }
        levelModifier.AddEnemySpawnChanceThroughoutDay(512);
        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "red");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}