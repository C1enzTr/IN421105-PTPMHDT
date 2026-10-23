using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Abstractions;

// ----- Logging / Notification (SRP, DIP) -----
public interface IAppLogger { void Info(string message); }

public interface INotificationService { void Send(string to, string message); }

public enum NotificationChannel { Email, Sms, Slack }

public interface INotificationServiceFactory
{
    INotificationService Create(NotificationChannel channel);
}

// ----- Repository: tách đọc / ghi (ISP + LSP) -----
public interface IClubReader
{
    IReadOnlyList<Club> GetAll();
    Club? GetById(Guid id);
}
public interface IClubRepository : IClubReader { void Add(Club club); }

public interface IMemberRepository
{
    void Add(Member member);
    IReadOnlyList<Member> GetByClub(Guid clubId);
}

public interface IEventRepository
{
    void Add(Event ev);
    Event? GetById(Guid id);
    IReadOnlyList<Event> GetByClub(Guid clubId);
}

// ----- Service (ISP: mỗi vai trò một interface nhỏ) -----
public interface IClubService
{
    Club CreateClub(string name, string description, string presidentEmail);
    Member AddMember(Guid clubId, string name, string email);
}

public interface IEventPlanner { Event CreateEvent(Guid clubId, string title, DateTime date); }
public interface IEventRegistration { void Register(Guid eventId, Guid memberId); }
public interface IEventCheckIn { void CheckIn(Guid eventId, Guid memberId); }

// ----- Strategy -----
public interface IReportStrategy
{
    string Name { get; }
    string Render(IReadOnlyList<ClubStat> stats);
}

public interface IReportService { string Generate(IReportStrategy strategy); }

// ----- Observer -----
public interface IEventObserver { void OnEventCreated(Event ev, Club club); }
