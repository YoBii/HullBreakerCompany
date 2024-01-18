using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;


namespace HullBreakerCompany.Hull;

public abstract class EventsHandler
{
    //EnemyBounty
    [HarmonyPostfix]
    [HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.KillEnemyServerRpc))]
    static void EnemyBounty()
    {
        Plugin.Mls.LogInfo($"Enemy killed, bounty is active: {Plugin.BountyIsActive}");
        if (!Plugin.BountyIsActive) return;
        Terminal tl = UnityEngine.Object.FindObjectOfType<Terminal>();        
        int bountyReward = Random.Range(40, 80); // Make bounty reward amount configurable
        tl.groupCredits += bountyReward;
        tl.SyncGroupCreditsServerRpc(tl.groupCredits, tl.numberOfItemsInDropship);

        HullManager.SendChatEventMessage("<color=white>Enemy killed. Your work keeps the company happy. You receive </color><color=green>" + bountyReward + "</color><color=white> credits.</color>");
    }


    //OneForAll event
    //TODO fix
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerControllerB), "KillPlayerClientRpc")]
    static void OneForAll()
    {
        if (!RoundManager.Instance.IsHost) return;
        Plugin.Mls.LogInfo($"Player killed, one for all is active: {Plugin.OneForAllIsActive}");
        if (!Plugin.OneForAllIsActive) return;
        Plugin.OneForAllIsActive = false;
        HullManager gc = UnityEngine.Object.FindObjectOfType<HullManager>();
        gc.timeOfDay.votedShipToLeaveEarlyThisRound = true;
        gc.timeOfDay.SetShipLeaveEarlyServerRpc();

        HullManager.SendChatEventMessage(
            "<color=red>One of the workers died, the ship will go into orbit in an hour</color>");
    }
        
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameNetworkManager), nameof(GameNetworkManager.StartHost))]
    static void ResetDayPassed()
    {
        Plugin.CurrentMessage = "Events not found";
        Plugin.DaysPassed = 0;
    }
}