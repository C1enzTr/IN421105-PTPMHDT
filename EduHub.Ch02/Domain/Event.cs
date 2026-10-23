namespace EduHub.Ch02.Domain;

/// <summary>Sự kiện của câu lạc bộ: đăng ký trước, check-in sau.</summary>
public sealed class Event
{
    private readonly HashSet<Guid> _registered = new();
    private readonly HashSet<Guid> _checkedIn = new();

    public Guid Id { get; private set; }
    public Guid ClubId { get; private set; }
    public string Title { get; private set; }
    public DateTime Date { get; private set; }

    public int RegisteredCount => _registered.Count;
    public int CheckedInCount => _checkedIn.Count;

    private Event(Guid clubId, string title, DateTime date)
    {
        Id = Guid.NewGuid();
        ClubId = clubId;
        Title = title;
        Date = date;
    }

    public static Event Create(Guid clubId, string title, DateTime date)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Tiêu đề sự kiện không được để trống.", nameof(title));

        return new Event(clubId, title.Trim(), date);
    }

    public void Register(Guid memberId)
    {
        if (!_registered.Add(memberId))
            throw new InvalidOperationException("Thành viên đã đăng ký sự kiện này.");
    }

    public void CheckIn(Guid memberId)
    {
        if (!_registered.Contains(memberId))
            throw new InvalidOperationException("Thành viên chưa đăng ký nên không thể check-in.");
        if (!_checkedIn.Add(memberId))
            throw new InvalidOperationException("Thành viên đã check-in rồi.");
    }
}
