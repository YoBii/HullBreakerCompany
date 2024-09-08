using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HullBreakerCompany.Events.Enemy;

public class HellEvent : HullEvent
{
    public HellEvent() {
        ID = "Hell";
        Weight = 1;
        Description = "Increases Jester spawn chance. Allows more than one to spawn.";
        MessagesList = new List<string>() {
            { "What's in the box?!" },
            { "Upon encounter evacuate immediately!" },
            { "Sir, I'm going to have to ask you to leave." },
            { "The company wishes the best of luck!" }
        };
        shortMessagesList = new List<string>() {
            { "HELL" },
            { "LEAVE" }
        };
    }
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable(Util.getEnemyByType(typeof(JesterAI)))) {
            return false;
        }
        levelModifier.AddEnemyComponentRarity(Util.getEnemyByType(typeof(JesterAI)), 100);
        levelModifier.AddEnemyComponentMaxCount(Util.getEnemyByType(typeof(JesterAI)), 4);
        levelModifier.AddEnemyComponentPower(Util.getEnemyByType(typeof(JesterAI)), 0);

        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "red");
        } else {
            HullManager.AddChatEventMessage(this);
        }
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