using EduHub.Ch02.Abstractions;
using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Patterns;

// OBSERVER: các bên đăng ký nghe, EventService không cần biết họ làm gì.
public class LogNotifier : IEventObserver
{
    private readonly IAppLogger _logger;
    public LogNotifier(IAppLogger logger) => _logger = logger;

    public void OnEventCreated(Event ev, Club club) =>
        _logger.Info($"Event created: '{ev.Title}' ({club.Name})");
}

public class EmailNotifier : IEventObserver
{
    private readonly INotificationService _notifier;
    private readonly IMemberRepository _members;

    public EmailNotifier(INotificationService notifier, IMemberRepository members)
    {
        _notifier = notifier;
        _members = members;
    }

    public void OnEventCreated(Event ev, Club club)
    {
        foreach (var m in _members.GetByClub(club.Id))
            _notifier.Send(m.Email, $"New event '{ev.Title}' of {club.Name} on {ev.Date:dd/MM/yyyy}");
    }
}
