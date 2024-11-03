using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using GameOfLifeApp.Enums;

namespace GameOfLifeApp.Models
{
    public class Cell
    {
        public int Col { get; set; }

        public int Row { get; set; }

        public CellState State { get; set; }

        public int LiveNeighborCellCount { get; set; }

        /// <summary>
        /// Cell constructor to initialize cell to provided coordinates and state.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <param name="state"></param>
        public Cell(int row, int column, CellState state)
        {
            Row = row;
            Col = column;
            State = state;
            LiveNeighborCellCount = 0;
        }

        [JsonConstructor]
        public Cell()
        {
            // No logic
        }
    }
}
