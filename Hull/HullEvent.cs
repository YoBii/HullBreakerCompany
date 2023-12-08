using System;
using System.Collections.Generic;
using BepInEx.Configuration;

namespace HullBreakerCompany.Hull;

public abstract class HullEvent
{ 
    public abstract string ID();
    public virtual int GetWeight()
    {
        return 1;
    }
    public virtual string GetDescription() => "Default description";
    public virtual string GetMessage() => "Default message";
    public virtual string GetShortMessage() => "SHORT";
    public virtual void Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity) { }
}