using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GameOfLifeApp.Interfaces;
using GameOfLifeApp.Models;

namespace GameOfLifeApp.Services
{
    public class GameToJsonSaver : ISaver
    {
        private readonly IGameConsole _gameConsole;
        private readonly IGameFileManager _gameFileManager;

        public GameToJsonSaver(IGameConsole gameConsole, IGameFileManager gameFileManager)
        {
            _gameConsole = gameConsole;
            _gameFileManager = gameFileManager;
        }

        /// <summary>
        /// Commences dialog (if there any local saves) for user to load a save file with various options.
        /// </summary>
        /// <returns>A CellArenaModel that was load from game save data if user choose to load a save, otherwise - null.</returns>
        public CellArenaModel? AskUserToLoadSave()
        {
            var filePaths = _gameFileManager.GetFiles(_gameFileManager.GetGameDirectory(), "*.save.json").ToList();

            if(filePaths.Any() == false)
            {
                return null;
            }

            _gameConsole.Clear();
            if (QueryUserYN("Do you want to load a game save? (Y/N): "))
            {
                while (true)
                {
                    for (int i = 0; i < filePaths.Count; i++)
                    {
                        _gameConsole.WriteLine($"({i + 1}): {filePaths[i].Replace(_gameFileManager.GetGameDirectory(), "")}");
                    }
                    _gameConsole.Write("Chosen option number: ");

                    if (!int.TryParse(_gameConsole.ReadLine(), out int choice) || choice <= 0 || choice > filePaths.Count)
                    {
                        _gameConsole.WriteLine("Please choose an option number from the list! Press any key to continue.");
                        _gameConsole.ReadKey();

                        continue;
                    }
                    choice--;

                    return ReadFromFile(filePaths[choice]);
                }
            }

            return null;
        }

        /// <summary>
        /// Commences dialog for user to save game progress to a local file with naming options.
        /// </summary>
        /// <returns>True - if user saved, else - False.</returns>
        public bool AskUserToSave(CellArenaModel cellArenaModel)
        {
            if (QueryUserYN("Do you want to save the game progress? (Y/N): "))
            {
                _gameConsole.Write("Enter a file name: ");
                string fileName = _gameConsole.ReadLine() + ".save.json";
                return SaveToFile(cellArenaModel, fileName);
            }

            return false;
        }

        /// <summary>
        /// Asks user a provided yes/no question.
        /// </summary>
        /// <returns>True - if user accepts, else - False.</returns>
        public bool QueryUserYN(string yesNoQuery)
        {
            _gameConsole.Write(yesNoQuery);
            if (_gameConsole.ReadLine()?.ToUpper() == "Y")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Saves the cell arena data to a specified file.
        /// </summary>
        /// <returns>True - if saved successfully, else - False.</returns>
        public bool SaveToFile(CellArenaModel cellArenaModel, string fileName)
        {
            _gameConsole.WriteLine("Saving...");

            var saveObject = ConvertCellArenaModelToGameSaveDataModel(cellArenaModel);

            try
            {
                var jsonString = JsonSerializer.Serialize(saveObject);
                _gameFileManager.SaveToFile(fileName, jsonString);
            }
            catch (Exception e)
            {
                _gameConsole.WriteLine("Saving error: " + e.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Reads cell arena data from a specified file.
        /// </summary>
        /// <returns>CellArenaModel object if loaded successfully, else - null.</returns>
        public CellArenaModel? ReadFromFile(string filePath)
        {
            if(_gameFileManager.FileExists(filePath))
            {
                try
                {
                    string jsonDataString = _gameFileManager.ReadFromFile(filePath);
                    var saveData = JsonSerializer.Deserialize<GameSaveDataModel>(jsonDataString);
                    if (saveData != null && saveData.ActiveCells != null)
                    {
                        return ConvertGameSaveDataModelToCellArenaModel(saveData);
                    }
                }
                catch (Exception e)
                {
                    _gameConsole.WriteLine("Save loading error. Press any key to continue.");
                    _gameConsole.ReadKey();
                    return null;
                }
            }

            return null;
        }

        /// <summary>
        /// Helper method for converting CellArenaModel to GameSaveDataModel.
        /// </summary>
        /// <param name="cellArenaModel"></param>
        /// <returns></returns>
        private static GameSaveDataModel ConvertCellArenaModelToGameSaveDataModel(CellArenaModel cellArenaModel)
        {
            IEnumerable<Cell> activeCells = new List<Cell>();

            for (int row = 1; row < cellArenaModel.FieldHeight + 1; row++)
            {
                for (int col = 1; col < cellArenaModel.FieldLength + 1; col++)
                {
                    activeCells = activeCells.Append(cellArenaModel.CellField[row, col]);
                }
            }

            return new()
            {
                Seed = cellArenaModel.Seed,
                ActiveCells = activeCells,
                FieldLength = cellArenaModel.FieldLength,
                FieldHeight = cellArenaModel.FieldHeight,
                IterationCount = cellArenaModel.IterationCount
            };
        }

        /// <summary>
        /// Helper method for converting GameSaveDataModel to CellArenaModel.
        /// </summary>
        /// <param name="gameSaveDataModel"></param>
        /// <returns></returns>
        private static CellArenaModel ConvertGameSaveDataModelToCellArenaModel(GameSaveDataModel gameSaveDataModel)
        {
            Cell[,] cells = new Cell[gameSaveDataModel.FieldHeight + 2, gameSaveDataModel.FieldLength + 2];

            var enumerator = gameSaveDataModel.ActiveCells.GetEnumerator();
            for (int row = 0; row < gameSaveDataModel.FieldHeight + 2; row++)
            {
                for (int col = 0; col < gameSaveDataModel.FieldLength + 2; col++)
                {
                    if(row == 0 || row == gameSaveDataModel.FieldHeight + 1 || col == 0 || col == gameSaveDataModel.FieldLength + 1)
                    {
                        cells[row, col] = new Cell(row, col, Enums.CellState.Dead);
                    }
                    else
                    {
                        enumerator.MoveNext();
                        cells[row, col] = enumerator.Current;
                    }
                }
            }

            return new()
            {
                FieldHeight = gameSaveDataModel.FieldHeight,
                FieldLength = gameSaveDataModel.FieldLength,
                CellField = cells,
                Seed = gameSaveDataModel.Seed,
                IterationCount = gameSaveDataModel.IterationCount
            };
        }
    }
}
