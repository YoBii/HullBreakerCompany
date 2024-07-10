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
    public override string GetDescription() => "Increases Jester spawn chance. Allows more than one to spawn.";
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
        if (!levelModifier.IsEnemySpawnable(Util.getEnemyByType(typeof(JesterAI)))) {
            return false;
        }
        levelModifier.AddEnemyComponentRarity(Util.getEnemyByType(typeof(JesterAI)), 100);
        levelModifier.AddEnemyComponentMaxCount(Util.getEnemyByType(typeof(JesterAI)), 4);
        levelModifier.AddEnemyComponentPower(Util.getEnemyByType(typeof(JesterAI)), 0);

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