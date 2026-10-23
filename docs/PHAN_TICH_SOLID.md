# Nhiệm vụ 1 — Phân tích vi phạm SOLID trong `ClubManager`

**Sinh viên:** Trần Minh Trung Kiên — **Mã học phần:** IN421105

## Tổng quan

`ClubManager` là **God class**: một class ôm kiểm tra dữ liệu, lưu CLB/thành viên, ghi log ra file,
gửi thông báo và in báo cáo. Hệ quả: khó đọc (một file ôm cả năm việc), khó kiểm thử (dính trực tiếp
`File` và `Console`), khó mở rộng (thêm kênh SMS phải sửa lại class).

## Bảng vi phạm

| # | Nguyên lý | Vị trí trong `ClubManager.cs` | Mô tả vi phạm | Hậu quả |
|---|-----------|-------------------------------|---------------|---------|
| 1 | **S** – Single Responsibility | Method `CreateClub()` | Một method vừa kiểm tra tên, vừa lưu vào danh sách, vừa `File.AppendAllText` ghi log, vừa `Console.WriteLine` báo tin | Class có ít nhất 4 lý do để sửa; đổi cách ghi log hay cách báo tin đều phải mở `CreateClub` ra sửa, dễ làm hỏng phần khác |
| 2 | **O** – Open/Closed | Đoạn gửi thông báo: chuỗi `if (email.Contains("@gmail.com")) … else if ("@yahoo.com") … else …` | Chọn cách gửi bằng `if/else` theo tên miền email | Thêm SMS hoặc Slack bắt buộc mở method cũ ra sửa (nguy cơ lỗi hồi quy); số nhánh phình to không kiểm soát |
| 3 | **L + I** – Liskov & Interface Segregation | Kho dữ liệu "ôm mọi thao tác" (đọc, ghi, báo cáo trong một kiểu) | Bản chỉ-đọc hoặc máy check-in bị ép cài cả method không dùng, phải `throw NotSupportedException` / `NotImplementedException` | Code gọi `repo.Add(...)` sập khi gặp bản chỉ-đọc (phá hợp đồng, vi phạm LSP); class bị ép phụ thuộc method nó không cần (vi phạm ISP) |
| 4 | **D** – Dependency Inversion | Các chỗ gọi `File.AppendAllText`, `Console.WriteLine` và `new` class cụ thể bên trong logic | Logic nghiệp vụ (cấp cao) phụ thuộc trực tiếp vào class cụ thể và API hệ thống (cấp thấp) | Không thể thay bản giả khi unit test; đổi nơi lưu dữ liệu hay nơi ghi log phải sửa chính `ClubManager` |

## Cách sửa (refactor: đổi cấu trúc, giữ nguyên chức năng) và nơi cài đặt

| # | Cách sửa | Cài đặt trong bài làm |
|---|----------|----------------------|
| 1 | `ClubService` chỉ điều phối. Kiểm tra tên giao cho **Domain** (`Club.Create`), lưu giao repository, ghi log giao `IAppLogger`, báo tin giao `INotificationService`, thống kê giao `ReportService` | `Services/ClubService.cs`, `Services/ReportService.cs`, `Domain/Club.cs` |
| 2 | Mỗi kênh một class cài `INotificationService`. Chọn kênh bằng **Factory** (một `switch` duy nhất nằm trong nhà máy). Thêm kênh = thêm class + một nhánh trong factory, nơi gọi không đổi | `Infrastructure/Notifications.cs`, `Patterns/NotificationServiceFactory.cs` |
| 3 | Tách interface nhỏ: `IClubReader` (đọc) ⊂ `IClubRepository` (đọc + ghi); `IEventPlanner`, `IEventRegistration`, `IEventCheckIn` thay cho interface béo. `ReadOnlyClubRepository` chỉ ký `IClubReader` nên không hứa có `Add` | `Abstractions/Interfaces.cs`, `Infrastructure/Repositories.cs`, `Services/EventService.cs` |
| 4 | Service nhận interface qua **constructor** (Dependency Injection). `Program.cs` là **composition root**, nơi duy nhất dùng `new` cho class cụ thể | `Program.cs`, constructor trong `Services/` |

## Kết quả refactor (Nhiệm vụ 2)

`ClubManager` được tách thành 3 service và nhiều class nhỏ, tất cả phụ thuộc qua interface:

- `ClubService`, `EventService`, `ReportService` (điều phối nghiệp vụ)
- `Club`, `Member`, `Event`, `ClubStat` (Domain, không dính `File`/`Console`)
- `InMemoryClubRepository`, `InMemoryMemberRepository`, `InMemoryEventRepository`, `ConsoleLogger`, `FileLogger`, các kênh Email/SMS/Slack (Infrastructure)

## Pattern áp dụng (Nhiệm vụ 3)

| Pattern | Vấn đề gốc | Thành phần |
|---------|-----------|-----------|
| Factory | `if/else` chọn kênh thông báo | `NotificationServiceFactory` |
| Decorator | Muốn ghi log cho repository mà không sửa nó | `LoggingClubRepository` |
| Strategy | Xuất báo cáo nhiều định dạng | `IReportStrategy`, `ConsoleReportStrategy`, `CsvReportStrategy` |
| Observer | Tạo sự kiện xong phải báo nhiều bên | `IEventObserver`, `LogNotifier`, `EmailNotifier` |
