using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeApp.Models
{
    public class GameSaveDataModel
    {
        public int FieldLength { get; set; }

        public int FieldHeight { get; set; }

        public int IterationCount { get; set; }

        public string Seed { get; set; }

        public IEnumerable<Cell> ActiveCells { get; set; }
    }
}
