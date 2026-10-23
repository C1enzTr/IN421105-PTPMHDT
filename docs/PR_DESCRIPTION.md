**Sinh viên:** Trần Minh Trung Kiên — **Mã học phần:** IN421105

## What
Tách `ClubManager` (God class) thành `ClubService`, `EventService`, `ReportService`, phụ thuộc qua interface
(`IClubRepository`, `IMemberRepository`, `IEventRepository`, `INotificationService`, `IAppLogger`).
Áp dụng 4 pattern: Factory, Decorator, Strategy, Observer. Thêm unit test xUnit (Arrange-Act-Assert) cho lớp Domain và Service.

## Why
`ClubManager` vi phạm SRP (làm 4–5 việc), OCP (if/else theo kênh), LSP/ISP (kho dữ liệu ôm mọi thao tác),
DIP (gọi thẳng `File`, `Console`, `new`). Chi tiết: `docs/PHAN_TICH_SOLID.md`.

## How to test
```bash
dotnet build
dotnet test
dotnet run --project EduHub.Ch02
```

## Ghi chú rebase
- Đã `git rebase origin/main` trước khi mở PR, chạy lại `dotnet build` và `dotnet test` sau khi rebase.
