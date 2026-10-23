using EduHub.Ch02.Abstractions;
using EduHub.Ch02.Domain;
using EduHub.Ch02.Infrastructure;
using EduHub.Ch02.Patterns;
using EduHub.Ch02.Services;

namespace EduHub.Ch02.Tests.Services;

// ----- Bản giả (fake) vì mọi thứ đều phụ thuộc interface -----
internal class FakeLogger : IAppLogger
{
    public List<string> Lines { get; } = new();
    public void Info(string message) => Lines.Add(message);
}

internal class FakeNotifier : INotificationService
{
    public List<(string To, string Msg)> Sent { get; } = new();
    public void Send(string to, string message) => Sent.Add((to, message));
}

internal class SpyObserver : IEventObserver
{
    public int Calls;
    public void OnEventCreated(Event ev, Club club) => Calls++;
}

public class ServiceTests
{
    [Fact]
    public void CreateClub_SavesLogsAndNotifiesPresident()
    {
        // Arrange
        var logger = new FakeLogger();
        var notifier = new FakeNotifier();
        var repo = new InMemoryClubRepository();
        var service = new ClubService(repo, new InMemoryMemberRepository(), notifier, logger);

        // Act
        var club = service.CreateClub("Tech Club", "mô tả", "p@dthu.edu.vn");

        // Assert
        Assert.NotNull(repo.GetById(club.Id));
        Assert.Single(notifier.Sent);
        Assert.Equal("p@dthu.edu.vn", notifier.Sent[0].To);
        Assert.Contains(logger.Lines, l => l.Contains("Club created"));
    }

    [Fact]
    public void Factory_CreatesCorrectChannelType()
    {
        // Arrange
        INotificationServiceFactory factory = new NotificationServiceFactory();

        // Act
        var sms = factory.Create(NotificationChannel.Sms);

        // Assert
        Assert.IsType<SmsNotificationService>(sms);
    }

    [Fact]
    public void LoggingDecorator_Add_LogsAndDelegatesToInner()
    {
        // Arrange
        var logger = new FakeLogger();
        var inner = new InMemoryClubRepository();
        IClubRepository repo = new LoggingClubRepository(inner, logger);
        var club = Club.Create("Tech Club", "mô tả");

        // Act
        repo.Add(club);

        // Assert
        Assert.Same(club, inner.GetById(club.Id));        // bản thật vẫn được gọi
        Assert.Contains(logger.Lines, l => l.Contains("[repo] Add club"));
    }

    [Fact]
    public void CreateEvent_NotifiesAllSubscribedObservers()
    {
        // Arrange
        var clubs = new InMemoryClubRepository();
        var club = Club.Create("Tech Club", "mô tả");
        clubs.Add(club);
        var service = new EventService(clubs, new InMemoryEventRepository());
        var o1 = new SpyObserver();
        var o2 = new SpyObserver();
        service.Subscribe(o1);
        service.Subscribe(o2);

        // Act
        service.CreateEvent(club.Id, "Workshop", DateTime.Today);

        // Assert
        Assert.Equal(1, o1.Calls);
        Assert.Equal(1, o2.Calls);
    }

    [Fact]
    public void Report_CsvStrategy_RendersHeaderAndRow()
    {
        // Arrange
        var clubs = new InMemoryClubRepository();
        var club = Club.Create("Tech Club", "mô tả");
        clubs.Add(club);
        var report = new ReportService(clubs, new InMemoryMemberRepository(), new InMemoryEventRepository());

        // Act
        var csv = report.Generate(new CsvReportStrategy());

        // Assert
        Assert.StartsWith("Club,Members,Events,Registered,CheckedIn", csv);
        Assert.Contains("\"Tech Club\",0,0,0,0", csv);
    }
}
