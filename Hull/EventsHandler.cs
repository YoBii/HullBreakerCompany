using System;
using System.Collections.Generic;
using System.Text;
using GameNetcodeStuff;
using HarmonyLib;

namespace HullBreakerCompany.Hull;

public abstract class EventsHandler
{
    //EnemyBounty
    [HarmonyPostfix]
    [HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.KillEnemy))]
    static void EnemyBounty(bool destroy)
    {
        Plugin.Mls.LogInfo($"Enemy killed, instance isHost: {RoundManager.Instance.IsHost}");
        if (!RoundManager.Instance.IsHost) return;
        Plugin.Mls.LogInfo($"Enemy killed, bounty is active: {Plugin.BountyIsActive}; destroy is {destroy}");
        if (Plugin.BountyIsActive && !destroy) {
            // use System.Random as workaround for diversity breaking UnityEngine.Random
            System.Random rnd = new();
            int bountyReward = rnd.Next(Plugin.BountyRewardMin, Plugin.BountyRewardMax);
            HullManager.Instance.AddMoney(bountyReward);
            // chat print reward. Detailed on 1st kill, abbreviated after
            if (Plugin.BountyFirstKill) {
                // string building
                List<String> rewardMessages = new() {
                    { "Threat neutralized! Keep up the good work. The company sends [AMOUNT] credits." },
                    { "Enemy killed! Your work keeps the company happy. You receive [AMOUNT] credits." },
                    { "Bounty success! Your handwork is invaluable to the company. [AMOUNT] credits rewarded." },
                    { "Monster killed! The company values your commitment. [AMOUNT] credits received." },
                };
                StringBuilder rewardString = new("<color=white>" + rewardMessages[UnityEngine.Random.Range(0, rewardMessages.Count)] + "</color>");
                rewardString.Replace("[AMOUNT]", "</color><color=green>" + bountyReward.ToString() + "</color><color=white>");

                HullManager.SendChatEventMessage(rewardString.ToString());
                Plugin.BountyFirstKill = false;
            } else {
                HullManager.SendChatEventMessage("<color=white>Bounty reward: </color><color=green>" + bountyReward + "</color><color=white> credits</color>");
            }
            Plugin.Mls.LogInfo($"Bounty credits rewarded: {bountyReward}");
        }
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