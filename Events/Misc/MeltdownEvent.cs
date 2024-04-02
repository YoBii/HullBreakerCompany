using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FacilityMeltdown.API;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Misc;

public class MeltdownEvent : HullEvent
{
    public int dayInSeconds;
    public int currentDaysLeft;
    public override string ID() => "Meltdown";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Starts reactor meltdown sequence sometime during the day.";
    public static List<string> MessagesList = new() {
        { "Unstable reactor core. Extract and leave immediately!" },
        { "Elevated radiation levels. Reactor meltdown imminent!" },
        { "Prevent a nuclear disaster by gracefully removing the apparatus core" },
        { "Increased radiation levels. Don't wander too far off the ship." }
    };
    public static List<string> shortMessagesList = new() {
        { "SURPRISE" },
        { "MELTDOWN" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        currentDaysLeft = TimeOfDay.Instance.daysUntilDeadline;
        dayInSeconds = (int)HullManager.Instance.timeOfDay.lengthOfHours * HullManager.Instance.timeOfDay.numberOfHours;
        HullManager.AddChatEventMessage(this);
        HullManager.Instance.ExecuteAfterDelay(() => { StartMeltdown(); }, UnityEngine.Random.Range(dayInSeconds * 0.3f, dayInSeconds * 0.8f));
        EventsHandler.MeltdownActive = true;
        return true;
    }

    private void StartMeltdown() {

        if (!EventsHandler.MeltdownActive || TimeOfDay.Instance.playersManager.inShipPhase || RoundManager.Instance.currentLevel.levelID == 3) {
            Plugin.Mls.LogInfo($"Meltdown Event abort. Reason: MeltdownActive: {EventsHandler.MeltdownActive}; " +
                $"inShipPhase: {TimeOfDay.Instance.playersManager.inShipPhase}; " +
                $"currentLevel: {RoundManager.Instance.currentLevel.PlanetName} (ID {RoundManager.Instance.currentLevel.levelID}");
            return;
        }
        Plugin.Mls.LogInfo(ID() + $" Event: Meltdown initiated");
        MeltdownAPI.StartMeltdown(PluginInfo.PLUGIN_GUID);
        HullManager.SendChatEventMessage("<color=red>NUCLEAR MELTDOWN IMMINENT! ABANDON MISSION!</color>");
    }
}