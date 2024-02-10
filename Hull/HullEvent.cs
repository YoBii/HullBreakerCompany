using System;
using System.Collections.Generic;

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
    public virtual string GetShortMessage() => "Short message";
    public virtual bool Execute(SelectableLevel level, Dictionary<Type, int> enemyComponentRarity,
        Dictionary<Type, int> outsideComponentRarity) { return true; }
}