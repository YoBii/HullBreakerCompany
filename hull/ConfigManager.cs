using BepInEx.Configuration;
using System.Collections.Generic;
using System.IO;
using BepInEx;

namespace HullBreakerCompany.hull
{
    public class ConfigManager
    {
        private static string _configPath = Path.Combine(Paths.ConfigPath, "HullBreakerCompany.cfg");
        private static ConfigFile _configFile;

        public static Dictionary<GameEvents, int> GetWeights()
        {
            EnsureConfigExists();

            return new Dictionary<GameEvents, int>()
            {
                { GameEvents.Nothing, _configFile.Bind("Weights", "Nothing", 1).Value },
                { GameEvents.FlowerMan, _configFile.Bind("Weights", "FlowerMan", 2).Value },
                { GameEvents.SpringMan, _configFile.Bind("Weights", "SpringMan", 1).Value },
                { GameEvents.HoarderBug, _configFile.Bind("Weights", "HoarderBug", 4).Value },
                { GameEvents.Turret, _configFile.Bind("Weights", "Turret", 1).Value },
                { GameEvents.LandMine, _configFile.Bind("Weights", "LandMine", 4).Value },
                { GameEvents.OutSideEnemyDay, _configFile.Bind("Weights", "OutSideEnemyDay", 1).Value },
                { GameEvents.Lizards, _configFile.Bind("Weights", "Lizards", 1).Value },
                { GameEvents.Arachnophobia, _configFile.Bind("Weights", "Arachnophobia", 1).Value },
                { GameEvents.Bee, _configFile.Bind("Weights", "Bee", 2).Value },
                { GameEvents.Slime, _configFile.Bind("Weights", "Slime", 1).Value },
                { GameEvents.DevochkaPizdec, _configFile.Bind("Weights", "DevochkaPizdec", 1).Value },
                { GameEvents.EnemyBounty, _configFile.Bind("Weights", "EnemyBounty", 4).Value },
                { GameEvents.OneForAll, _configFile.Bind("Weights", "OneForAll", 2).Value },
                { GameEvents.OpenTheNoor, _configFile.Bind("Weights", "OpenTheNoor", 3).Value },
                { GameEvents.OnAPowderKeg, _configFile.Bind("Weights", "OnAPowderKeg", 1).Value }
            };
        }

        private static void EnsureConfigExists()
        {
            if (_configFile == null)
            {
                if (!File.Exists(_configPath))
                {
                    CreateDefaultConfigFile();
                }
                _configFile = new ConfigFile(_configPath, true);
            }
        }

        private static void CreateDefaultConfigFile()
        {
            using StreamWriter sw = File.CreateText(_configPath);
            sw.WriteLine("[Weights]");
            sw.WriteLine("Nothing = 1");
            sw.WriteLine("FlowerMan = 2");
            sw.WriteLine("SpringMan = 1");
            sw.WriteLine("HoarderBug = 4");
            sw.WriteLine("Turret = 1");
            sw.WriteLine("LandMine = 4");
            sw.WriteLine("OutSideEnemyDay = 1");
            sw.WriteLine("Lizards = 1");
            sw.WriteLine("Arachnophobia = 1");
            sw.WriteLine("Bee = 2");
            sw.WriteLine("Slime = 1");
            sw.WriteLine("DevochkaPizdec = 1");
            sw.WriteLine("EnemyBounty = 4");
            sw.WriteLine("OneForAll = 2");
            sw.WriteLine("OpenTheNoor = 3");
            sw.WriteLine("OnAPowderKeg = 1");
        }
    }
}