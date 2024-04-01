using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class OnAPowderKegEvent : HullEvent
{
    public int dayInSeconds;
    public override string ID() => "OnAPowderKeg";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Landmines can detonate at any time";
    public static List<string> MessagesList = new() {
        { "Reccuring explosions" },
        { "Explosions detected" },
        { "They installed faulty mines" },
        { "Mines explode at the slightest vibration" },
        { "Tick, tick.. BOOM!" }
    };
    public static List<string> shortMessagesList = new() {
        { "SURPRISE" },
        { "EXPLOSIONS" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        dayInSeconds = (int)HullManager.Instance.timeOfDay.lengthOfHours * HullManager.Instance.timeOfDay.numberOfHours;
        HullManager.AddChatEventMessage(this);
        levelModifier.AddLandmines(Plugin.LandmineScale * 2f / 3f);
        HullManager.Instance.ExecuteAfterDelay(() => { DetonateLandMine(level); }, UnityEngine.Random.Range(dayInSeconds / 6, dayInSeconds / 2));
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

        Plugin.Mls.LogInfo(ID() + $" Event: PowderKeg has been ignited. Will explode a landmine every {dayInSeconds / landmines.Count() / 30}-{dayInSeconds / landmines.Count() / 4} seconds.");
        foreach (var (landmine, i) in landmines.Select((landmine, i) => (landmine, i)))
        {
            Plugin.Mls.LogMessage(ID() + $" Event: Random landmine explosion #{i + 1}");
            landmine.ExplodeMineServerRpc();
            await Task.Delay(TimeSpan.FromSeconds(UnityEngine.Random.Range(dayInSeconds / landmines.Count() / 30, dayInSeconds / landmines.Count() / 4)));
        }
    }
}