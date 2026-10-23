using EduHub.Ch02.Abstractions;
using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Services;

// Vai trò "người phát tin" của Observer. Cài 3 interface nhỏ (ISP).
public class EventService : IEventPlanner, IEventRegistration, IEventCheckIn
{
    private readonly IClubReader _clubs;
    private readonly IEventRepository _events;
    private readonly List<IEventObserver> _observers = new();

    public EventService(IClubReader clubs, IEventRepository events)
    {
        _clubs = clubs;
        _events = events;
    }

    public void Subscribe(IEventObserver o) => _observers.Add(o);
    public void Unsubscribe(IEventObserver o) => _observers.Remove(o);

    public Event CreateEvent(Guid clubId, string title, DateTime date)
    {
        var club = _clubs.GetById(clubId)
                   ?? throw new InvalidOperationException("Không tìm thấy CLB.");
        var ev = Event.Create(clubId, title, date);
        _events.Add(ev);
        foreach (var o in _observers.ToList()) o.OnEventCreated(ev, club);
        return ev;
    }

    public void Register(Guid eventId, Guid memberId) => GetEvent(eventId).Register(memberId);
    public void CheckIn(Guid eventId, Guid memberId) => GetEvent(eventId).CheckIn(memberId);

    private Event GetEvent(Guid id) =>
        _events.GetById(id) ?? throw new InvalidOperationException("Không tìm thấy sự kiện.");
}
