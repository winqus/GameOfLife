using GameOfLifeApp.Enums;
using GameOfLifeApp.Interfaces;
using GameOfLifeApp.Models;
using GameOfLifeApp.Services;
using Moq;

namespace GameOfLifeApp.Tests
{
    public class GameTests
    {
        [Fact]
        public void StartGame_Invokes_DependancyMethods_When_cellArenaModel_is_not_null()
        {
            // Arrange
            var mockSaver = new Mock<ISaver>();
            int expected_fieldLength = 3, expected_fieldHeight = 4;
            var cellArenaModel = new CellArenaModel()
            {
                CellField = new Cell[3, 3],
                FieldLength = expected_fieldLength,
                FieldHeight = expected_fieldHeight,
                IterationCount = 0,
                Seed = "000000000000"
            };
            mockSaver.Setup(x => x.AskUserToLoadSave()).Returns(cellArenaModel) ;
            var mockVisualizer = new Mock<IVisualize>();
            mockVisualizer.Setup(x => x.Visualize(It.IsAny<IEnumerable<Cell>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()));
            var mockCellArena = new Mock<ICellArena>();
            mockCellArena.Setup(x => x.ActiveCells).Returns(new List<Cell>());
            mockCellArena.Setup(x => x.ArenaData).Returns(cellArenaModel);
            var mockGameConsole = new Mock<IGameConsole>();
            
            var sut = new Game(mockSaver.Object, mockVisualizer.Object, mockCellArena.Object, mockGameConsole.Object);

            // Act
            sut.StartGame();

            // Assert
            mockSaver.Verify(x => x.AskUserToLoadSave(), Times.Once());
            mockCellArena.Verify(x => x.AssignCellArenaModel(cellArenaModel), Times.Once());
            mockVisualizer.Verify(x => x.Visualize(It.IsAny<IEnumerable<Cell>>(), expected_fieldLength, expected_fieldHeight,
                    $"Generation: (0)" + Environment.NewLine +
                    $"Live cell count: 0" + Environment.NewLine +
                    "Press Esc to stop the game."
                ), Times.Once());
            mockVisualizer.Verify(x => x.VisualizeMenu(out expected_fieldLength, out expected_fieldHeight), Times.Never());
        }

        [Fact]
        public void StartGame_Invokes_DependancyMethods_When_cellArenaModel_is_null()
        {
            // Arrange
            var mockSaver = new Mock<ISaver>();
            mockSaver.Setup(x => x.AskUserToLoadSave()).Returns(It.IsAny<CellArenaModel>());
            var mockVisualizer = new Mock<IVisualize>();
            int expected_fieldLength = 3, expected_fieldHeight = 4;
            mockVisualizer.Setup(x => x.VisualizeMenu(out expected_fieldLength, out expected_fieldHeight));
            var mockCellArena = new Mock<ICellArena>();
            var mockGameConsole = new Mock<IGameConsole>();

            var sut = new Game(mockSaver.Object, mockVisualizer.Object, mockCellArena.Object, mockGameConsole.Object);

            // Act
            sut.StartGame();

            // Assert
            mockSaver.Verify(x => x.AskUserToLoadSave(), Times.Once());
            mockCellArena.Verify(x => x.AssignCellArenaModel(It.IsAny<CellArenaModel>()), Times.Never());
            mockVisualizer.Verify(x => x.VisualizeMenu(out expected_fieldLength, out expected_fieldHeight), Times.Once());
            mockCellArena.Verify(x => x.CreateEmptyArena(expected_fieldLength, expected_fieldHeight), Times.Once());
            mockCellArena.Verify(x => x.InitializeWithSeed(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void RunGame_Invokes_DependancyMethods_Once()
        {
            // Arrange
            int iterationCount = 0, fieldLength = 3, fieldHeight = 4;
            var mockSaver = new Mock<ISaver>();
            mockSaver.Setup(x => x.AskUserToSave(It.IsAny<CellArenaModel>())).Returns(It.IsAny<bool>());
            var mockVisualizer = new Mock<IVisualize>();
            mockVisualizer.Setup(x => x.Visualize(It.IsAny<IEnumerable<Cell>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()));
            var mockCellArena = new Mock<ICellArena>();
            mockCellArena.Setup(x => x.ActiveCells).Returns(new List<Cell>());
            CellArenaModel cellArenaModel = new()
            {
                CellField = new Cell[fieldHeight, fieldLength],
                FieldLength = fieldLength,
                FieldHeight = fieldHeight,
                IterationCount = fieldLength,
                Seed = "000000000000"
            };
            mockCellArena.Setup(x => x.ArenaData).Returns(cellArenaModel);
            var mockGameConsole = new Mock<IGameConsole>();
            mockGameConsole.Setup(x => x.ReadKey(true)).Returns(new ConsoleKeyInfo((char)27, ConsoleKey.Escape, false, false, false));
            mockGameConsole.SetupSequence(x => x.KeyAvailable)
                .Returns(false)
                .Returns(true);

            var sut = new Game(mockSaver.Object, mockVisualizer.Object, mockCellArena.Object, mockGameConsole.Object);

            // Act
            sut.RunGame();

            // Assert
            mockCellArena.Verify(x => x.Update(), Times.Once());
            mockVisualizer.Verify(x => x.Visualize(It.IsAny<IEnumerable<Cell>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once());
            mockSaver.Verify(x => x.AskUserToSave(It.IsAny<CellArenaModel>()), Times.Once());
        }

        [Fact]
        public void RunGame_Invokes_DependancyMethods_Twice_Invokes_AskUserToSave_Once()
        {
            // Arrange
            int iterationCount = 0, fieldLength = 3, fieldHeight = 4;
            var mockSaver = new Mock<ISaver>();
            mockSaver.Setup(x => x.AskUserToSave(It.IsAny<CellArenaModel>())).Returns(It.IsAny<bool>());
            var mockVisualizer = new Mock<IVisualize>();
            mockVisualizer.Setup(x => x.Visualize(It.IsAny<IEnumerable<Cell>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()));
            var mockCellArena = new Mock<ICellArena>();
            mockCellArena.Setup(x => x.ActiveCells).Returns(new List<Cell>());
            CellArenaModel cellArenaModel = new()
            {
                CellField = new Cell[fieldHeight, fieldLength],
                FieldLength = fieldLength,
                FieldHeight = fieldHeight,
                IterationCount = fieldLength,
                Seed = "000000000000"
            };
            mockCellArena.Setup(x => x.ArenaData).Returns(cellArenaModel);
            var mockGameConsole = new Mock<IGameConsole>();
            mockGameConsole.Setup(x => x.ReadKey(true)).Returns(new ConsoleKeyInfo((char)27, ConsoleKey.Escape, false, false, false));
            mockGameConsole.SetupSequence(x => x.KeyAvailable)
                .Returns(false)
                .Returns(false)
                .Returns(true);

            var sut = new Game(mockSaver.Object, mockVisualizer.Object, mockCellArena.Object, mockGameConsole.Object);

            // Act
            sut.RunGame();

            // Assert
            mockCellArena.Verify(x => x.Update(), Times.Exactly(2));
            mockVisualizer.Verify(x => x.Visualize(It.IsAny<IEnumerable<Cell>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Exactly(2));
            mockSaver.Verify(x => x.AskUserToSave(It.IsAny<CellArenaModel>()), Times.Once());
        }
    }
}