using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using QuanLyNgayLe.Database;
using QuanLyNgayLe.Models;

namespace QuanLyNgayLe.Services
{
    public class HolidayService
    {
        private readonly string _connectionString;

        public HolidayService()
        {
            _connectionString = DatabaseHelper.ConnectionString;
        }

        public List<Holiday> GetAllHolidays()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                const string sql = @"
                    SELECT Id, Name, Date, Description, Enabled, NotificationDaysBefore, CreatedAt, UpdatedAt
                    FROM Holidays
                    ORDER BY Date ASC
                ";

                var holidays = connection.Query<Holiday>(sql).ToList();
                return holidays;
            }
        }

        public List<Holiday> GetUpcomingHolidays(int daysAhead = 365)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var today = DateTime.Now;
                var futureDate = today.AddDays(daysAhead);

                const string sql = @"
                    SELECT Id, Name, Date, Description, Enabled, NotificationDaysBefore, CreatedAt, UpdatedAt
                    FROM Holidays
                    WHERE Enabled = 1 AND Date >= @StartDate AND Date <= @EndDate
                    ORDER BY Date ASC
                ";

                var holidays = connection.Query<Holiday>(sql, new
                {
                    StartDate = today.ToString("yyyy-MM-dd"),
                    EndDate = futureDate.ToString("yyyy-MM-dd")
                }).ToList();

                return holidays;
            }
        }

        public List<Holiday> GetHolidaysDueForNotification()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var today = DateTime.Now;

                const string sql = @"
                    SELECT Id, Name, Date, Description, Enabled, NotificationDaysBefore, CreatedAt, UpdatedAt
                    FROM Holidays
                    WHERE Enabled = 1 
                    AND julianday(Date) - julianday(@Today) > 0 
                    AND julianday(Date) - julianday(@Today) <= NotificationDaysBefore
                    ORDER BY Date ASC
                ";

                var holidays = connection.Query<Holiday>(sql, new
                {
                    Today = today.ToString("yyyy-MM-dd")
                }).ToList();

                return holidays;
            }
        }

        public Holiday GetHolidayById(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                const string sql = @"
                    SELECT Id, Name, Date, Description, Enabled, NotificationDaysBefore, CreatedAt, UpdatedAt
                    FROM Holidays
                    WHERE Id = @Id
                ";

                return connection.QueryFirstOrDefault<Holiday>(sql, new { Id = id });
            }
        }

        public int AddHoliday(Holiday holiday)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                const string sql = @"
                    INSERT INTO Holidays (Name, Date, Description, Enabled, NotificationDaysBefore, CreatedAt, UpdatedAt)
                    VALUES (@Name, @Date, @Description, @Enabled, @NotificationDaysBefore, @CreatedAt, @UpdatedAt);
                    SELECT last_insert_rowid();
                ";

                var id = connection.QuerySingle<int>(sql, new
                {
                    holiday.Name,
                    Date = holiday.Date.ToString("yyyy-MM-dd"),
                    holiday.Description,
                    holiday.Enabled,
                    holiday.NotificationDaysBefore,
                    CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                });

                return id;
            }
        }

        public bool UpdateHoliday(Holiday holiday)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                const string sql = @"
                    UPDATE Holidays
                    SET Name = @Name, Date = @Date, Description = @Description, 
                        Enabled = @Enabled, NotificationDaysBefore = @NotificationDaysBefore, 
                        UpdatedAt = @UpdatedAt
                    WHERE Id = @Id
                ";

                var result = connection.Execute(sql, new
                {
                    holiday.Id,
                    holiday.Name,
                    Date = holiday.Date.ToString("yyyy-MM-dd"),
                    holiday.Description,
                    holiday.Enabled,
                    holiday.NotificationDaysBefore,
                    UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                });

                return result > 0;
            }
        }

        public bool DeleteHoliday(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                const string sql = "DELETE FROM Holidays WHERE Id = @Id";

                var result = connection.Execute(sql, new { Id = id });
                return result > 0;
            }
        }

        public bool ToggleHolidayStatus(int id, bool enabled)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                const string sql = @"
                    UPDATE Holidays
                    SET Enabled = @Enabled, UpdatedAt = @UpdatedAt
                    WHERE Id = @Id
                ";

                var result = connection.Execute(sql, new
                {
                    Id = id,
                    Enabled = enabled ? 1 : 0,
                    UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                });

                return result > 0;
            }
        }
    }
}
