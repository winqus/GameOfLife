using GameOfLifeApp.Enums;
using GameOfLifeApp.Interfaces;
using GameOfLifeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeApp.Services
{
    public class Game : IGame
    {
        public readonly ISaver _gameSaver;
        
        private readonly IVisualize _visualizer;

        private readonly ICellArena _cellArena;

        private readonly IGameConsole _gameConsole;

        public Game(ISaver gameSaver, IVisualize visualizer, ICellArena cellArena, IGameConsole gameConsole)
        {
            _gameSaver = gameSaver;
            _visualizer = visualizer;
            _cellArena = cellArena;
            _gameConsole = gameConsole;
        }

        public void StartGame()
        {
            CellArenaModel? cellArenaModel = _gameSaver.AskUserToLoadSave();

            if (cellArenaModel != null)
            {
                _cellArena.AssignCellArenaModel(cellArenaModel);

                _visualizer.Visualize(
                    _cellArena.ActiveCells, _cellArena.ArenaData.FieldLength, _cellArena.ArenaData.FieldHeight,
                    $"Generation: ({_cellArena.ArenaData.IterationCount})" + Environment.NewLine +
                    $"Live cell count: {_cellArena.ActiveCells.Count(c => c.State == CellState.Live)}" + Environment.NewLine +
                    "Press Esc to stop the game."
                );
                Thread.Sleep(1000);
            }
            else
            {
                _visualizer.VisualizeMenu(out int fieldLength, out int fieldHeight);
                _cellArena.CreateEmptyArena(fieldLength, fieldHeight);
                _cellArena.InitializeWithSeed(new SeedGenerator().GenerateNewSeed(fieldLength * fieldHeight));
            }
        }

        public void RunGame()
        {
            do
            {
                while (!_gameConsole.KeyAvailable)
                {
                    _cellArena.Update();

                    _visualizer.Visualize(
                        _cellArena.ActiveCells, _cellArena.ArenaData.FieldLength, _cellArena.ArenaData.FieldHeight,
                        $"Generation: ({_cellArena.ArenaData.IterationCount})" + Environment.NewLine +
                        $"Live cell count: {_cellArena.ActiveCells.Count(c => c.State == CellState.Live)}" + Environment.NewLine +
                        "Press Esc to stop the game."
                    );

                    Thread.Sleep(1000);
                }
            } while (_gameConsole.ReadKey(true).Key != ConsoleKey.Escape);

            _gameSaver.AskUserToSave(_cellArena.ArenaData);
        }
    }
}
