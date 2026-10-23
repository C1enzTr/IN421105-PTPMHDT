using EduHub.Ch02.Abstractions;
using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Patterns;

/// <summary>OBSERVER: mỗi bên quan tâm tới sự kiện mới là một observer.
/// Thêm bên mới (ví dụ SMS) chỉ cần viết thêm class và Subscribe, EventService không đổi.</summary>
public sealed class LogNotifier : IEventObserver
{
    private readonly IAppLogger _logger;

    public LogNotifier(IAppLogger logger) => _logger = logger;

    public void OnEventCreated(Event ev, Club club) =>
        _logger.Info($"[LOG] Event created: '{ev.Title}' ({club.Name})");
}

public sealed class EmailNotifier : IEventObserver
{
    private readonly IMemberRepository _members;
    private readonly INotificationService _notifier;

    public EmailNotifier(IMemberRepository members, INotificationService notifier)
    {
        _members = members;
        _notifier = notifier;
    }

    public void OnEventCreated(Event ev, Club club)
    {
        foreach (var member in _members.GetByClub(club.Id))
            _notifier.Send(member.Email, $"New event '{ev.Title}' on {ev.Date:dd/MM/yyyy} by {club.Name}");
    }
}
