using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class DevochkaPizdecEvent : HullEvent
{
    public override string ID() => "DevochkaPizdec";
    public override int GetWeight() => 5;
    public override string GetDescription() => "Increased chance of phantom girl spawn";
    public override string GetMessage() => "<color=white>Detected signs of paranormal acitivy</color>";
    public override string GetShortMessage() => "<color=white>PARANORMAL</color>";
    public override void Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        bool enemyExists = level.Enemies.Exists(enemy => enemy.GetType() == typeof(DressGirlAI));
        if (!enemyExists) return;
        
        enemyComponentRarity.Add(typeof(DressGirlAI), 32);
        HullManager.SendChatEventMessage(this);
    }
}