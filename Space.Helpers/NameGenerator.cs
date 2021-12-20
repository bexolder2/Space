using Space.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Space.Helpers
{
    public class NameGenerator : IGenerator<string>
    {
        private const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int nameLength = 9;

        public string Generate(List<string> args = null)
        {
            Random rnd = new Random();
            StringBuilder result = new StringBuilder();

            for(int i = 0; i < nameLength; i++)
            {
                result.Append(alphabet[rnd.Next(0, alphabet.Length)]);
            }

            return result.ToString();
        }
    }
}
