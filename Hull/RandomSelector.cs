using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.hull;

namespace HullBreakerCompany.Hull;

public abstract class RandomSelector
{
    private static Random _random = new();
    public static List<string> GetRandomGameEvents()
    {
        var increaseEventCountPerDay = ConfigManager.GetConfigValue("IncreaseEventCountPerDay", false, "The number of events will increase every day. Visit the company building to reset");
        var eventCount = increaseEventCountPerDay ? Plugin.DaysPassed : ConfigManager.GetConfigValue("EventCount", 3, "The number of events that will be active at the same time");
        
        var gameEvents = GetWeightedRandomGameEvents(ConfigManager.GetWeights(), eventCount);
        return gameEvents;
    }

    private static List<T> GetWeightedRandomGameEvents<T>(Dictionary<T, int> weights, int count)
    {
        var totalWeight = weights.Where(x => x.Value > 0).Sum(x => x.Value);
        var selectedItems = new HashSet<T>();

        while (selectedItems.Count < count)
        {
            var randomNumber = _random.Next(totalWeight);
            foreach (var item in weights)
            {
                if (item.Value == 0)
                {
                    continue;
                }

                if (randomNumber < item.Value)
                {
                    selectedItems.Add(item.Key);
                    break;
                }
                randomNumber -= item.Value;
            }
        }

        return selectedItems.ToList();
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