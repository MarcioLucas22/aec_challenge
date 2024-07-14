using System;
using System.Data.SQLite;
using System.IO;

namespace aec_challenge
{
    internal class DataHandler
    {
        private static readonly string databasePath;

        static DataHandler()
        {
            string rootDirectory = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)));

            string databaseFile = Path.Combine(rootDirectory, "db.sqlite3");
            databasePath = $"Data Source={databaseFile}; Version=3;";
        }

        public static void CreateTable()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(databasePath))
                {
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        string createTableQuery = @"
                        CREATE TABLE IF NOT EXISTS SearchResults (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Title TEXT NOT NULL,
                            Description TEXT NOT NULL,
                            Instructor TEXT,
                            Workload TEXT
                        )";

                        using (SQLiteCommand createTableCmd = new SQLiteCommand(createTableQuery, connection))
                        {
                            createTableCmd.ExecuteNonQuery();
                        }

                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar tabela: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }


        public static void InsertData(string title, string description, string instructor, string workload)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(databasePath))
                {
                    connection.Open();

                    string insertQuery = @"
                    INSERT INTO SearchResults (Title, Description, Instructor, Workload) 
                    VALUES (@title, @description, @instructor, @workload)";

                    using (SQLiteCommand cmd = new SQLiteCommand(insertQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@instructor", instructor);
                        cmd.Parameters.AddWithValue("@workload", workload);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        Console.WriteLine($"{rowsAffected} linhas inseridas na tabela.");
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar tabela: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
