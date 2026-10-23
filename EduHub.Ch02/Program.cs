using EduHub.Ch02.Abstractions;
using EduHub.Ch02.Infrastructure;
using EduHub.Ch02.Patterns;
using EduHub.Ch02.Services;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// ===== Composition root: NƠI DUY NHẤT dùng new cho class cụ thể =====
IAppLogger logger = new ConsoleLogger();                // đổi sang new FileLogger("club-log.txt") nếu muốn ghi file
INotificationService notifier = new EmailNotificationService();

IClubRepository clubRepo = new LoggingClubRepository(new InMemoryClubRepository(), logger); // Decorator
IMemberRepository memberRepo = new InMemoryMemberRepository();
IEventRepository eventRepo = new InMemoryEventRepository();

var clubService = new ClubService(clubRepo, memberRepo, notifier, logger);
var eventService = new EventService(clubRepo, eventRepo);
var reportService = new ReportService(clubRepo, memberRepo, eventRepo);
INotificationServiceFactory factory = new NotificationServiceFactory();

// ===== 1. Tạo CLB và thành viên =====
Console.WriteLine("== 1. Tạo CLB ==");
var club = clubService.CreateClub("Tech Club", "Câu lạc bộ công nghệ", "president@student.dthu.edu.vn");
var m1 = clubService.AddMember(club.Id, "An", "a@student.dthu.edu.vn");
var m2 = clubService.AddMember(club.Id, "Bình", "b@student.dthu.edu.vn");

// ===== 2. Observer =====
Console.WriteLine("\n== 2. Observer: tạo sự kiện -> 2 observer được báo ==");
eventService.Subscribe(new LogNotifier(logger));
eventService.Subscribe(new EmailNotifier(notifier, memberRepo));
var ev = eventService.CreateEvent(club.Id, "Workshop SOLID", new DateTime(2026, 11, 20));
eventService.Register(ev.Id, m1.Id);
eventService.Register(ev.Id, m2.Id);
eventService.CheckIn(ev.Id, m1.Id);

// ===== 3. Strategy =====
Console.WriteLine("\n== 3. Strategy: báo cáo ==");
Console.WriteLine(reportService.Generate(new ConsoleReportStrategy()));
Console.WriteLine(reportService.Generate(new CsvReportStrategy()));

// ===== 4. Factory =====
Console.WriteLine("\n== 4. Factory: tạo kênh thông báo theo yêu cầu ==");
foreach (var channel in Enum.GetValues<NotificationChannel>())
    factory.Create(channel).Send("a@student.dthu.edu.vn", $"Thử kênh {channel}");

// ===== 5. Kiểm tra dữ liệu sai =====
Console.WriteLine("\n== 5. Kiểm tra dữ liệu sai ==");
try { clubService.CreateClub("   ", "x", "p@dthu.edu.vn"); }
catch (ArgumentException ex) { Console.WriteLine($"  Lỗi đúng dự kiến: {ex.Message}"); }

try { eventService.CheckIn(ev.Id, Guid.NewGuid()); }
catch (InvalidOperationException ex) { Console.WriteLine($"  Lỗi đúng dự kiến: {ex.Message}"); }
