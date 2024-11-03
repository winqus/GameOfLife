using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeApp.Interfaces
{
    public interface IGameFileManager
    {
        IEnumerable<string> GetFiles(string dirPath, string searchPattern);

        string GetGameDirectory();

        void SaveToFile(string fileName, string value);
        
        string ReadFromFile(string filePath);

        bool FileExists(string path);
    }
}
