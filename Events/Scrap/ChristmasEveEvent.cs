using System;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events.Scrap;

public class ChristmasEveEvent : HullEvent
{
    public override string ID() => "ChristmasEve";
    public override int GetWeight() => 20;
    public override string GetDescription() => "Spawns a lot of gifts.";
    public static List<string> MessagesList = new() {
        { "Is it Christmas already?" },
        { "Help Santa collect the presents!" },
        { "The Company wishes a Merry Christmas!" }
    };
    public static List<string> shortMessagesList = new() {
        { "XMAS" }
    };
    public override string GetMessage() => "<color=white>" + MessagesList[UnityEngine.Random.Range(0, MessagesList.Count)] + "</color>";
    public override string GetShortMessage() => "<color=white>" + shortMessagesList[UnityEngine.Random.Range(0, shortMessagesList.Count)] + "</color>";
    public override bool Execute(SelectableLevel level, LevelModifier levelModifier) {
        string scrapToSpawn = "Gift";
        if (levelModifier.IsScrapSpawnable(scrapToSpawn)) {
            levelModifier.AddSpawnableScrapRarity(scrapToSpawn, 1500);
            HullManager.AddChatEventMessage(this);
            return true;
        } else {
            return false;
        }
    }
}