using System;
using System.Data.SQLite;
using System.IO;

namespace QuanLyNgayLe.Database
{
    public class DatabaseHelper
    {
        private static readonly string DbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "QuanLyNgayLe",
            "holidays.db"
        );

        public static string ConnectionString => $"Data Source={DbPath};Version=3;";

        public static void InitializeDatabase()
        {
            try
            {
                // Create directory if it doesn't exist
                string directory = Path.GetDirectoryName(DbPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Create or check database
                if (!File.Exists(DbPath))
                {
                    CreateDatabase();
                }
                else
                {
                    // Ensure tables exist
                    EnsureTablesExist();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khởi tạo cơ sở dữ liệu: {ex.Message}", ex);
            }
        }

        private static void CreateDatabase()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string createTableSql = @"
                    CREATE TABLE Holidays (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Date TEXT NOT NULL,
                        Description TEXT,
                        Enabled INTEGER DEFAULT 1,
                        NotificationDaysBefore INTEGER DEFAULT 1,
                        CreatedAt TEXT NOT NULL,
                        UpdatedAt TEXT NOT NULL
                    );

                    CREATE TABLE NotificationLog (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        HolidayId INTEGER NOT NULL,
                        NotificationTime TEXT NOT NULL,
                        NotificationType TEXT,
                        FOREIGN KEY (HolidayId) REFERENCES Holidays(Id)
                    );

                    CREATE INDEX idx_holiday_date ON Holidays(Date);
                    CREATE INDEX idx_holiday_enabled ON Holidays(Enabled);
                ";

                using (var command = new SQLiteCommand(createTableSql, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Insert default holidays
                InsertDefaultHolidays(connection);
            }
        }

        private static void EnsureTablesExist()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string checkTablesSql = @"
                    SELECT name FROM sqlite_master WHERE type='table' AND name='Holidays'
                ";

                using (var command = new SQLiteCommand(checkTablesSql, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result == null)
                    {
                        CreateDatabase();
                    }
                }
            }
        }

        private static void InsertDefaultHolidays(SQLiteConnection connection)
        {
            int year = DateTime.Now.Year;
            var holidays = new[]
            {
                ("Tết Nguyên Đán", $"{year}-02-10", "Ngày Tết Nguyên Đán (Tết âm lịch)", 3),
                ("Giải phóng Miền Nam", $"{year}-04-30", "Ngày Giải phóng Miền Nam", 1),
                ("Quốc tế Lao động", $"{year}-05-01", "Ngày Quốc tế Lao động", 1),
                ("Quốc khánh", $"{year}-09-02", "Ngày Quốc khánh Việt Nam", 3),
                ("Giáng Sinh", $"{year}-12-25", "Ngày Giáng Sinh", 7),
            };

            foreach (var (name, date, description, notifyDays) in holidays)
            {
                string insertSql = @"
                    INSERT INTO Holidays (Name, Date, Description, Enabled, NotificationDaysBefore, CreatedAt, UpdatedAt)
                    VALUES (@Name, @Date, @Description, 1, @NotifyDays, @Now, @Now)
                ";

                using (var command = new SQLiteCommand(insertSql, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Date", date);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@NotifyDays", notifyDays);
                    command.Parameters.AddWithValue("@Now", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        // Holiday already exists, skip
                    }
                }
            }
        }
    }
}
