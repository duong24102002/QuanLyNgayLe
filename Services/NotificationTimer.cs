using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using QuanLyNgayLe.Models;
using QuanLyNgayLe.Services;

namespace QuanLyNgayLe.Services
{
    public class NotificationTimer
    {
        private Timer _timer;
        private HolidayService _holidayService;
        private NotificationService _notificationService;
        private List<int> _notifiedHolidayIds;
        private bool _isRunning;

        public event EventHandler<Holiday> OnNotificationRequired;

        public NotificationTimer()
        {
            _holidayService = new HolidayService();
            _notificationService = new NotificationService();
            _notifiedHolidayIds = new List<int>();
            _isRunning = false;

            _timer = new Timer(60000); // Check every 1 minute
            _timer.Elapsed += CheckForNotifications;
            _timer.AutoReset = true;
        }

        public void Start()
        {
            if (!_isRunning)
            {
                _timer.Start();
                _isRunning = true;
                Console.WriteLine("[Thông báo] Dịch vụ nhắc nhở đã khởi động");
            }
        }

        public void Stop()
        {
            if (_isRunning)
            {
                _timer.Stop();
                _isRunning = false;
                Console.WriteLine("[Thông báo] Dịch vụ nhắc nhở đã dừng");
            }
        }

        private void CheckForNotifications(object sender, ElapsedEventArgs e)
        {
            try
            {
                var holidaysDueForNotification = _holidayService.GetHolidaysDueForNotification();

                foreach (var holiday in holidaysDueForNotification)
                {
                    // Only notify once per holiday per day
                    if (!_notifiedHolidayIds.Contains(holiday.Id))
                    {
                        _notificationService.ShowNotification(holiday);
                        _notifiedHolidayIds.Add(holiday.Id);
                        OnNotificationRequired?.Invoke(this, holiday);
                    }
                }

                // Reset notification list at midnight
                if (DateTime.Now.Hour == 0 && DateTime.Now.Minute == 0)
                {
                    _notifiedHolidayIds.Clear();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Lỗi] Lỗi kiểm tra thông báo: {ex.Message}");
            }
        }
    }
}
