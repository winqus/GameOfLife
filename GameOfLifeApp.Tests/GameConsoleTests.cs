using GameOfLifeApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeApp.Tests
{
    public class GameConsoleTests
    {
        [Theory]
        [InlineData("My random string!")]
        [InlineData("My looooooooooooooooooooooooooooooooooooooooooooooooooooong string!!!!")]
        [InlineData("Quite longer string with different lines!   \n Second line!\t\n  THIRDDDD!")]
        public void WriteLine_Outputs_String(string expected_value)
        {
            // Arrange
            TextWriter textWriter = new StringWriter();
            var gameConsole = new GameConsole(textWriter);

            // Act
            gameConsole.WriteLine(expected_value);

            // Assert
            Assert.Equal(expected_value + Environment.NewLine, textWriter.ToString());
        }

        [Theory]
        [InlineData("My input string!")]
        [InlineData("My looooooooooooooooooooooooooooooooooooooooooooooooooooong string!!!!")]
        [InlineData("123123123123123123123213 213 21312 3123 12 3112312 312 312312 21223")]
        [InlineData("Quite longer string with different lines!   \n Second line!\t\n  THIRDDDD!")]
        public void Write_Outputs_String(string expected_value)
        {
            // Arrange
            TextWriter textWriter = new StringWriter();
            var gameConsole = new GameConsole(textWriter);

            // Act
            gameConsole.Write(expected_value);

            // Assert
            Assert.Equal(expected_value, textWriter.ToString());
        }
    }
}
