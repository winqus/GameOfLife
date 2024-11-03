using GameOfLifeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeApp.Interfaces
{
    public interface ICellArena
    {
        public CellArenaModel ArenaData { get; }

        public IEnumerable<Cell> ActiveCells { get; }

        void AssignCellArenaModel(CellArenaModel cellArenaModel);   

        void CreateEmptyArena(int length, int height);

        void InitializeWithSeed(string seed);

        void Update();
    }
}
