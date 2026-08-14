using FacilityMeltdown.API;
using HullBreakerCompany.Hull;
using System.Collections.Generic;

namespace HullBreakerCompany.Events.Misc;

public class MeltdownEvent : HullEvent
{
    public MeltdownEvent()
    {
        ID = "Meltdown";
        Weight = 10;
        Description = "Starts reactor meltdown sequence sometime during the day.";
        MessagesList = new List<string>() {
        { "Unstable reactor core. Extract and leave immediately!" },
        { "Elevated radiation levels. Reactor meltdown imminent!" },
        { "Prevent a nuclear disaster by gracefully removing the apparatus core" },
        { "Increased radiation levels. Don't wander too far off the ship." }
    };
        shortMessagesList = new List<string>() {
        { "SURPRISE" },
        { "MELTDOWN" }
    };
    }
    public int dayInSeconds;
    public int currentDaysLeft;
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        currentDaysLeft = TimeOfDay.Instance.daysUntilDeadline;
        dayInSeconds = (int)HullManager.Instance.timeOfDay.lengthOfHours * HullManager.Instance.timeOfDay.numberOfHours;
        if (Plugin.ColoredEventMessages)
        {
            HullManager.AddChatEventMessageColored(this, "red");
        }
        else
        {
            HullManager.AddChatEventMessage(this);
        }
        HullManager.Instance.ExecuteAfterDelay(() => { StartMeltdown(); }, UnityEngine.Random.Range(dayInSeconds * 0.3f, dayInSeconds * 0.8f));
        EventsHandler.MeltdownActive = true;
        return true;
    }

    private void StartMeltdown()
    {

        if (!EventsHandler.MeltdownActive || TimeOfDay.Instance.playersManager.inShipPhase || RoundManager.Instance.currentLevel.levelID == 3)
        {
            Plugin.Mls.LogInfo($"Meltdown Event abort. Reason: MeltdownActive: {EventsHandler.MeltdownActive}; " +
                $"inShipPhase: {TimeOfDay.Instance.playersManager.inShipPhase}; " +
                $"currentLevel: {RoundManager.Instance.currentLevel.PlanetName} (ID {RoundManager.Instance.currentLevel.levelID})");
            return;
        }
        Plugin.Mls.LogInfo(GetID() + $" Event: Meltdown initiated");
        MeltdownAPI.StartMeltdown(MyPluginInfo.PLUGIN_GUID);
        string warningMessage = LocaleManager.Get("events.Meltdown.warningMessage") ?? "NUCLEAR MELTDOWN! ABANDON MISSION!";
        HullManager.SendChatEventMessage("<color=red>" + warningMessage + "</color>");
    }
}