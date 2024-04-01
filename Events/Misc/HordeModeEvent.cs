using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;
using UnityEngine;

namespace HullBreakerCompany.Events.Misc;

public class HordeModeEvent : HullEvent
{
    public override string ID() => "HordeMode";
    public override int GetWeight() => 5;
    public override string GetDescription() => "Increased inside enemy spawns";
    public static List<string> MessagesList = new() {
        { "Extreme activity levels!" },
        { "No one has ever returned from here.." },
        { "Caution! Activity level 9 9 9 9 9 9 9 9 9 9 9 9" },
        { "The company wishes the best of luck!" }
    };
    public static List<string> shortMessagesList = new() {
        { "HORDEMODE" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        // maybe check whether spawnchance field in levelModifier is set instead
        if (Plugin.UseHullBreakerLevelSettings) {
            Plugin.Mls.LogInfo("Spawn chance already increased by HullBreaker level settings");
            return false;
        }
        levelModifier.AddEnemySpawnChanceThroughoutDay(512);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}