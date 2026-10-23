using EduHub.Ch02.Abstractions;
using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Infrastructure;

public class InMemoryClubRepository : IClubRepository
{
    private readonly List<Club> _items = new();
    public void Add(Club club) => _items.Add(club);
    public IReadOnlyList<Club> GetAll() => _items.AsReadOnly();
    public Club? GetById(Guid id) => _items.FirstOrDefault(c => c.Id == id);
}

public class InMemoryMemberRepository : IMemberRepository
{
    private readonly List<Member> _items = new();
    public void Add(Member member) => _items.Add(member);
    public IReadOnlyList<Member> GetByClub(Guid clubId) =>
        _items.Where(m => m.ClubId == clubId).ToList();
}

public class InMemoryEventRepository : IEventRepository
{
    private readonly List<Event> _items = new();
    public void Add(Event ev) => _items.Add(ev);
    public Event? GetById(Guid id) => _items.FirstOrDefault(e => e.Id == id);
    public IReadOnlyList<Event> GetByClub(Guid clubId) =>
        _items.Where(e => e.ClubId == clubId).ToList();
}

// Bản chỉ đọc: chỉ ký IClubReader nên KHÔNG hứa có Add -> đúng LSP.
public class ReadOnlyClubRepository : IClubReader
{
    private readonly IClubReader _source;
    public ReadOnlyClubRepository(IClubReader source) => _source = source;
    public IReadOnlyList<Club> GetAll() => _source.GetAll();
    public Club? GetById(Guid id) => _source.GetById(id);
}
