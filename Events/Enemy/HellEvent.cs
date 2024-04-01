using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HullBreakerCompany.Events.Enemy;

public class HellEvent : HullEvent
{
    public override string ID() => "Hell";
    public override int GetWeight() => 1;
    public override string GetDescription() => "Increased chance of spawning Jester and more enemies";
    public static List<string> MessagesList = new() {
        { "What's in the box?!" },
        { "Upon encounter evacuate immediately!" },
        { "Sir, I'm going to have to ask you to leave." },
        { "The company wishes the best of luck!" }
    };
    public static List<string> shortMessagesList = new() {
        { "HELL" },
        { "LEAVE" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable(EnemyUtil.getEnemyByType(typeof(JesterAI)))) {
            return false;
        }
        levelModifier.AddEnemyComponentRarity(EnemyUtil.getEnemyByType(typeof(JesterAI)), 500);
        levelModifier.AddEnemyComponentMaxCount(EnemyUtil.getEnemyByType(typeof(JesterAI)), 4);
        levelModifier.AddEnemyComponentPower(EnemyUtil.getEnemyByType(typeof(JesterAI)), 1);

        HullManager.AddChatEventMessage(this);
        //RoundManager.Instance.hourTimeBetweenEnemySpawnBatches = 1;

        //HullManager.Instance.ExecuteAfterDelay(() => { Hell(); }, 16f);
        return true;
    }
    private void Hell()
    {
        EnemyVent[] enemyVent = UnityEngine.Object.FindObjectsOfType<EnemyVent>();

        for (int i = 0; i < 8; i++)
        {
            if (enemyVent.Length > 0)
            {
                EnemyVent randomVent = enemyVent[Random.Range(0, enemyVent.Length)];
                RoundManager.Instance.SpawnEnemyFromVent(randomVent);
            }
        }

    }
}