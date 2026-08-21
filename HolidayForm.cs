using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyNgayLe.Models;

namespace QuanLyNgayLe
{
    public partial class HolidayForm : Form
    {
        public Holiday Holiday { get; private set; }
        private bool _isEditMode;

        public HolidayForm()
        {
            _isEditMode = false;
            Holiday = new Holiday();
            InitializeComponent();
        }

        public HolidayForm(Holiday holiday)
        {
            _isEditMode = true;
            Holiday = holiday ?? new Holiday();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = _isEditMode ? "Chỉnh Sửa Ngày Lễ" : "Thêm Ngày Lễ Mới";
            this.Size = new Size(400, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Create layout
            var mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 7,
                Padding = new Padding(10)
            };

            // Name
            var lblName = new Label { Text = "Tên Ngày Lễ:", AutoSize = true };
            var txtName = new TextBox { Dock = DockStyle.Fill, Text = Holiday.Name ?? "" };
            mainPanel.Controls.Add(lblName, 0, 0);
            mainPanel.Controls.Add(txtName, 1, 0);

            // Date
            var lblDate = new Label { Text = "Ngày:", AutoSize = true };
            var dtpDate = new DateTimePicker 
            { 
                Dock = DockStyle.Fill, 
                Value = Holiday.Date != default ? Holiday.Date : DateTime.Now,
                Format = DateTimePickerFormat.Short
            };
            mainPanel.Controls.Add(lblDate, 0, 1);
            mainPanel.Controls.Add(dtpDate, 1, 1);

            // Description
            var lblDesc = new Label { Text = "Mô Tả:", AutoSize = true };
            var txtDesc = new TextBox 
            { 
                Dock = DockStyle.Fill, 
                Text = Holiday.Description ?? "",
                Multiline = true,
                Height = 60
            };
            mainPanel.Controls.Add(lblDesc, 0, 2);
            mainPanel.Controls.Add(txtDesc, 1, 2);

            // Notification Days
            var lblNotifyDays = new Label { Text = "Nhắc Trước (ngày):", AutoSize = true };
            var numNotifyDays = new NumericUpDown 
            { 
                Dock = DockStyle.Fill,
                Value = Holiday.NotificationDaysBefore,
                Minimum = 0,
                Maximum = 30
            };
            mainPanel.Controls.Add(lblNotifyDays, 0, 3);
            mainPanel.Controls.Add(numNotifyDays, 1, 3);

            // Enabled
            var lblEnabled = new Label { Text = "Bật Thông Báo:", AutoSize = true };
            var chkEnabled = new CheckBox 
            { 
                Checked = Holiday.Enabled,
                Dock = DockStyle.Left
            };
            mainPanel.Controls.Add(lblEnabled, 0, 4);
            mainPanel.Controls.Add(chkEnabled, 1, 4);

            // Buttons
            var btnPanel = new Panel { Dock = DockStyle.Bottom, Height = 40 };
            var btnOK = new Button 
            { 
                Text = "Lưu", 
                Location = new Point(200, 5), 
                Size = new Size(80, 30),
                DialogResult = DialogResult.OK
            };
            btnOK.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên ngày lễ!", "Cảnh báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Holiday.Name = txtName.Text;
                Holiday.Date = dtpDate.Value;
                Holiday.Description = txtDesc.Text;
                Holiday.NotificationDaysBefore = (int)numNotifyDays.Value;
                Holiday.Enabled = chkEnabled.Checked;
                Holiday.UpdatedAt = DateTime.Now;

                this.DialogResult = DialogResult.OK;
                this.Close();
            };
            btnPanel.Controls.Add(btnOK);

            var btnCancel = new Button 
            { 
                Text = "Hủy", 
                Location = new Point(290, 5), 
                Size = new Size(80, 30),
                DialogResult = DialogResult.Cancel
            };
            btnPanel.Controls.Add(btnCancel);

            mainPanel.Controls.Add(btnPanel, 0, 6);
            mainPanel.SetColumnSpan(btnPanel, 2);

            // Set column widths
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));

            this.Controls.Add(mainPanel);
        }
    }
}
