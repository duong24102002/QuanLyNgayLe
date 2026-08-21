using System;
using System.Windows.Forms;
using QuanLyNgayLe.Database;

namespace QuanLyNgayLe
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                // Initialize database
                DatabaseHelper.InitializeDatabase();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi động ứng dụng: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
