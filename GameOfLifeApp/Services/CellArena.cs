using GameOfLifeApp.Enums;
using GameOfLifeApp.Interfaces;
using GameOfLifeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeApp.Services
{
    public class CellArena : ICellArena
    {
        private CellArenaModel _cellArenaModel;

        public CellArenaModel ArenaData
        {
            get => _cellArenaModel;
        }

        public IEnumerable<Cell> ActiveCells
        {
            get => GetActiveCells();
        }

        public CellArena()
        {
            // No logic
        }

        /// <summary>
        /// Assign a CellArenaModel to CellArena.
        /// </summary>
        /// <param name="cellArenaModel"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void AssignCellArenaModel(CellArenaModel cellArenaModel)
        {
            if (cellArenaModel.Seed.Length != cellArenaModel.FieldLength * cellArenaModel.FieldHeight)
            {
                throw new ArgumentOutOfRangeException(nameof(cellArenaModel.Seed), "Seed length doesn't match arena properties.");
            }

            _cellArenaModel = cellArenaModel;
        }

        /// <summary>
        /// Creates an empty arena with dead cells according to specified length and height,
        /// </summary>
        /// <param name="length"></param>
        /// <param name="height"></param>
        /// <exception cref="ArgumentException"></exception>
        public void CreateEmptyArena(int length, int height)
        {
            if (length < 3 || height < 3)
            {
                throw new ArgumentException("Length or height can't be less than 3.");
            }

            CellArenaModel cellArenaModel = new()
            {
                CellField = new Cell[height + 2, length + 2],
                FieldLength = length,
                FieldHeight = height,
                IterationCount = 0,
                Seed = new string(Enumerable.Repeat('0', length * height).ToArray())
            };

            for (int row = 0; row < cellArenaModel.FieldHeight + 2; row++)
            {
                for (int col = 0; col < cellArenaModel.FieldLength + 2; col++)
                {
                    cellArenaModel.CellField[row, col] = new Cell(row, col, state: CellState.Dead);
                }
            }

            this.AssignCellArenaModel(cellArenaModel);
        }

        /// <summary>
        /// Assigns all cell states based on provided game seed.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void InitializeWithSeed(string seed)
        {
            if (seed.Length.Equals(_cellArenaModel.Seed.Length) == false)
            {
                throw new ArgumentException("Seed length does not match arena field count.");
            }

            _cellArenaModel.Seed = seed;

            foreach (var cell in GetActiveCells())
            {
                _cellArenaModel.CellField[cell.Row, cell.Col].State = _cellArenaModel.Seed[cell.Col - 1 + (cell.Row - 1) * _cellArenaModel.FieldLength] switch
                {
                    '0' => CellState.Dead,
                    '1' => CellState.Live,
                    _ => throw new Exception("Values within seed must be 0 or 1.")
                };
            }

            UpdateCellLiveNeighborCount();
        }

        /// <summary>
        /// Updates the cell field by one generation.
        /// </summary>
        public void Update()
        {
            _cellArenaModel.IterationCount++;

            foreach (var cell in GetActiveCells())
            {
                if (cell.State == CellState.Live && cell.LiveNeighborCellCount >= 2 && cell.LiveNeighborCellCount <= 3)
                {
                    continue;
                }
                else if (cell.State == CellState.Dead && cell.LiveNeighborCellCount == 3)
                {
                    cell.State = CellState.Live;
                }
                else if (cell.State == CellState.Live)
                {
                    cell.State = CellState.Dead;
                }
            }

            UpdateCellLiveNeighborCount();
        }

        /// <summary>
        /// Updates all cell live neighbor count.
        /// </summary>
        private void UpdateCellLiveNeighborCount()
        {
            foreach (var cell in GetActiveCells())
            {
                cell.LiveNeighborCellCount = GetLiveNeighborCellCount(cell.Row, cell.Col);
            }
        }

        /// <summary>
        /// Counts how many neightbor cells are alive.
        /// </summary>
        /// <param name="centerCellRow"></param>
        /// <param name="centerCellCol"></param>
        /// <returns></returns>
        private int GetLiveNeighborCellCount(int centerCellRow, int centerCellCol)
        {
            int liveNeighborCellCount = 0;
            for (int offset_row = -1; offset_row < 2; offset_row++)
            {
                for (int offset_col = -1; offset_col < 2; offset_col++)
                {
                    if (offset_row == 0 && offset_col == 0)
                    {
                        continue;
                    }

                    if (_cellArenaModel.CellField[centerCellRow + offset_row, centerCellCol + offset_col].State == CellState.Live)
                    {
                        liveNeighborCellCount++;
                    }
                }
            }

            return liveNeighborCellCount;
        }

        /// <summary>
        /// Helper method for returning all game field cells as enumerable.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Cell> GetActiveCells()
        {
            for (int row = 1; row < _cellArenaModel.FieldHeight + 1; row++)
            {
                for (int col = 1; col < _cellArenaModel.FieldLength + 1; col++)
                {
                    yield return _cellArenaModel.CellField[row, col];
                }
            }

            yield break;
        }
    }
}
