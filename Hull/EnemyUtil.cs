using System;
using System.Collections.Generic;
using System.Text;

namespace HullBreakerCompany.Hull {
    internal class EnemyUtil {
        private  static Dictionary<String, Type> EnemyBase = new()
        {
            { "flowerman", typeof(FlowermanAI) },
            { "hoarderbug", typeof(HoarderBugAI) },
            { "springman", typeof(SpringManAI) },
            { "crawler", typeof(CrawlerAI) },
            { "sandspider", typeof(SandSpiderAI) },
            { "jester", typeof(JesterAI) },
            { "centipede", typeof(CentipedeAI) },
            { "blobai", typeof(BlobAI) },
            { "dressgirl", typeof(DressGirlAI) },
            { "pufferenemy", typeof(PufferAI) },
            { "eyelessdogs", typeof(MouthDogAI) },
            { "forestgiant", typeof(ForestGiantAI) },
            { "sandworm", typeof(SandWormAI) },
            { "baboonbird", typeof(BaboonBirdAI) },
            { "nutcrackerenemy", typeof(NutcrackerEnemyAI)},
            { "maskedplayerenemy", typeof(MaskedPlayerEnemy)}
        };

        private static Dictionary<Type, String> EnemiesByType = new() {
            { typeof(FlowermanAI), "Flowerman" },
            { typeof(HoarderBugAI), "HoarderBug" },
            { typeof(SpringManAI), "SpringMan" },
            { typeof(CrawlerAI), "Crawler" },
            { typeof(SandSpiderAI), "Sandspider" },
            { typeof(JesterAI), "Jester" },
            { typeof(CentipedeAI), "Centipede" },
            { typeof(BlobAI), "Blob" },
            { typeof(DressGirlAI), "DressGirl" },
            { typeof(PufferAI), "PufferEnemy" },
            { typeof(MouthDogAI), "MouthDog" },
            { typeof(ForestGiantAI), "ForestGiant" },
            { typeof(SandWormAI), "SandWorm" },
            { typeof(BaboonBirdAI), "baboonBird" },
            { typeof(NutcrackerEnemyAI), "NutcrackerEnemy"},
            { typeof(MaskedPlayerEnemy), "MaskedPlayerEnemy"},
            { typeof(RedLocustBees), "RedLocustBees" }
        };

        public static string getEnemyByType (Type type) {
            try {
                EnemiesByType.TryGetValue(type, out var enemy);
                return enemy;
            } catch {
                return null;
            }
        }
        public static Type getEnemyByString(String str) {
            try {
                EnemyBase.TryGetValue(str, out var enemy);
                return enemy;
            } catch {
                return null;
            }
        }
    }
}
