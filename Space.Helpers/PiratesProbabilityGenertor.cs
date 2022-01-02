using System;
using System.Collections.Generic;
using Space.Helpers.Interfaces;

namespace Space.Helpers
{
    public class PiratesProbabilityGenertor : IGenerator<bool>
    {
        private const int minValue = 0;
        private const int maxValue = 101;
        private const int piratesProbability = 40;

        public bool Generate(List<bool> args = null)
        {
            bool result = false;

            Random rnd = new Random();
            int val = rnd.Next(minValue, maxValue);

            if(val <= piratesProbability)
            {
                result = true;
            }

            return result;
        }
    }
}
