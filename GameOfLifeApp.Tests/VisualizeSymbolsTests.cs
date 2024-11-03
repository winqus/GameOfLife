using GameOfLifeApp.Interfaces;
using GameOfLifeApp.Models;
using GameOfLifeApp.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace GameOfLifeApp.Tests
{
    public class VisualizeSymbolsTests
    {
        [Theory]
        [InlineData(3, 3)]
        [InlineData(3, 4)]
        [InlineData(9, 333)]
        [InlineData(1000, 1010)]
        public void VisualizeMenu_Invokes_Functions_With_TextInputOutput(int expected_fieldLength, int expected_fieldHeight)
        {
            // Arrange
            var mock = new Mock<IGameConsole>();
            mock.Setup(x => x.WriteLine(It.IsAny<string>()));
            mock.Setup(x => x.Write(It.IsAny<string>()));
            mock.SetupSequence(x => x.ReadLine())
                .Returns(expected_fieldLength.ToString())
                .Returns(expected_fieldHeight.ToString());

            var sut = new VisualizeSymbols(mock.Object);
            int output_fieldLength, output_fieldHeight;

            // Act
            sut.VisualizeMenu(out output_fieldLength, out output_fieldHeight);

            // Assert
            Assert.Equal(expected_fieldLength, output_fieldLength);
            Assert.Equal(expected_fieldHeight, output_fieldHeight);
            mock.Verify(x => x.Clear(), Times.Once);
            mock.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Once);
            mock.Verify(x => x.ReadKey(), Times.Never);
            mock.Verify(x => x.Write(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public void VisualizeMenu_Invokes_Repetetive_Function_Calls_With_TextInputOutput()
        {
            // Arrange
            var mock = new Mock<IGameConsole>();
            mock.Setup(x => x.WriteLine(It.IsAny<string>()));
            mock.Setup(x => x.Write(It.IsAny<string>()));
            mock.SetupSequence(x => x.ReadLine())
                .Returns("2")
                .Returns("-1")
                .Returns("a")
                .Returns("0")
                .Returns("3")
                .Returns("5");

            var sut = new VisualizeSymbols(mock.Object);

            // Act
            sut.VisualizeMenu(out int output_fieldLength, out int output_fieldHeight);

            // Assert
            mock.Verify(x => x.Clear(), Times.Exactly(3));
            mock.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(5));
            mock.Verify(x => x.ReadKey(), Times.Exactly(2));
            mock.Verify(x => x.Write(It.IsAny<string>()), Times.Exactly(6));
        }

        [Fact]
        public void Visualize_Invokes_Functions_With_TextOutput()
        {
            // Arrange
            var mock = new Mock<IGameConsole>();
            mock.Setup(x => x.WriteLine(It.IsAny<string>()));

            var sut = new VisualizeSymbols(mock.Object);
            IEnumerable<Cell> input_cells = new List<Cell>
            {
                new Cell(1, 1, Enums.CellState.Dead),
                new Cell(1, 2, Enums.CellState.Live),
                new Cell(1, 3, Enums.CellState.Live),
                new Cell(2, 1, Enums.CellState.Live),
                new Cell(2, 2, Enums.CellState.Dead),
                new Cell(2, 3, Enums.CellState.Live),
                new Cell(3, 1, Enums.CellState.Live),
                new Cell(3, 2, Enums.CellState.Live),
                new Cell(3, 3, Enums.CellState.Dead),
            };
            int input_length = 3, input_height = 3;

            // Act
            sut.Visualize(input_cells, input_length, input_height);

            // Assert
            mock.Verify(x => x.Clear(), Times.Exactly(1));
            mock.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(2));
            mock.Verify(x => x.WriteLine($"Field size: {input_length} x {input_height}"), Times.Exactly(1));
            mock.Verify(x => x.WriteLine($"011{Environment.NewLine}101{Environment.NewLine}110"), Times.Exactly(1));
        }

        [Theory]
        [InlineData("Some random text for output.")]
        public void Visualize_With_additionalInfo_Invokes_Functions_With_TextOutput(string input_additionalInfo)
        {
            // Arrange
            var mock = new Mock<IGameConsole>();
            mock.Setup(x => x.WriteLine(It.IsAny<string>()));

            var sut = new VisualizeSymbols(mock.Object);
            IEnumerable<Cell> input_cells = new List<Cell>
            {
                new Cell(1, 1, Enums.CellState.Dead),
                new Cell(1, 2, Enums.CellState.Live),
                new Cell(1, 3, Enums.CellState.Live),
                new Cell(2, 1, Enums.CellState.Live),
                new Cell(2, 2, Enums.CellState.Dead),
                new Cell(2, 3, Enums.CellState.Live),
                new Cell(3, 1, Enums.CellState.Live),
                new Cell(3, 2, Enums.CellState.Live),
                new Cell(3, 3, Enums.CellState.Dead),
            };
            int input_length = 3, input_height = 3;

            // Act
            sut.Visualize(input_cells, input_length, input_height, input_additionalInfo);

            // Assert
            mock.Verify(x => x.Clear(), Times.Exactly(1));
            mock.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(3));
            mock.Verify(x => x.WriteLine($"Field size: {input_length} x {input_height}"), Times.Exactly(1));
            mock.Verify(x => x.WriteLine($"011{Environment.NewLine}101{Environment.NewLine}110"), Times.Exactly(1));
            mock.Verify(x => x.WriteLine($"{input_additionalInfo}"), Times.Exactly(1));
        }
    }
}
