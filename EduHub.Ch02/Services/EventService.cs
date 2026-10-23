using EduHub.Ch02.Abstractions;
using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Services;

/// <summary>Quản lý sự kiện. Đồng thời là "người phát tin" của Observer:
/// tạo sự kiện xong thì báo mọi observer đã đăng ký.</summary>
public sealed class EventService : IEventPlanner, IEventRegistration, IEventCheckIn
{
    private readonly IClubReader _clubs;          // chỉ cần đọc => ISP
    private readonly IEventRepository _events;
    private readonly List<IEventObserver> _observers = new();

    public EventService(IClubReader clubs, IEventRepository events)
    {
        _clubs = clubs;
        _events = events;
    }

    public void Subscribe(IEventObserver observer) => _observers.Add(observer);
    public void Unsubscribe(IEventObserver observer) => _observers.Remove(observer);

    public Event CreateEvent(Guid clubId, string title, DateTime date)
    {
        var club = _clubs.GetById(clubId)
                   ?? throw new InvalidOperationException("Không tìm thấy câu lạc bộ.");

        var ev = Event.Create(clubId, title, date);
        _events.Add(ev);

        foreach (var observer in _observers.ToList())
            observer.OnEventCreated(ev, club);

        return ev;
    }

    public void Register(Guid eventId, Guid memberId) => GetEvent(eventId).Register(memberId);

    public void CheckIn(Guid eventId, Guid memberId) => GetEvent(eventId).CheckIn(memberId);

    private Event GetEvent(Guid eventId) =>
        _events.GetById(eventId) ?? throw new InvalidOperationException("Không tìm thấy sự kiện.");
}
