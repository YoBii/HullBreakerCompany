using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;

namespace HullBreakerCompany.Hull;

public abstract class RandomSelector
{
    private static readonly Random _random = new();

    private static Dictionary<string, int> weights = new();

    public static List<string> GetRandomGameEvents(int count)
    {
        weights = ConfigManager.GetWeights();
        
        var gameEvents = GetWeightedRandomGameEvents(weights, count);

        // remove selected events from pool - no duplicate events
        foreach (var e in gameEvents ) {
            if (e == "NothingEvent") continue;
            weights.Remove(e);
        }
        return gameEvents;
    }
    
    /// <summary>
    /// Returns one randomly selected event 
    /// </summary>
    public static string GetAnotherRandomGameEvent() {
        if (weights.Count == 0) {
            return null;
        }
        string newEvent = GetWeightedRandomGameEvents(weights, 1)[0];
        if (newEvent != "Nothing") weights.Remove(newEvent);
        return newEvent; 
    }
    private static List<T> GetWeightedRandomGameEvents<T>(Dictionary<T, int> weights, int count)
    {
        var totalWeight = weights.Where(x => x.Value > 0).Sum(x => x.Value);
        var selectedItems = new HashSet<T>();
        
        if (totalWeight < 1) {
            return selectedItems.ToList();
        }

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