using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeApp.Interfaces
{
    public interface IGameConsole
    {
        Encoding OutputEncoding { get; set; }

        bool KeyAvailable { get; }

        void Clear();

        void WriteLine(string? value);

        void Write(string? value);

        string? ReadLine();

        void ReadKey();

        ConsoleKeyInfo ReadKey(bool intercept);
    }
}
