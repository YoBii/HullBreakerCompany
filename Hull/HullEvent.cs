using System;
using System.Collections.Generic;

namespace HullBreakerCompany.Event;

public abstract class HullEvent
{
    
    public abstract string ID();
    public virtual void Execute(SelectableLevel level, Dictionary<Type, int> componentRarity) { }
}