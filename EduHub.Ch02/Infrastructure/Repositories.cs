using EduHub.Ch02.Abstractions;
using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Infrastructure;

public sealed class InMemoryClubRepository : IClubRepository
{
    private readonly List<Club> _items = new();

    public void Add(Club club) => _items.Add(club);
    public IReadOnlyList<Club> GetAll() => _items.ToList();
    public Club? GetById(Guid id) => _items.FirstOrDefault(c => c.Id == id);
}

/// <summary>Bản chỉ đọc: chỉ ký IClubReader nên không hứa có Add (đúng LSP).</summary>
public sealed class ReadOnlyClubRepository : IClubReader
{
    private readonly IReadOnlyList<Club> _items;

    public ReadOnlyClubRepository(IEnumerable<Club> items) => _items = items.ToList();

    public IReadOnlyList<Club> GetAll() => _items;
    public Club? GetById(Guid id) => _items.FirstOrDefault(c => c.Id == id);
}

public sealed class InMemoryMemberRepository : IMemberRepository
{
    private readonly List<Member> _items = new();

    public void Add(Member member) => _items.Add(member);
    public IReadOnlyList<Member> GetByClub(Guid clubId) =>
        _items.Where(m => m.ClubId == clubId).ToList();
}

public sealed class InMemoryEventRepository : IEventRepository
{
    private readonly List<Event> _items = new();

    public void Add(Event ev) => _items.Add(ev);
    public Event? GetById(Guid id) => _items.FirstOrDefault(e => e.Id == id);
    public IReadOnlyList<Event> GetByClub(Guid clubId) =>
        _items.Where(e => e.ClubId == clubId).ToList();
}
