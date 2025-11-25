using System;
using System.IO;

namespace SustainEats.Shared.Services
{
    public class DbPathService
    {
        public string GetDbPath()
        {
#if ANDROID
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "app.db");
#elif IOS
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "app.db");
#elif WINDOWS
            return Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "app.db");
#else
            return "app.db";
#endif
        }
    }
}