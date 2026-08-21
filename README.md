README.md

# 🎉 Quản Lý Ngày Lễ - Holiday Reminder

Ứng dụng Windows Forms quản lý ngày lễ với tính năng nhắc nhở tự động.

## 🎯 Tính Năng

✅ **Quản lý ngày lễ**
- Xem danh sách tất cả ngày lễ
- Thêm ngày lễ mới
- Chỉnh sửa ngày lễ
- Xóa ngày lễ

✅ **Thông báo tự động**
- Thông báo popup
- Âm thanh cảnh báo
- Nhắc nhở trước N ngày
- Bật/tắt thông báo

✅ **Quản lý dữ liệu**
- Lưu trữ trong SQLite
- Import/Export JSON
- Danh sách ngày lễ Việt Nam mặc định

## 🚀 Cài Đặt & Chạy

### Yêu Cầu
- .NET 6.0 trở lên
- Visual Studio 2022 hoặc cao hơn

### Bước 1: Clone Repository
```bash
git clone https://github.com/duong24102002/QuanLyNgayLe.git
cd QuanLyNgayLe
```

### Bước 2: Cài Đặt Dependencies
```bash
dotnet restore
```

### Bước 3: Chạy Ứng Dụng
```bash
dotnet run
```

Hoặc trong Visual Studio:
- Nhấn **F5** để chạy
- Hoặc chọn **Debug → Start Debugging**

## 📋 Cách Sử Dụng

### 1. **Thêm Ngày Lễ**
- Nhấn nút "➕ Thêm"
- Điền thông tin ngày lễ
- Chọn ngày và số ngày nhắc trước
- Nhấn "Lưu"

### 2. **Chỉnh Sửa Ngày Lễ**
- Chọn ngày lễ trong danh sách
- Nhấn nút "✏️ Sửa"
- Cập nhật thông tin
- Nhấn "Lưu"

### 3. **Xóa Ngày Lễ**
- Chọn ngày lễ trong danh sách
- Nhấn nút "🗑️ Xóa"
- Xác nhận xóa

### 4. **Kích Hoạt Thông Báo**
- Đánh dấu "🔔 Bật thông báo"
- Ứng dụng sẽ tự động hiển thị thông báo khi gần ngày lễ

## 🗄️ Cơ Sở Dữ Liệu

Dữ liệu được lưu trữ trong SQLite tại:
```
%APPDATA%\QuanLyNgayLe\holidays.db
```

### Bảng Holidays
- `Id` - Mã ngày lễ
- `Name` - Tên ngày lễ
- `Date` - Ngày tháng năm
- `Description` - Mô tả
- `Enabled` - Bật/tắt thông báo
- `NotificationDaysBefore` - Nhắc trước N ngày
- `CreatedAt` - Ngày tạo
- `UpdatedAt` - Ngày cập nhật

## 📚 Ngày Lễ Mặc Định

Ứng dụng bao gồm các ngày lễ Việt Nam:
- 🧧 Tết Nguyên Đán
- 🇻🇳 Giải phóng Miền Nam (30/4)
- 💼 Quốc tế Lao động (1/5)
- 🎊 Quốc khánh (2/9)
- 🎄 Giáng Sinh (25/12)

## 🛠️ Cấu Trúc Dự Án

```
QuanLyNgayLe/
├── Models/
│   └── Holiday.cs                 # Model ngày lễ
├── Services/
│   ├── HolidayService.cs          # Quản lý ngày lễ
│   ├── NotificationService.cs     # Thông báo
│   ├── NotificationTimer.cs       # Timer kiểm tra thông báo
│   └── HolidayImportExportService.cs
├── Database/
│   └── DatabaseHelper.cs          # Khởi tạo cơ sở dữ liệu
├── MainForm.cs                    # Giao diện chính
├── HolidayForm.cs                 # Form thêm/sửa ngày lễ
├── Program.cs                     # Điểm khởi đầu
└── QuanLyNgayLe.csproj           # Tập tin dự án
```

## 📦 Dependencies

- `System.Data.SQLite` - Cơ sở dữ liệu SQLite
- `Dapper` - ORM nhẹ cho database
- `Newtonsoft.Json` - Import/Export JSON

## 🤝 Đóng Góp

Hãy tạo Pull Request hoặc Issues nếu bạn có đề xuất cải tiến!

## 📄 License

MIT License - Bạn tự do sử dụng và sửa đổi

## 👨‍💻 Tác Giả

**Dương** - [duong24102002](https://github.com/duong24102002)

---

**Mời bạn sử dụng và feedback! 🚀**
