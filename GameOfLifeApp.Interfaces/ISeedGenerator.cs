using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeApp.Interfaces
{
    public interface ISeedGenerator
    {
        public string GenerateNewSeed(int seedLength);
    }
}
