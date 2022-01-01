using Space.Model.Enums;
using Space.Model.Modules;
using System.Collections.Generic;

namespace Space.Infrastructure.Helpers
{
    public class ShipPropertyCounter
    {
        public static int GetEnergyСonsumption(ref Player player)
        {
            int result = 0;
            List<Engine> engines = new List<Engine>();
            List<Generator> generators = new List<Generator>();

            var tmp = GetEnginesAndGenerators(ref player);
            engines = tmp.Key;
            generators = tmp.Value;

            if (engines.Count > 0 || generators.Count > 0)
            {
                int maxEnginesDistance = 0;
                int maxGeneratorsEnergy = 0;
                foreach (var engine in engines)
                {
                    maxEnginesDistance += engine.EnergyConsymptionPer100Kilometers;
                }
                foreach (var generator in generators)
                {
                    maxGeneratorsEnergy += generator.Efficiency.Power;
                }
                result = maxEnginesDistance - maxGeneratorsEnergy;
            }

            return result;
        }

        public static int CountAvailableDistance(ref Player player)
        {
            int result = 0;
            List<Engine> engines = new List<Engine>();
            List<Generator> generators = new List<Generator>();

            var tmp = GetEnginesAndGenerators(ref player);
            engines = tmp.Key;
            generators = tmp.Value;

            if (engines.Count > 0 || generators.Count > 0)
            {
                int maxEnginesDistance = 0;
                int maxGeneratorsEnergy = 0;
                foreach (var engine in engines)
                {
                    maxEnginesDistance += engine.EnergyConsymptionPer100Kilometers;
                }
                foreach (var generator in generators)
                {
                    maxGeneratorsEnergy += generator.Efficiency.Power;
                }

                double powerValue = player.Resources.EnergyValue;
                while (powerValue > 0)
                {
                    powerValue = powerValue - maxEnginesDistance + maxGeneratorsEnergy;
                    result += 100;
                }
            }
            player.Spaceship.AvailablePowerReserve = result;

            return result;
        }

        private static KeyValuePair<List<Engine>, List<Generator>> GetEnginesAndGenerators(ref Player player)
        {
            List<Engine> engines = new List<Engine>();
            List<Generator> generators = new List<Generator>();

            foreach (var item in player.Spaceship.ShipModules)
            {
                if (item.Value == Module.Engine)
                {
                    engines.Add((Engine)item.Key);
                }
                else if (item.Value == Module.Generator)
                {
                    generators.Add((Generator)item.Key);
                }
            }

            return new KeyValuePair<List<Engine>, List<Generator>>(engines, generators);
        }

        public static int CountDamageValue(ref Spaceship spaceship)
        {
            int result = 0;

            foreach(var item in spaceship.ShipModules)
            {
                if (item.Value == Module.Gun)
                {
                    result += ((Gun)item.Key).Damage;
                }
            }
            spaceship.Damage = result;

            return result;
        }

        public static int CountHPValue(ref Spaceship spaceship)
        {
            int result = 0;

            foreach(var item in spaceship.ShipModules)
            {
                result += ((BaseModel)item.Key).HP;
            }
            spaceship.HP = result;

            return result;
        }
    }
}
