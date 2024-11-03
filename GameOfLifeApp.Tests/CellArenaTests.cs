using GameOfLifeApp.Enums;
using GameOfLifeApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace GameOfLifeApp.Tests
{
    public class CellArenaTests
    {
        [Theory]
        [InlineData(3, 3)]
        [InlineData(5, 4)]
        [InlineData(7, 3)]
        [InlineData(20, 5)]
        [InlineData(1000, 1000)]
        public void CreateEmptyArena_Assigns_CellArenaModel(int input_length, int input_height)
        {
            // Arrange
            var cellArena = new CellArena();
            var expectedCellCount = input_length * input_height;
            var expectedSeedLength = input_length * input_height;
            var expectedIteration = 0;
            var expectedArrayLengthOfDimension0 = input_height + 2;
            var expectedArrayLengthOfDimension1 = input_length + 2;

            // Act
            cellArena.CreateEmptyArena(input_length, input_height);

            // Assert
            Assert.NotNull(cellArena.ArenaData);
            Assert.Equal(input_length, cellArena.ArenaData.FieldLength);
            Assert.Equal(input_height, cellArena.ArenaData.FieldHeight);
            Assert.Equal(expectedSeedLength, cellArena.ArenaData.Seed.Length);
            Assert.Equal(expectedSeedLength, cellArena.ArenaData.Seed.Count(c => c == '0'));
            Assert.Equal(expectedIteration, cellArena.ArenaData.IterationCount);
            Assert.Equal(expectedArrayLengthOfDimension0, cellArena.ArenaData.CellField.GetLength(0));
            Assert.Equal(expectedArrayLengthOfDimension1, cellArena.ArenaData.CellField.GetLength(1));
            Assert.Equal(expectedCellCount, cellArena.ActiveCells.Count());
            Assert.True(cellArena.ActiveCells.All(c => c.State.Equals(CellState.Dead)));
        }

        [Theory]
        [InlineData(2, 3)]
        [InlineData(3, 2)]
        [InlineData(5, 1)]
        [InlineData(1, 1)]
        [InlineData(-7, 3)]
        [InlineData(20, -5)]
        [InlineData(0, 0)]
        [InlineData(5, 2)]
        [InlineData(2, 7777)]
        public void CreateEmptyArena_Throws_ArgumentException(int input_length, int input_height)
        {
            // Arrange
            var cellArena = new CellArena();

            // Act
            Exception ex = Record.Exception(() =>
                cellArena.CreateEmptyArena(input_length, input_height)
            );

            // Assert
            Assert.IsType<ArgumentException>(ex);
        }

        [Theory]
        [InlineData(3, 3, "111111111")]
        [InlineData(3, 3, "101100011")]
        [InlineData(5, 4, "10011100111001111111")]
        public void InitializeWithSeed_Assigns_Seed(int input_length, int input_height, string input_seed)
        {
            // Arrange
            var cellArena = new CellArena();
            cellArena.CreateEmptyArena(input_length, input_height);

            // Act
            cellArena.InitializeWithSeed(input_seed);

            // Assert
            Assert.True(cellArena.ArenaData.Seed.Equals(input_seed));
        }

        [Theory]
        [InlineData(3, 5, "111111111")]
        [InlineData(3, 3, "10010101010101111000000000000000000000000")]
        [InlineData(5, 11, "101100011")]
        [InlineData(52, 42, "10011100111001111111")]
        public void InitializeWithSeed_Throws_ArgumentException(int input_length, int input_height, string input_seed)
        {
            // Arrange
            var cellArena = new CellArena();
            cellArena.CreateEmptyArena(input_length, input_height);

            // Act
            Exception ex = Record.Exception(() =>
                cellArena.InitializeWithSeed(input_seed)
            );

            // Assert
            Assert.IsType<ArgumentException>(ex);
        }

        [Theory]
        [InlineData(3, 3, "1111a1111")]
        [InlineData(3, 4, "x11111111111")]
        [InlineData(4, 3, "a1111111111d")]
        [InlineData(3, 3, "aabbaaaaa")]
        [InlineData(5, 4, "10011100111001pop111")]
        public void InitializeWithSeed_Throws_Exception(int input_length, int input_height, string input_seed)
        {
            // Arrange
            var cellArena = new CellArena();
            cellArena.CreateEmptyArena(input_length, input_height);

            // Act
            Exception ex = Record.Exception(() =>
                cellArena.InitializeWithSeed(input_seed)
            );

            // Assert
            Assert.IsType<Exception>(ex);
        }

        [Theory]
        [InlineData(0, 2, 2, "000000000")]
        [InlineData(8, 2, 2, "111101111")]
        [InlineData(2, 1, 1, "011101111")]
        public void InitializeWithSeed_Invokes_UpdateCellLiveNeighborCount(int expectedNeightborCount, int cellRow, int cellColumn, string input_seed)
        {
            // Arrange
            var cellArena = new CellArena();
            int input_length = 3;
            int input_height = 3;
            cellArena.CreateEmptyArena(input_length, input_height);
            var preActNeightbourCount = cellArena.ActiveCells.FirstOrDefault(c => c.Row.Equals(cellRow) && c.Col.Equals(cellColumn))?.LiveNeighborCellCount;

            // Act
            cellArena.InitializeWithSeed(input_seed);

            // Assert
            Assert.Equal(0, preActNeightbourCount);
            Assert.True(cellArena.ActiveCells.FirstOrDefault(c => c.Row.Equals(cellRow) && c.Col.Equals(cellColumn))?.LiveNeighborCellCount.Equals(expectedNeightborCount));
        }

        [Theory]
        [InlineData("000000000", "000010000", 1)]
        [InlineData("000000000", "000010000", 5)]
        [InlineData("110110000", "110110000", 1)]
        [InlineData("110110000", "110110000", 3)]
        [InlineData("010010010", "000111000", 1)]
        [InlineData("000111000", "000111000", 2)]
        [InlineData("000111000", "000111000", 10)]
        public void Update_UpdatesCellField_By_Generation_Count(string expected_cellField_stringFormat, string input_seed, int expectedGenerationCount)
        {
            // Arrange
            var cellArena = new CellArena();
            int input_length = 3;
            int input_height = 3;
            cellArena.CreateEmptyArena(input_length, input_height);
            cellArena.InitializeWithSeed(input_seed);

            // Act
            for (int i = 0; i < expectedGenerationCount; i++)
            {
                cellArena.Update();
            }

            // Assert
            Assert.Equal(expectedGenerationCount, cellArena.ArenaData.IterationCount);

            // Asserting each cell position and state.
            var enumerator = cellArena.ActiveCells.GetEnumerator();
            for (int i = 0; i < expected_cellField_stringFormat.Length; i++)
            {
                enumerator.MoveNext();
                var expectedCellState = expected_cellField_stringFormat[i] switch
                {
                    '0' => CellState.Dead,
                    '1' => CellState.Live,
                    _ => throw new Exception("Invalid character found in expected_cellField_stringFormat.")
                };
                Assert.Equal(expectedCellState, enumerator.Current.State);
            }
        }
    }
}
