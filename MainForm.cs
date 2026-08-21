using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyNgayLe.Models;
using QuanLyNgayLe.Services;

namespace QuanLyNgayLe
{
    public partial class MainForm : Form
    {
        private HolidayService _holidayService;
        private NotificationTimer _notificationTimer;
        private NotificationService _notificationService;

        public MainForm()
        {
            InitializeComponent();
            _holidayService = new HolidayService();
            _notificationTimer = new NotificationTimer();
            _notificationService = new NotificationService();

            this.Text = "Quản Lý Ngày Lễ - Holiday Reminder";
            this.Icon = SystemIcons.Application;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeUI();
                LoadHolidaysData();
                _notificationTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeUI()
        {
            // Create main layout
            var mainPanel = new Panel { Dock = DockStyle.Fill };

            // Create toolbar
            var toolbarPanel = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.LightGray };
            
            // Buttons
            var btnAdd = new Button { Text = "➕ Thêm", Location = new Point(10, 10), Size = new Size(80, 40) };
            btnAdd.Click += (s, e) => ShowAddHolidayDialog();
            toolbarPanel.Controls.Add(btnAdd);

            var btnEdit = new Button { Text = "✏️ Sửa", Location = new Point(100, 10), Size = new Size(80, 40) };
            btnEdit.Click += (s, e) => ShowEditHolidayDialog();
            toolbarPanel.Controls.Add(btnEdit);

            var btnDelete = new Button { Text = "🗑️ Xóa", Location = new Point(190, 10), Size = new Size(80, 40) };
            btnDelete.Click += (s, e) => DeleteHoliday();
            toolbarPanel.Controls.Add(btnDelete);

            var btnRefresh = new Button { Text = "🔄 Làm mới", Location = new Point(280, 10), Size = new Size(80, 40) };
            btnRefresh.Click += (s, e) => LoadHolidaysData();
            toolbarPanel.Controls.Add(btnRefresh);

            var chkNotification = new CheckBox 
            { 
                Text = "🔔 Bật thông báo", 
                Location = new Point(380, 15), 
                Size = new Size(150, 30),
                Checked = true
            };
            chkNotification.CheckedChanged += (s, e) => 
            {
                if (chkNotification.Checked)
                    _notificationTimer.Start();
                else
                    _notificationTimer.Stop();
            };
            toolbarPanel.Controls.Add(chkNotification);

            mainPanel.Controls.Add(toolbarPanel);

            // Create DataGridView
            var dgvHolidays = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            // Add columns
            dgvHolidays.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "ID", Visible = false });
            dgvHolidays.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Tên Ngày Lễ" });
            dgvHolidays.Columns.Add(new DataGridViewTextBoxColumn { Name = "Date", HeaderText = "Ngày" });
            dgvHolidays.Columns.Add(new DataGridViewTextBoxColumn { Name = "Description", HeaderText = "Mô Tả" });
            dgvHolidays.Columns.Add(new DataGridViewCheckBoxColumn { Name = "Enabled", HeaderText = "Bật" });
            dgvHolidays.Columns.Add(new DataGridViewTextBoxColumn { Name = "NotifyDays", HeaderText = "Nhắc Trước (ngày)" });

            mainPanel.Controls.Add(dgvHolidays);

            this.Controls.Add(mainPanel);
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Store reference for later use
            this.Tag = dgvHolidays;
        }

        private void LoadHolidaysData()
        {
            try
            {
                var dgvHolidays = this.Tag as DataGridView;
                if (dgvHolidays == null) return;

                dgvHolidays.Rows.Clear();
                var holidays = _holidayService.GetAllHolidays();

                foreach (var holiday in holidays)
                {
                    var daysUntil = (holiday.Date.Date - DateTime.Now.Date).Days;
                    var statusIcon = daysUntil == 0 ? "🎉" : daysUntil < 0 ? "✓" : "⏰";

                    dgvHolidays.Rows.Add(
                        holiday.Id,
                        $"{statusIcon} {holiday.Name}",
                        holiday.Date.ToString("dd/MM/yyyy"),
                        holiday.Description,
                        holiday.Enabled,
                        holiday.NotificationDaysBefore
                    );
                }

                this.Text = $"Quản Lý Ngày Lễ - {holidays.Count} ngày lễ";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowAddHolidayDialog()
        {
            using (var form = new HolidayForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _holidayService.AddHoliday(form.Holiday);
                        LoadHolidaysData();
                        MessageBox.Show("Thêm ngày lễ thành công!", "Thông báo", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi thêm ngày lễ: {ex.Message}", "Lỗi", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ShowEditHolidayDialog()
        {
            var dgvHolidays = this.Tag as DataGridView;
            if (dgvHolidays == null || dgvHolidays.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một ngày lễ để chỉnh sửa!", "Cảnh báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int id = (int)dgvHolidays.SelectedRows[0].Cells["Id"].Value;
                var holiday = _holidayService.GetHolidayById(id);

                using (var form = new HolidayForm(holiday))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        _holidayService.UpdateHoliday(form.Holiday);
                        LoadHolidaysData();
                        MessageBox.Show("Cập nhật ngày lễ thành công!", "Thông báo", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi chỉnh sửa ngày lễ: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteHoliday()
        {
            var dgvHolidays = this.Tag as DataGridView;
            if (dgvHolidays == null || dgvHolidays.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một ngày lễ để xóa!", "Cảnh báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa ngày lễ này?", "Xác nhận", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int id = (int)dgvHolidays.SelectedRows[0].Cells["Id"].Value;
                    _holidayService.DeleteHoliday(id);
                    LoadHolidaysData();
                    MessageBox.Show("Xóa ngày lễ thành công!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi xóa ngày lễ: {ex.Message}", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _notificationTimer.Stop();
        }
    }
}
