using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;
using Random = UnityEngine.Random;

namespace HullBreakerCompany.Events;

public class HellEvent : HullEvent
{
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        const string message = "<color=orange>It says here that there is total hell happening on the this moon</color>";
        componentRarity.Add(typeof(JesterAI), 64);
        
        HullManager.SendChatMessage(message);
        RoundManager.Instance.hourTimeBetweenEnemySpawnBatches = 1;

        HullManager.Instance.ExecuteAfterDelay(() => { Hell(); }, 15f);
    }
    private void Hell()
    {
        EnemyVent[] enemyVent = UnityEngine.Object.FindObjectsOfType<EnemyVent>();

        for (int i = 0; i < 32; i++)
        {
            if (enemyVent.Length > 0)
            {
                EnemyVent randomVent = enemyVent[Random.Range(0, enemyVent.Length)];
                RoundManager.Instance.SpawnEnemyFromVent(randomVent);
            }
        }

    }
}