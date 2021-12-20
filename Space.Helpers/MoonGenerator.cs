using Space.Helpers.Interfaces;
using System;
using System.Collections.Generic;

namespace Space.Helpers
{
    public class MoonGenerator : IGenerator<int>
    {
        private const int minValue = 0;
        private const int maxValue = 4;

        public int Generate(List<int> args = null)
        {
            Random rnd = new Random();
            return rnd.Next(minValue, maxValue);
        }
    }
}
