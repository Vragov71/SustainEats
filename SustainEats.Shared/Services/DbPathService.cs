using System;
using System.IO;

namespace SustainEats.Shared.Services
{
    public class DbPathService
    {
        public string GetDbPath()
        {
            var dbName = "app.db";

            // Проверка 1: Ако сме на MAUI (Телефон/Desktop App)
            // Използваме специалната папка за данни на устройството
            if (IsMobileOrDesktopApp())
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(path, dbName);
            }

            // Проверка 2: Ако сме Уеб (SustainEats.Web)
            // Използваме папката, където работи сайта
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbName);
        }

        private bool IsMobileOrDesktopApp()
        {
            // Лесна проверка дали имаме достъп до Android/iOS специфични API-та
            // Ако този код "гръмне", значи сме в Web, затова го пазим в try-catch
            try
            {
                // Този тип съществува само в MAUI проектите при Runtime
                return Type.GetType("Microsoft.Maui.Controls.Application, Microsoft.Maui.Controls") != null;
            }
            catch
            {
                return false;
            }
        }
    }
}