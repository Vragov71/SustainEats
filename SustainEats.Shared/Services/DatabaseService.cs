using Microsoft.Data.Sqlite;

namespace SustainEats.Shared.Services
{
    public class DatabaseService
    {
        private readonly string _dbPath;

        public DatabaseService(DbPathService pathService)
        {
            _dbPath = pathService.GetDbPath();
            InitializeDatabase();
        }

        public SqliteConnection GetConnection()
        {
            return new SqliteConnection($"Data Source={_dbPath}");
        }

        private void InitializeDatabase()
        {
            using var connection = GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL,
                    Email TEXT NOT NULL UNIQUE,
                    PasswordHash TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Categories (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    ParentId INTEGER,
                    FOREIGN KEY (ParentId) REFERENCES Categories (Id)
                );

                CREATE TABLE IF NOT EXISTS IngredientDefinitions (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Protein REAL NOT NULL,
                    Fat REAL NOT NULL,
                    Carbs REAL NOT NULL,
                    BaseUnit INTEGER NOT NULL
                );

                CREATE TABLE IF NOT EXISTS PantryItems (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER NOT NULL,
                    IngredientDefinitionId INTEGER NOT NULL,
                    Quantity REAL NOT NULL,
                    ExpiryDate TEXT,
                    FOREIGN KEY (UserId) REFERENCES Users (Id),
                    FOREIGN KEY (IngredientDefinitionId) REFERENCES IngredientDefinitions (Id)
                );

                CREATE TABLE IF NOT EXISTS Recipes (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Instructions TEXT,
                    PreparationTime INTEGER NOT NULL,
                    TotalCalories REAL NOT NULL,
                    TotalProtein REAL NOT NULL,
                    TotalFat REAL NOT NULL,
                    TotalCarbs REAL NOT NULL
                );

                CREATE TABLE IF NOT EXISTS RecipeIngredients (
                    RecipeId INTEGER NOT NULL,
                    IngredientDefinitionId INTEGER NOT NULL,
                    QuantityNeeded REAL NOT NULL,
                    PRIMARY KEY (RecipeId, IngredientDefinitionId),
                    FOREIGN KEY (RecipeId) REFERENCES Recipes (Id),
                    FOREIGN KEY (IngredientDefinitionId) REFERENCES IngredientDefinitions (Id)
                );
            ";
            command.ExecuteNonQuery();
        }
    }
}
