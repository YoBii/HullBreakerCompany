using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HullBreakerCompany.Events;

public class HellEvent : HullEvent
{
    public override string ID() => "Hell";
    public override int GetWeight() => 1;
    public override string GetDescription() => "Increased chance of spawning Jester and more enemies";
    public static List<String> MessagesList = new() {
        { "Extreme activity levels!" },
        { "No one has ever returned from here.." },
        { "Caution! Activity level 9 9 9 9 9 9 9 9 9 9 9 9 9" },
        { "The company wishes the best of luck!" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>HELL</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        if (level.Enemies.All(unit => unit.enemyType.enemyPrefab.GetComponent<JesterAI>() == null)) return;
        
        enemyComponentRarity.Add(typeof(JesterAI), 64);
        
        HullManager.SendChatEventMessage(this);
        RoundManager.Instance.hourTimeBetweenEnemySpawnBatches = 1;

        HullManager.Instance.ExecuteAfterDelay(() => { Hell(); }, 16f);
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