// EduHub — Bài tập Ch02: Refactoring (SOLID + Design Patterns)
// Sinh viên: Trần Minh Trung Kiên — Mã học phần: IN421105
//
// Program.cs là COMPOSITION ROOT: nơi duy nhất dùng `new` cho class cụ thể,
// rồi đưa vào constructor của các service (Dependency Injection thủ công).

using EduHub.Ch02.Abstractions;
using EduHub.Ch02.Domain;
using EduHub.Ch02.Infrastructure;
using EduHub.Ch02.Patterns;
using EduHub.Ch02.Services;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// ----- Ráp các class lại -----
IAppLogger logger = new ConsoleLogger();
INotificationService notifier = new EmailNotificationService();

IClubRepository clubRepo = new LoggingClubRepository(new InMemoryClubRepository(), logger); // Decorator
IMemberRepository memberRepo = new InMemoryMemberRepository();
IEventRepository eventRepo = new InMemoryEventRepository();

IClubService clubService = new ClubService(clubRepo, memberRepo, notifier, logger);
var eventService = new EventService(clubRepo, eventRepo);
var reportService = new ReportService(clubRepo, memberRepo, eventRepo);
INotificationServiceFactory factory = new NotificationServiceFactory();

// ----- 1. Tạo CLB và thành viên -----
Console.WriteLine("== 1. Tạo CLB và thêm thành viên ==");
var club = clubService.CreateClub("Tech Club", "Câu lạc bộ công nghệ", "president@dthu.edu.vn");
var an = clubService.AddMember(club.Id, "An", "a@student.dthu.edu.vn");
var binh = clubService.AddMember(club.Id, "Bình", "b@student.dthu.edu.vn");

// ----- 2. Observer -----
Console.WriteLine();
Console.WriteLine("== 2. Observer: tạo sự kiện -> 2 observer được báo ==");
eventService.Subscribe(new LogNotifier(logger));
eventService.Subscribe(new EmailNotifier(memberRepo, notifier));

var workshop = eventService.CreateEvent(club.Id, "Workshop SOLID", new DateTime(2026, 11, 20));
eventService.Register(workshop.Id, an.Id);
eventService.Register(workshop.Id, binh.Id);
eventService.CheckIn(workshop.Id, an.Id);

// ----- 3. Strategy -----
Console.WriteLine();
Console.WriteLine("== 3. Strategy: báo cáo Console và CSV ==");
foreach (IReportStrategy strategy in new IReportStrategy[] { new ConsoleReportStrategy(), new CsvReportStrategy() })
{
    Console.WriteLine($"-- {strategy.Name} --");
    Console.WriteLine(reportService.Generate(strategy));
}

// ----- 4. Factory -----
Console.WriteLine();
Console.WriteLine("== 4. Factory: tạo kênh thông báo theo yêu cầu ==");
foreach (var channel in Enum.GetValues<NotificationChannel>())
    factory.Create(channel).Send("a@student.dthu.edu.vn", $"Thử kênh {channel}");

// ----- 5. Kiểm tra dữ liệu sai (luật nghiệp vụ nằm ở Domain) -----
Console.WriteLine();
Console.WriteLine("== 5. Kiểm tra dữ liệu sai ==");
TryRun("Tên CLB rỗng", () => clubService.CreateClub("  ", "mô tả", "p@dthu.edu.vn"));
TryRun("Email sai", () => clubService.AddMember(club.Id, "Chi", "khong-co-a-cong"));
TryRun("Check-in khi chưa đăng ký", () => eventService.CheckIn(workshop.Id, Guid.NewGuid()));
TryRun("Đăng ký trùng", () => eventService.Register(workshop.Id, an.Id));

static void TryRun(string label, Action action)
{
    try
    {
        action();
        Console.WriteLine($"  [?] {label}: không có lỗi (bất thường)");
    }
    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
    {
        Console.WriteLine($"  [OK] {label} -> {ex.GetType().Name}: {ex.Message}");
    }
}
