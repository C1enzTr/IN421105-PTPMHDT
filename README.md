# EduHub — Bài tập Ch02: Refactoring

## Chạy
```bash
dotnet build
dotnet test
dotnet run --project EduHub.Ch02
```
(Project dùng `net10.0`; nếu SDK của bạn khác, đổi `TargetFramework` trong 2 file `.csproj`.)

## Cấu trúc
```
EduHub.Ch02/
  Domain/          Club, Member, Event, ClubStat  — luật nghiệp vụ, không dính File/Console
  Abstractions/    Interface (IClubRepository, INotificationService, IAppLogger, ...)
  Services/        ClubService, EventService, ReportService
  Infrastructure/  Repository in-memory, Logger, kênh Email/SMS/Slack
  Patterns/        Factory, Decorator, Strategy, Observer
  Program.cs       Composition root — nơi duy nhất new class cụ thể
EduHub.Ch02.Tests/ xUnit, Arrange-Act-Assert
docs/              Phân tích SOLID, mẫu PR, hướng dẫn Git
```
