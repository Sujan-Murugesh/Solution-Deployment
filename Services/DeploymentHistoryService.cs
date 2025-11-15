using Sujan_Solution_Deployer.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sujan_Solution_Deployer.Services
{
    public class DeploymentHistoryService
    {
        private readonly string connectionString;
        private readonly string dbPath;

        public DeploymentHistoryService()
        {
            var appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "SujanSolutionDeployer");

            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }

            dbPath = Path.Combine(appDataPath, "DeploymentHistory.db");
            connectionString = $"Data Source={dbPath};Version=3;";

            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
            }

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS DeploymentHistory (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        DeploymentDate TEXT NOT NULL,
                        SolutionUniqueName TEXT NOT NULL,
                        SolutionFriendlyName TEXT NOT NULL,
                        SourceVersion TEXT,
                        TargetVersion TEXT,
                        SourceEnvironment TEXT NOT NULL,
                        TargetEnvironment TEXT NOT NULL,
                        IsManaged INTEGER NOT NULL,
                        DeployedAsManaged INTEGER NOT NULL,
                        Status INTEGER NOT NULL,
                        DeployedBy TEXT,
                        ErrorMessage TEXT,
                        DurationSeconds INTEGER,
                        BackupCreated INTEGER,
                        BackupPath TEXT,
                        Notes TEXT
                    )";

                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                AddNotesColumnIfNotExists(connection);

                // Create index for faster queries
                var createIndexQuery = @"
                    CREATE INDEX IF NOT EXISTS idx_deployment_date 
                    ON DeploymentHistory(DeploymentDate DESC)";

                using (var command = new SQLiteCommand(createIndexQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private void AddNotesColumnIfNotExists(SQLiteConnection connection)
        {
            try
            {
                // Check if Notes column exists
                var checkColumnQuery = "PRAGMA table_info(DeploymentHistory)";
                bool notesColumnExists = false;

                using (var command = new SQLiteCommand(checkColumnQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["name"].ToString() == "Notes")
                            {
                                notesColumnExists = true;
                                break;
                            }
                        }
                    }
                }

                // Add column if it doesn't exist
                if (!notesColumnExists)
                {
                    var alterTableQuery = "ALTER TABLE DeploymentHistory ADD COLUMN Notes TEXT";
                    using (var command = new SQLiteCommand(alterTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error but don't fail initialization
                System.Diagnostics.Debug.WriteLine($"Error adding Notes column: {ex.Message}");
            }
        }

        public void AddDeployment(DeploymentHistory history)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var query = @"
                    INSERT INTO DeploymentHistory (
                        DeploymentDate, SolutionUniqueName, SolutionFriendlyName,
                        SourceVersion, TargetVersion, SourceEnvironment, TargetEnvironment,
                        IsManaged, DeployedAsManaged, Status, DeployedBy, ErrorMessage,
                        DurationSeconds, BackupCreated, BackupPath, Notes
                    ) VALUES (
                        @DeploymentDate, @SolutionUniqueName, @SolutionFriendlyName,
                        @SourceVersion, @TargetVersion, @SourceEnvironment, @TargetEnvironment,
                        @IsManaged, @DeployedAsManaged, @Status, @DeployedBy, @ErrorMessage,
                        @DurationSeconds, @BackupCreated, @BackupPath, @Notes
                    )";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DeploymentDate", history.DeploymentDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@SolutionUniqueName", history.SolutionUniqueName);
                    command.Parameters.AddWithValue("@SolutionFriendlyName", history.SolutionFriendlyName);
                    command.Parameters.AddWithValue("@SourceVersion", history.SourceVersion ?? string.Empty);
                    command.Parameters.AddWithValue("@TargetVersion", history.TargetVersion ?? string.Empty);
                    command.Parameters.AddWithValue("@SourceEnvironment", history.SourceEnvironment);
                    command.Parameters.AddWithValue("@TargetEnvironment", history.TargetEnvironment);
                    command.Parameters.AddWithValue("@IsManaged", history.IsManaged ? 1 : 0);
                    command.Parameters.AddWithValue("@DeployedAsManaged", history.DeployedAsManaged ? 1 : 0);
                    command.Parameters.AddWithValue("@Status", (int)history.Status);
                    command.Parameters.AddWithValue("@DeployedBy", history.DeployedBy ?? string.Empty);
                    command.Parameters.AddWithValue("@ErrorMessage", history.ErrorMessage ?? string.Empty);
                    command.Parameters.AddWithValue("@DurationSeconds", history.DurationSeconds);
                    command.Parameters.AddWithValue("@BackupCreated", history.BackupCreated ? 1 : 0);
                    command.Parameters.AddWithValue("@BackupPath", history.BackupPath ?? string.Empty);
                    command.Parameters.AddWithValue("@Notes", history.Notes ?? string.Empty);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddHistory(DeploymentHistory history)
        {
            AddDeployment(history);
        }

        public List<DeploymentHistory> GetAllHistory(int limit = 100)
        {
            var historyList = new List<DeploymentHistory>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var query = @"
                    SELECT * FROM DeploymentHistory 
                    ORDER BY DeploymentDate DESC 
                    LIMIT @Limit";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Limit", limit);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            historyList.Add(MapReaderToHistory(reader));
                        }
                    }
                }
            }

            return historyList;
        }

        public List<DeploymentHistory> GetHistoryByEnvironment(string targetEnvironment, int limit = 50)
        {
            var historyList = new List<DeploymentHistory>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var query = @"
                    SELECT * FROM DeploymentHistory 
                    WHERE TargetEnvironment = @TargetEnvironment
                    ORDER BY DeploymentDate DESC 
                    LIMIT @Limit";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TargetEnvironment", targetEnvironment);
                    command.Parameters.AddWithValue("@Limit", limit);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            historyList.Add(MapReaderToHistory(reader));
                        }
                    }
                }
            }

            return historyList;
        }

        public List<DeploymentHistory> GetHistoryBySolution(string solutionUniqueName, int limit = 50)
        {
            var historyList = new List<DeploymentHistory>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var query = @"
                    SELECT * FROM DeploymentHistory 
                    WHERE SolutionUniqueName = @SolutionUniqueName
                    ORDER BY DeploymentDate DESC 
                    LIMIT @Limit";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SolutionUniqueName", solutionUniqueName);
                    command.Parameters.AddWithValue("@Limit", limit);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            historyList.Add(MapReaderToHistory(reader));
                        }
                    }
                }
            }

            return historyList;
        }

        public void ClearHistory()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var query = "DELETE FROM DeploymentHistory";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private DeploymentHistory MapReaderToHistory(SQLiteDataReader reader)
        {
            return new DeploymentHistory
            {
                Id = Convert.ToInt32(reader["Id"]),
                DeploymentDate = DateTime.Parse(reader["DeploymentDate"].ToString()),
                SolutionUniqueName = reader["SolutionUniqueName"].ToString(),
                SolutionFriendlyName = reader["SolutionFriendlyName"].ToString(),
                SourceVersion = reader["SourceVersion"].ToString(),
                TargetVersion = reader["TargetVersion"].ToString(),
                SourceEnvironment = reader["SourceEnvironment"].ToString(),
                TargetEnvironment = reader["TargetEnvironment"].ToString(),
                IsManaged = Convert.ToInt32(reader["IsManaged"]) == 1,
                DeployedAsManaged = Convert.ToInt32(reader["DeployedAsManaged"]) == 1,
                Status = (DeploymentStatus)Convert.ToInt32(reader["Status"]),
                DeployedBy = reader["DeployedBy"].ToString(),
                ErrorMessage = reader["ErrorMessage"].ToString(),
                DurationSeconds = Convert.ToInt32(reader["DurationSeconds"]),
                BackupCreated = Convert.ToInt32(reader["BackupCreated"]) == 1,
                BackupPath = reader["BackupPath"].ToString(),
                Notes = reader["Notes"] != DBNull.Value ? reader["Notes"].ToString() : string.Empty
            };
        }
    }
}
