using System;
using System.Collections.Generic;
using HullBreakerCompany.Event;
using HullBreakerCompany.hull;

namespace HullBreakerCompany.Events;

public class BabkinPogrebEvent : HullEvent
{
    public override string ID() => "BabkinPogreb";
    public override int GetWeight() => 10;
    public override string GetDescription() => "Only jars of pickles spawn on the moon";
    public override string GetMessage() => "<color=white>On the this moon, something strange happened with scrap...</color>";
    public override string GetShortMessage() => "<color=white>ITEM MYSTERY</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity)
    {
        level.spawnableScrap.RemoveAll(item => item.spawnableItem.itemName != "Jar of pickles");
        level.spawnableScrap[0].rarity = 100;
        
        HullManager.SendChatEventMessage(this);
    }

}