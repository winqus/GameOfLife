using GameOfLifeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeApp.Interfaces
{
    public interface ISaver
    {
        /// <summary>
        /// Commences dialog for user to load a save file with various options.
        /// </summary>
        /// <returns>A CellArenaModel that was load from game save data if user choose to load a save, otherwise - null.</returns>
        public CellArenaModel? AskUserToLoadSave();

        /// <summary>
        /// Commences dialog for user to save game progress to a local file with naming options.
        /// </summary>
        /// <returns>True - if user saved, else - False.</returns>
        public bool AskUserToSave(CellArenaModel cellArenaModel);

        /// <summary>
        /// Asks user a provided yes/no question.
        /// </summary>
        /// <returns>True - if user accepts, else - False.</returns>
        public bool QueryUserYN(string yesNoQuery);

        /// <summary>
        /// Saves the cell arena to a specified file.
        /// </summary>
        /// <returns>True - if saved successfully, else - False.</returns>
        public bool SaveToFile(CellArenaModel cellArena, string fileName);

        /// <summary>
        /// Reads cell arena data from a specified file.
        /// </summary>
        /// <returns>CellArenaModel object if loaded successfully, else - null.</returns>
        public CellArenaModel? ReadFromFile(string filePath);
    }
}
