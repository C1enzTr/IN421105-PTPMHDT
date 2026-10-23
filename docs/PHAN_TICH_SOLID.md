# Nhiệm vụ 1 — Phân tích vi phạm SOLID trong `ClubManager`

> ⚠️ **Việc cần làm trước khi nộp:** mở `ClubManager.cs` trong starter, điền **số dòng thật** vào cột "Vị trí".
> Các đoạn code dưới đây là mô tả theo mẫu của slide Ch02 (trang 10, 12, 18, 21); đối chiếu với file của bạn và sửa cho khớp.

## Tổng quan

`ClubManager` là **God class**: một class ôm kiểm tra dữ liệu, lưu CLB/thành viên, ghi log ra file,
gửi thông báo và in báo cáo. Hệ quả: khó đọc, khó kiểm thử (dính trực tiếp `File`, `Console`), khó mở rộng.

## Bảng vi phạm

| # | Nguyên lý | Vị trí (điền dòng) | Mô tả vi phạm | Hậu quả |
|---|-----------|--------------------|---------------|---------|
| 1 | **S** – Single Responsibility | `CreateClub()` — dòng ___ đến ___ | Một method vừa kiểm tra tên, vừa lưu vào danh sách, vừa `File.AppendAllText` ghi log, vừa `Console.WriteLine` báo tin | Class có ≥ 4 lý do để sửa; đổi cách ghi log hay cách báo tin đều phải mở `CreateClub` ra sửa, dễ làm hỏng phần khác |
| 2 | **O** – Open/Closed | Method gửi thông báo — dòng ___ | Chuỗi `if (email.Contains("@gmail.com")) … else if ("@yahoo.com") … else …` chọn kênh/cách gửi | Thêm SMS hoặc Slack bắt buộc sửa lại method cũ (nguy cơ lỗi hồi quy); số nhánh phình to không kiểm soát |
| 3 | **L + I** – Liskov & Interface Segregation | Kho dữ liệu / interface "ôm mọi thao tác" — dòng ___ | Một kho dữ liệu duy nhất gồm đọc + ghi + báo cáo; bản chỉ-đọc (hoặc máy check-in) bị ép cài cả method không dùng, phải `throw NotSupportedException/NotImplementedException` | Code đang dùng `repo.Add(...)` sập khi gặp bản chỉ-đọc (phá hợp đồng – LSP); class bị ép phụ thuộc method nó không cần (ISP) |
| 4 | **D** – Dependency Inversion | Các dòng `new ...`, `File.AppendAllText`, `Console.WriteLine` trong logic — dòng ___ | Logic nghiệp vụ (cấp cao) phụ thuộc trực tiếp vào class cụ thể và API hệ thống (cấp thấp) | Không thể thay bản giả khi unit test; đổi nơi lưu dữ liệu (EF Core), đổi nơi ghi log phải sửa chính `ClubManager` |

## Đề xuất cách sửa (refactor — đổi cấu trúc, giữ nguyên chức năng)

| # | Cách sửa | Cài đặt trong bài làm |
|---|----------|----------------------|
| 1 | Tách trách nhiệm: `ClubService` chỉ điều phối; kiểm tra tên giao cho **Domain** (`Club.Create`); log và thông báo giao cho `IAppLogger`, `INotificationService`; thống kê giao cho `ReportService` | `Services/ClubService.cs`, `Services/ReportService.cs`, `Domain/Club.cs` |
| 2 | Mỗi kênh một class cài `INotificationService`; chọn kênh bằng **Factory** (một `switch` duy nhất nằm trong nhà máy). Thêm kênh = thêm class + một nhánh trong factory, nơi gọi không đổi | `Infrastructure/Notifications.cs`, `Patterns/NotificationServiceFactory.cs` |
| 3 | Tách interface nhỏ: `IClubReader` (đọc) ⊂ `IClubRepository` (đọc + ghi); `IEventPlanner`, `IEventRegistration`, `IEventCheckIn` thay cho interface béo. Bản chỉ-đọc chỉ ký `IClubReader` nên không hứa có `Add` | `Abstractions/Interfaces.cs`, `ReadOnlyClubRepository` |
| 4 | Service nhận interface qua **constructor** (Dependency Injection). `Program.cs` là **composition root** — nơi duy nhất dùng `new` cho class cụ thể | `Program.cs`, các constructor trong `Services/` |

## Pattern áp dụng (Nhiệm vụ 3)

| Pattern | Vấn đề gốc | Thành phần |
|---------|-----------|-----------|
| Factory | `if/else` chọn kênh thông báo | `NotificationServiceFactory` |
| Decorator | Muốn ghi log cho repository mà không sửa nó | `LoggingClubRepository` |
| Strategy | Xuất báo cáo nhiều định dạng | `IReportStrategy`, `ConsoleReportStrategy`, `CsvReportStrategy` |
| Observer | Tạo sự kiện xong phải báo nhiều bên | `IEventObserver`, `LogNotifier`, `EmailNotifier` |
