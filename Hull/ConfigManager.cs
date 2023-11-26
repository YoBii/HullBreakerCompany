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
        public static int GetEventCount()
        {
            EnsureConfigExists();
            return _configFile.Bind("Settings", "EventCount", 3).Value;
        }
        public static Dictionary<GameEvents, int> GetWeights()
        {
            EnsureConfigExists();

            return new Dictionary<GameEvents, int>()
            {
                { GameEvents.Nothing, _configFile.Bind("Weights", "Nothing", 30).Value },
                { GameEvents.FlowerMan, _configFile.Bind("Weights", "FlowerMan", 20).Value },
                { GameEvents.SpringMan, _configFile.Bind("Weights", "SpringMan", 10).Value },
                { GameEvents.HoarderBug, _configFile.Bind("Weights", "HoarderBug", 50).Value },
                { GameEvents.Turret, _configFile.Bind("Weights", "Turret", 10).Value },
                { GameEvents.LandMine, _configFile.Bind("Weights", "LandMine", 30).Value },
                { GameEvents.OutSideEnemyDay, _configFile.Bind("Weights", "OutSideEnemyDay", 10).Value },
                { GameEvents.Lizards, _configFile.Bind("Weights", "Lizards", 15).Value },
                { GameEvents.Arachnophobia, _configFile.Bind("Weights", "Arachnophobia", 20).Value },
                { GameEvents.Bee, _configFile.Bind("Weights", "Bee", 30).Value },
                { GameEvents.Slime, _configFile.Bind("Weights", "Slime", 20).Value },
                { GameEvents.DevochkaPizdec, _configFile.Bind("Weights", "DevochkaPizdec", 4).Value },
                { GameEvents.EnemyBounty, _configFile.Bind("Weights", "EnemyBounty", 50).Value },
                { GameEvents.OneForAll, _configFile.Bind("Weights", "OneForAll", 10).Value },
                { GameEvents.OpenTheNoor, _configFile.Bind("Weights", "OpenTheNoor", 20).Value },
                { GameEvents.OnAPowderKeg, _configFile.Bind("Weights", "OnAPowderKeg", 10).Value },
                { GameEvents.Hell, _configFile.Bind("Weights", "Hell", 1).Value }
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