using System;
using System.Collections.Generic;
using HullBreakerCompany.Hull;

namespace HullBreakerCompany.Events;

public class NutcrackerEvent : HullEvent
{
    public override string ID() => "Nutcracker";
    public override int GetWeight() => 5;
    public override string GetDescription() => "Increased chance of NutCracker spawn";
    public override string GetMessage() => "<color=white>Detected signs of paranormal acitivy</color>";
    public override string GetShortMessage() => "<color=white>PARANORMAL</color>";

    public override void Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity)
    {
        enemyComponentRarity.Add(typeof(NutcrackerEnemyAI), 64);
        HullManager.SendChatEventMessage(this);
    }
}