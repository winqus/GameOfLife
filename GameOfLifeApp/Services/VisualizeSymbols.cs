using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLifeApp.Enums;
using GameOfLifeApp.Interfaces;
using GameOfLifeApp.Models;

namespace GameOfLifeApp.Services
{
    public class VisualizeSymbols : IVisualize
    {
        private readonly IGameConsole _gameConsole;

        public string CharacterForLiveCell { get; set; } = "1";

        public string CharacterForDeadCell { get; set; } = "0";

        public bool ClearConsoleEachPrint { get; set; } = true;

        /// <summary>
        /// Default constructor. Sets the respresenting characters of live/dead cells.
        /// </summary>
        /// <param name="characterForLiveCell"></param>
        /// <param name="characterForDeadCell"></param>
        public VisualizeSymbols(IGameConsole gameConsole)
        {
            _gameConsole = gameConsole;
            //_gameConsole.OutputEncoding = System.Text.Encoding.UTF8;
        }

        /// <summary>
        /// Outputs a menu for selecting game arena length and height.
        /// </summary>
        /// <param name="fieldLength"></param>
        /// <param name="fieldHeight"></param>
        public void VisualizeMenu(out int fieldLength, out int fieldHeight)
        {
            do
            {
                _gameConsole.Clear();

                // Game title ascii art output.
                _gameConsole.WriteLine("\r\n   _____                         ____   __   _      _  __     \r\n  / ____|                       / __ \\ / _| | |    (_)/ _|    \r\n | |  __  __ _ _ __ ___   ___  | |  | | |_  | |     _| |_ ___ \r\n | | |_ |/ _` | '_ ` _ \\ / _ \\ | |  | |  _| | |    | |  _/ _ \\\r\n | |__| | (_| | | | | | |  __/ | |__| | |   | |____| | ||  __/\r\n  \\_____|\\__,_|_| |_| |_|\\___|  \\____/|_|   |______|_|_| \\___|\r\n                                                              \r\n                                                              \r\n");

                _gameConsole.Write("Enter the game arena length: ");
                Int32.TryParse(_gameConsole.ReadLine(), out fieldLength);

                _gameConsole.Write("Enter the game arena height: ");
                Int32.TryParse(_gameConsole.ReadLine(), out fieldHeight);

                if (fieldLength < 3 || fieldHeight < 3)
                {
                    _gameConsole.WriteLine("Game arena sizes must be an integer no less than 3! Press any key to try again.");
                    _gameConsole.ReadKey();
                }
            } while (fieldLength < 3 || fieldHeight < 3);
        }

        /// <summary>
        /// Outputs to console grid of provided cells. Characters are printed according to what was provided in constructor.
        /// </summary>
        /// <param name="cells"></param>
        /// <param name="fieldLength"></param>
        /// <param name="fieldHeight"></param>
        /// <param name="additionalInfo"></param>
        /// <exception cref="Exception"></exception>
        public void Visualize(IEnumerable<Cell> cells, int fieldLength, int fieldHeight, string? additionalInfo = null)
        {
            if(ClearConsoleEachPrint)
            {
                _gameConsole.Clear();
            }

            _gameConsole.WriteLine($"Field size: {fieldLength} x {fieldHeight}");
            if (additionalInfo != null)
            {
                _gameConsole.WriteLine(additionalInfo);
            }

            StringBuilder sb = new StringBuilder();

            int appendedChars = 0;
            foreach(var cell in cells)
            {
                sb.Append(cell.State switch
                {
                    CellState.Live => CharacterForLiveCell,
                    CellState.Dead => CharacterForDeadCell,
                    _ => throw new Exception("Invalid cell state.")
                });

                appendedChars++;

                if(appendedChars % fieldLength == 0 && appendedChars < fieldLength * fieldHeight)
                {
                    sb.AppendLine();
                }
            }

            _gameConsole.WriteLine(sb.ToString());
        }
    }
}
