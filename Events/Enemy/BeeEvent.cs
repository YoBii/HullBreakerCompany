using HullBreakerCompany.Hull;
using System.Collections.Generic;

namespace HullBreakerCompany.Events.Enemy;

public class BeeEvent : HullEvent
{
    public BeeEvent()
    {
        ID = "Bee";
        Weight = 20;
        Description = "Increases bee hive spawns outside";
        MessagesList = new List<string>() {
            { "Dominated by hostile life form"},
            { "Dominant species detected"},
            { "Sticky golden goodness" },
            { "Extract the bee hives!" },
            { "Don't get stung by bees!" }
        };
        shortMessagesList = new List<string>() {
            { "BZZZZZ" },
            { "STICKY GOLD" }
        };
    }

    public override bool Execute(SelectableLevel level, LevelModifier levelModifier)
    {
        if (!levelModifier.IsDaytimeEnemySpawnable(Util.getEnemyByType(typeof(RedLocustBees))))
        {
            return false;
        }
        levelModifier.AddDaytimeEnemyComponentRarity(Util.getEnemyByType(typeof(RedLocustBees)), 1000);
        levelModifier.AddDaytimeEnemyComponentPower(Util.getEnemyByType(typeof(RedLocustBees)), 0);
        levelModifier.AddDaytimeEnemyComponentMaxCount(Util.getEnemyByType(typeof(RedLocustBees)), 10);
        levelModifier.AddDaytimeEnemySpawnChanceThroughoutDay(32);
        HullManager.AddChatEventMessage(this);
        return true;
    }
}