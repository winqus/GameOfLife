using GameOfLifeApp.Interfaces;
using GameOfLifeApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeApp.Tests
{
    public class SeedGeneratorTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(6)]
        [InlineData(8)]
        [InlineData(123)]
        [InlineData(123666)]
        public void GenerateNewSeed_Returns_CorrectSeedString(int input_seedLength)
        {
            // Arrange
            var generator = new SeedGenerator();
            var newSeed = new string("");

            // Act
            newSeed = generator.GenerateNewSeed(input_seedLength);

            // Arrange
            Assert.Equal(input_seedLength, newSeed.Length);
            Assert.Equal(input_seedLength, newSeed.Count(c => c == '0' || c == '1'));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-12)]
        public void GenerateNewSeed_Throws_ArgumentOutOfRangeException(int input_seedLength)
        {
            // Arrange
            var generator = new SeedGenerator();

            // Act
            Exception ex = Record.Exception(() =>
                generator.GenerateNewSeed(input_seedLength)
            );

            // Assert
            Assert.IsType<ArgumentOutOfRangeException>(ex);
        }
    }
}
