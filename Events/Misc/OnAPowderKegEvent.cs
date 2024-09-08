using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class OnAPowderKegEvent : HullEvent
{
    public OnAPowderKegEvent() {
        ID = "OnAPowderKeg";
        Weight = 10;
        Description = "Landmines will detonate randomly. Spawns additional landmines.";
        MessagesList = new List<string>() {
            { "Reccuring explosions" },
            { "Explosions detected" },
            { "They installed faulty mines" },
            { "Mines explode at the slightest vibration" },
            { "Tick, tick.. BOOM!" }
        };
        shortMessagesList = new List<string>() {
            { "SURPRISE" },
            { "EXPLOSIONS" }
        };
    }
    public int dayInSeconds;
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsTrapUnitSpawnable(Util.getTrapUnitByType(typeof(Landmine)))) return false;
        levelModifier.AddTrapUnit(Util.getTrapUnitByType(typeof(Landmine)), (int)(Plugin.LandmineScale * 2 / 3));

        dayInSeconds = (int)HullManager.Instance.timeOfDay.lengthOfHours * HullManager.Instance.timeOfDay.numberOfHours;
        HullManager.Instance.ExecuteAfterDelay(() => { DetonateLandMine(level); }, UnityEngine.Random.Range(dayInSeconds / 6, dayInSeconds / 2));
        
        EventsHandler.OnAPowderKegActive = true;
        if (Plugin.ColoredEventMessages) {
            HullManager.AddChatEventMessageColored(this, "red");
        } else {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }

    private async void DetonateLandMine(SelectableLevel level)
    {
        if (level == null)
        {
            Plugin.Mls.LogError("level is null");
            return;
        }
        Landmine[] landmines = UnityEngine.Object.FindObjectsOfType<Landmine>();
        if (landmines.Count() == 0) return;

        Plugin.Mls.LogInfo(GetID() + $" Event: PowderKeg has been ignited. Will explode a landmine every {dayInSeconds / landmines.Count() / 30}-{dayInSeconds / landmines.Count() / 4} seconds.");
        foreach (var (landmine, i) in landmines.Select((landmine, i) => (landmine, i)))
        {
            if (landmine == null) continue;
            if (!landmine.IsSpawned) continue;
            if (!EventsHandler.OnAPowderKegActive || TimeOfDay.Instance.playersManager.inShipPhase) {
                Plugin.Mls.LogInfo($"OnAPowderKeg Event abort. Reason: OnAPowderKegActive: {EventsHandler.OnAPowderKegActive}; " +
                    $"inShipPhase: {TimeOfDay.Instance.playersManager.inShipPhase};");
                break;
            }
            Plugin.Mls.LogMessage(GetID() + $" Event: Random landmine explosion #{i + 1}");
            landmine.ExplodeMineServerRpc();
            await Task.Delay(TimeSpan.FromSeconds(UnityEngine.Random.Range(dayInSeconds / landmines.Count() / 30, dayInSeconds / landmines.Count() / 4)));
        }
    }
}