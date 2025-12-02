using System;
using System.IO;

namespace SustainEats.Shared.Services
{
    public class DbPathService
    {
        private readonly string _dbFileName;

        public DbPathService(string dbFileName = "app.db")
        {
            _dbFileName = dbFileName;
        }

        public string GetDbPath()
        {
#if ANDROID
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _dbFileName);
#elif IOS
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _dbFileName);
#elif WINDOWS
            return Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, _dbFileName);
#else
            // For web or other platforms
            return _dbFileName;
#endif
        }
    }
}
