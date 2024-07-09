using HullBreakerCompany.Events.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using UnityEngine.UIElements;

namespace HullBreakerCompany.Hull;

public abstract class RandomSelector
{
    private static readonly Random _random = new();

    private static Dictionary<string, int> weights = new();

    public static void InitializeWeights() {
        weights = ConfigManager.GetWeights();
    }  
    /// <summary>
    /// Returns one random weighted event 
    /// </summary>
    public static string GetRandomGameEvent() {
        var totalWeight = weights.Where(x => x.Value > 0).Sum(x => x.Value);
        if (totalWeight < 1) {
            return null;
        }
        string randomGameEvent = "";

        var rnd = _random.Next(totalWeight);
        foreach (var ev in weights) {
            if (ev.Value == 0) {
                continue;
            }
            if (rnd < ev.Value) {
                randomGameEvent = ev.Key;
                break;
            }
            rnd -= ev.Value;
        }
        if (string.IsNullOrEmpty(randomGameEvent)) return null;
        // remove from pool unless NothingEvent - no duplicate events
        if (randomGameEvent != "Nothing") weights.Remove(randomGameEvent);
        return randomGameEvent; 
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