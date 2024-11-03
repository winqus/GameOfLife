using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLifeApp.Models;

namespace GameOfLifeApp.Interfaces
{
    public interface IVisualize
    {
        /// <summary>
        /// Method that visualizes the game field and some additional info.
        /// </summary>
        public void Visualize(IEnumerable<Cell> cells, int fieldLength, int fieldHeight, string? additionalInfo);

        /// <summary>
        /// Provides a menu for selecting game arena length and height.
        /// </summary>
        /// <param name="fieldLength"></param>
        /// <param name="fieldHeight"></param>
        public void VisualizeMenu(out int fieldLength, out int fieldHeight);
    }
}
