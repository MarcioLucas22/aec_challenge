using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace aec_challenge
{
    public class SearchResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Instructor { get; set; }
        public string Workload { get; set; }
    }

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
                Console.WriteLine($"ERROR: {ex.Message}");
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

                        cmd.ExecuteNonQuery();                        
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        public static List<SearchResult> GetAllTitles()
        {
            var results = new List<SearchResult>();

            try
            {                
                using (SQLiteConnection connection = new SQLiteConnection(databasePath))
                {
                    connection.Open();

                    string selectQuery = "SELECT Title FROM SearchResults";

                    using (SQLiteCommand cmd = new SQLiteCommand(selectQuery, connection))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var result = new SearchResult
                                {
                                    Title = reader["Title"].ToString(),
                                };

                                results.Add(result);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }

            return results;
        }
    }
}
