using System;
using System.Collections.Generic;
using System.Linq;

namespace HullBreakerCompany.hull;

public abstract class RandomEnumSelector
{
    private static Random _random = new Random();
    private static Dictionary<GameEvents, int> _weights = new Dictionary<GameEvents, int>()
    {
        { GameEvents.Nothing, 2 },
        { GameEvents.FlowerMan, 2 },
        { GameEvents.SpringMan, 1 },
        { GameEvents.HoarderBug, 3 },
        { GameEvents.Turret, 1 },
        { GameEvents.LandMine, 3 },
        { GameEvents.OutSideEnemyDay, 1 },
        { GameEvents.Lizards, 1 },
        { GameEvents.Arachnophobia, 1 },
        { GameEvents.Bee, 1 },
        { GameEvents.Slime, 1 },
        { GameEvents.DevochkaPizdec, 1 },
        { GameEvents.EnemyBounty, 4 },
    };

    public static List<GameEvents> GetRandomGameEvents()
    {
        var gameEvents = GetWeightedRandomGameEvents(_weights, 3);
        return gameEvents;
    }

    private static List<T> GetWeightedRandomGameEvents<T>(Dictionary<T, int> weights, int count)
    {
        var totalWeight = weights.Sum(x => x.Value);
        var selectedItems = new List<T>();

        for (int i = 0; i < count; i++)
        {
            var randomNumber = _random.Next(totalWeight);
            foreach (var item in weights)
            {
                if (randomNumber < item.Value)
                {
                    selectedItems.Add(item.Key);
                    break;
                }
                randomNumber -= item.Value;
            }
        }

        return selectedItems;
    }

    private static void Shuffle<T>(IList<T> list)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = _random.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}