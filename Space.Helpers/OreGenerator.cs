﻿using Space.Helpers.Interfaces;
using System;
using System.Collections.Generic;

namespace Space.Helpers
{
    public class OreGenerator : IGenerator<int>
    {
        private const int minValue = 100;
        private const int maxValue = 1001;

        public int Generate(List<int> args = null)
        {
            Random rnd = new Random();
            return rnd.Next(minValue, maxValue);
        }
    }
}
