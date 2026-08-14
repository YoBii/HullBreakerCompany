using HullBreakerCompany.Hull;
using System.Collections.Generic;

namespace HullBreakerCompany.Events.Enemy;

public class ButlerEvent : HullEvent
{
    public ButlerEvent()
    {
        ID = "Butler";
        Weight = 5;
        Description = "Increases spawn chance of butler and there's more of them";
        MessagesList = new List<string>() {
            { "Reports of paranormal activity" },
            { "Paranormal signature detected" },
            { "Someone is taking care of this place" },
            { "Mirror finished floors require meticulous care.."},
            { "Avoid stained clothing by not staining the floors. Suits are company property!" }
        };
        shortMessagesList = new List<string>() {
            { "BUTLER" },
            { "JANITOR" }
        };
    }

    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsEnemySpawnable(Util.getEnemyByType(typeof(ButlerEnemyAI))))
        {
            return false;
        }
        levelModifier.AddEnemyComponentRarity(Util.getEnemyByType(typeof(ButlerEnemyAI)), 100);
        levelModifier.AddEnemyComponentMaxCount(Util.getEnemyByType(typeof(ButlerEnemyAI)), 4);
        levelModifier.AddEnemyComponentPower(Util.getEnemyByType(typeof(ButlerEnemyAI)), 0);
        if (Plugin.ColoredEventMessages)
        {
            HullManager.AddChatEventMessageColored(this, "red");
        }
        else
        {
            HullManager.AddChatEventMessage(this);
        }
        return true;
    }
}
