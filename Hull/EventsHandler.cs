using System;
using System.Collections.Generic;
using System.Text;
using GameNetcodeStuff;
using HarmonyLib;

namespace HullBreakerCompany.Hull;

public abstract class EventsHandler
{
    public static bool BountyIsActive;
    public static int BountyRewards;

    public static bool OneForAllIsActive;
    public static bool BountyFirstKill;
    public static bool MeltdownActive;
    public static bool OnAPowderKegActive;
    
    public static void Reset() {
        Plugin.Mls.LogInfo($"Resetting EventsHandler variables.");
        BountyIsActive = false;
        BountyRewards = 0;
        OneForAllIsActive = false;
        MeltdownActive = false;
        OnAPowderKegActive = false;
    }

    // PATCHES

    //EnemyBounty
    [HarmonyPostfix]
    [HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.KillEnemy))]
    static void EnemyBounty(bool destroy)
    {
        Plugin.Mls.LogInfo($"Enemy killed, instance isHost: {RoundManager.Instance.IsHost}");
        if (!RoundManager.Instance.IsHost) return;
        Plugin.Mls.LogInfo($"Enemy killed, bounty is active: {BountyIsActive}; destroy is {destroy}");
        if (BountyIsActive && !destroy) {
            int bountyReward = UnityEngine.Random.Range(Plugin.BountyRewardMin, Plugin.BountyRewardMax);
            BountyRewards++;
            // chat print reward. Detailed on 1st kill, abbreviated after
            if (BountyFirstKill) {
                // string building
                List<String> rewardMessages = new() {
                    { "Threat neutralized! Keep up the good work. The company sends [AMOUNT] credits." },
                    { "Enemy killed! Your work keeps the company happy. You receive [AMOUNT] credits." },
                    { "Monster killed! The company values your commitment. [AMOUNT] credits received." },
                };
                StringBuilder rewardString = new("<color=white>" + rewardMessages[UnityEngine.Random.Range(0, rewardMessages.Count)] + "</color>");
                rewardString.Replace("[AMOUNT]", "</color><color=green>" + bountyReward.ToString() + "</color><color=white>");
                HullManager.SendChatEventMessage(rewardString.ToString());
                BountyFirstKill = false;
            } else if (Plugin.BountyRewardLimit > 0 && BountyRewards >= Plugin.BountyRewardLimit){
                BountyIsActive = false;
                bountyReward = (int) Math.Floor(Plugin.BountyRewardMax * 1.5f);
                Plugin.Mls.LogInfo("<color=white>Bounty complete! You receive </color><color=green>" + bountyReward + "</color><color=white> credits. Your handwork is invaluable to the company.");
            } else {
                HullManager.SendChatEventMessage("<color=white>Bounty reward: </color><color=green>" + bountyReward + "</color><color=white> credits</color>");
            }
            HullManager.Instance.AddMoney(bountyReward);
            Plugin.Mls.LogInfo($"Bounty credits rewarded: {bountyReward}");
        }
    }

    //OneForAll event
    //TODO fix
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerControllerB), "KillPlayerServerRpc")]
    static void OneForAll()
    {
        if (!RoundManager.Instance.IsHost) return;
        Plugin.Mls.LogInfo($"Player killed, one for all is active: {OneForAllIsActive}");
        if (!OneForAllIsActive) return;
        OneForAllIsActive = false;
        // HullManager gc = UnityEngine.Object.FindObjectOfType<HullManager>();
        // gc.timeOfDay.votedShipToLeaveEarlyThisRound = true;
        // gc.timeOfDay.SetShipLeaveEarlyServerRpc();
        HullManager.Instance.timeOfDay.votedShipToLeaveEarlyThisRound = true;
        HullManager.Instance.timeOfDay.SetShipLeaveEarlyServerRpc();

        HullManager.SendChatEventMessage(
            "<color=red>Employee signal lost! Ship will leave in two hours!</color>");
    }
}