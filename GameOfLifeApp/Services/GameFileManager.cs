using GameOfLifeApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeApp.Services
{
    public class GameFileManager : IGameFileManager
    {
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public IEnumerable<string> GetFiles(string dirPath, string searchPattern)
        {
            return Directory.EnumerateFiles(dirPath, searchPattern);
        }

        public string GetGameDirectory()
        {
            return Directory.GetCurrentDirectory();
        }

        public string ReadFromFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        public void SaveToFile(string fileName, string value)
        {
            File.WriteAllText(fileName, value);
        }
    }
}
