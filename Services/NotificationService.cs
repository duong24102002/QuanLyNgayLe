using System;
using System.Media;
using System.Windows.Forms;
using QuanLyNgayLe.Models;

namespace QuanLyNgayLe.Services
{
    public class NotificationService
    {
        private SystemSoundPlayer _soundPlayer;

        public NotificationService()
        {
            _soundPlayer = new SystemSoundPlayer();
        }

        public void ShowNotification(Holiday holiday)
        {
            try
            {
                // Play notification sound
                PlayNotificationSound();

                // Show popup notification
                ShowPopupNotification(holiday);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị thông báo: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PlayNotificationSound()
        {
            try
            {
                // Use system beep sound
                SystemSounds.Exclamation.Play();
            }
            catch
            {
                // Fallback: just beep
                Console.Beep();
            }
        }

        private void ShowPopupNotification(Holiday holiday)
        {
            var daysUntil = (holiday.Date.Date - DateTime.Now.Date).Days;
            
            string message;
            if (daysUntil == 0)
            {
                message = $"🎉 HÔM NAY LÀ NGÀY {holiday.Name.ToUpper()}!\n\n{holiday.Description}";
            }
            else if (daysUntil == 1)
            {
                message = $"⏰ NGÀY MAI LÀ NGÀY {holiday.Name.ToUpper()}!\n\nCòn 1 ngày nữa thôi!\n\n{holiday.Description}";
            }
            else
            {
                message = $"📅 SẮP TỚI: {holiday.Name}\n\nCòn {daysUntil} ngày\n\n{holiday.Description}";
            }

            MessageBox.Show(message, $"🔔 Nhắc nhở ngày lễ", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void SendEmailNotification(Holiday holiday, string recipientEmail)
        {
            // This would require email configuration
            // Placeholder for future implementation
            try
            {
                LogNotification(holiday, "Email");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gửi email: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LogNotification(Holiday holiday, string type)
        {
            // Log notification for tracking purposes
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Thông báo {type}: {holiday.Name}");
        }
    }
}
