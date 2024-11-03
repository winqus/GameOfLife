using GameOfLifeApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeApp.Services
{
    public class SeedGenerator : ISeedGenerator
    {
        /// <summary>
        /// Generates a new seed for the game initial cell states.
        /// </summary>
        public string GenerateNewSeed(int seedLength)
        {
            if (seedLength <= 0)
            {
                throw new ArgumentOutOfRangeException("seedLength must be more than 0");
            }

            Random random = new Random();

            const string charValues = "01";
            return new string(
                Enumerable.Repeat(charValues, seedLength).Select(c => charValues[random.Next(charValues.Length)]).ToArray()
            );
        }
    }
}
