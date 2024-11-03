using GameOfLifeApp.Interfaces;
using GameOfLifeApp.Models;
using GameOfLifeApp.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeApp.Tests
{
    public class GameToJsonSaverTests
    {
        [Fact]
        public void AskUserToLoadSave_Returns_CellArenaModel_When_SaveFilePath_Selected()
        {
            // Arrange
            var expectedCellArenaModel_FieldLength = 3;
            var expectedCellArenaModel_FieldHeight = 3;
            var expectedCellArenaModel_Seed = "010100011";
            var expectedCellArenaModel_IterationCount = 1;
            var mockGameConsole = new Mock<IGameConsole>();
            mockGameConsole.SetupSequence(x => x.ReadLine())
                .Returns("y")
                .Returns("1");
            var mockGameFileManager = new Mock<IGameFileManager>();
            mockGameFileManager.Setup(x => x.GetGameDirectory()).Returns(@"c:\gamedir");
            mockGameFileManager.Setup(x => x.GetFiles(@"c:\gamedir", "*.save.json")).Returns(new List<string>() { @"c:\gamedir\mysave1.save.json" });
            mockGameFileManager.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
            var objectJsonString = "{\"FieldLength\":3,\"FieldHeight\":3,\"IterationCount\":1,\"Seed\":\"010100011\",\"ActiveCells\":[{\"Col\":1,\"Row\":1,\"State\":1,\"LiveNeighborCellCount\":1},{\"Col\":2,\"Row\":1,\"State\":1,\"LiveNeighborCellCount\":2},{\"Col\":3,\"Row\":1,\"State\":1,\"LiveNeighborCellCount\":1},{\"Col\":1,\"Row\":2,\"State\":0,\"LiveNeighborCellCount\":1},{\"Col\":2,\"Row\":2,\"State\":1,\"LiveNeighborCellCount\":3},{\"Col\":3,\"Row\":2,\"State\":0,\"LiveNeighborCellCount\":1},{\"Col\":1,\"Row\":3,\"State\":1,\"LiveNeighborCellCount\":2},{\"Col\":2,\"Row\":3,\"State\":0,\"LiveNeighborCellCount\":2},{\"Col\":3,\"Row\":3,\"State\":1,\"LiveNeighborCellCount\":2}]}";
            mockGameFileManager.Setup(x => x.ReadFromFile(It.IsAny<string>())).Returns(objectJsonString);

            var saver = new GameToJsonSaver(mockGameConsole.Object, mockGameFileManager.Object);

            // Act
            CellArenaModel? model = saver.AskUserToLoadSave();

            // Assert
            Assert.NotNull(model);
            Assert.Equal(model.FieldLength, expectedCellArenaModel_FieldLength);
            Assert.Equal(model.FieldHeight, expectedCellArenaModel_FieldHeight);
            Assert.Equal(model.Seed, expectedCellArenaModel_Seed);
            Assert.Equal(model.IterationCount, expectedCellArenaModel_IterationCount);

            mockGameConsole.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(1));
            mockGameConsole.Verify(x => x.Write(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public void AskUserToLoadSave_Invokes_WriteLine_6_times_When_Invalid_User_Input()
        {
            // Arrange
            var mockGameConsole = new Mock<IGameConsole>();
            mockGameConsole.SetupSequence(x => x.ReadLine())
                .Returns("y")
                .Returns("a")
                .Returns("2")
                .Returns("1");
            var mockGameFileManager = new Mock<IGameFileManager>();
            mockGameFileManager.Setup(x => x.GetGameDirectory()).Returns(@"c:\gamedir");
            mockGameFileManager.Setup(x => x.GetFiles(@"c:\gamedir", "*.save.json")).Returns(new List<string>() { @"c:\gamedir\mysave1.save.json" });
            mockGameFileManager.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
            var objectJsonString = @"";
            mockGameFileManager.Setup(x => x.ReadFromFile(It.IsAny<string>())).Returns(objectJsonString);

            var saver = new GameToJsonSaver(mockGameConsole.Object, mockGameFileManager.Object);

            // Act
            CellArenaModel? model = saver.AskUserToLoadSave();

            // Assert
            Assert.Null(model);

            mockGameConsole.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(6));
            mockGameConsole.Verify(x => x.Write(It.IsAny<string>()), Times.Exactly(4));
        }

        [Fact]
        public void AskUserToSave_Invokes_SaveToFile_When_User_Confirms()
        {
            // Arrange
            var mockGameConsole = new Mock<IGameConsole>();
            mockGameConsole.SetupSequence(x => x.ReadLine())
                .Returns("y")
                .Returns("testsave1");
            var mockGameFileManager = new Mock<IGameFileManager>();
            var input_cellArenaModel = new CellArenaModel()
            {
                CellField = new Cell[1,1],
                FieldLength = 0,
                FieldHeight = 0,
                IterationCount = 0,
                Seed = "000000000"
            };

            var saver = new GameToJsonSaver(mockGameConsole.Object, mockGameFileManager.Object);

            // Act
            saver.AskUserToSave(input_cellArenaModel);

            // Assert
            mockGameConsole.Verify(x => x.Write(It.IsAny<string>()), Times.Exactly(2));
            mockGameConsole.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(1));
            mockGameFileManager.Verify(x => x.SaveToFile("testsave1.save.json", It.IsAny<string>()), Times.Exactly(1));
        }

        [Fact]
        public void ReadFromFile_Throws_Exception_When_Incorrect_SaveFileJsonString()
        {
            // Arrange
            var mockGameConsole = new Mock<IGameConsole>();
            var mockGameFileManager = new Mock<IGameFileManager>();
            mockGameFileManager.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
            mockGameFileManager.Setup(x => x.ReadFromFile(It.IsAny<string>())).Returns("incorrect_json_string");

            var saver = new GameToJsonSaver(mockGameConsole.Object, mockGameFileManager.Object);

            // Act
            saver.ReadFromFile(It.IsAny<string>());

            // Assert
            mockGameConsole.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Once());
        }
    }
}
