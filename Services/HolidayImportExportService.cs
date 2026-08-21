using System;
using System.Collections.Generic;
using System.IO;
using QuanLyNgayLe.Models;
using Newtonsoft.Json;

namespace QuanLyNgayLe.Services
{
    public class HolidayImportExportService
    {
        private readonly string _exportPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "holidays_backup.json"
        );

        public bool ExportToJson(List<Holiday> holidays)
        {
            try
            {
                var json = JsonConvert.SerializeObject(holidays, Formatting.Indented);
                File.WriteAllText(_exportPath, json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Holiday> ImportFromJson(string filePath)
        {
            try
            {
                var json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<Holiday>>(json) ?? new List<Holiday>();
            }
            catch
            {
                return new List<Holiday>();
            }
        }

        public string GetExportPath() => _exportPath;
    }
}
