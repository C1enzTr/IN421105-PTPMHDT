using EduHub.Ch02.Abstractions;
using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Patterns;

/// <summary>DECORATOR: cùng interface với repository thật, bọc nó lại để thêm ghi log
/// mà không sửa class gốc. Bỏ decorator ở Program.cs thì dòng log biến mất, service không đổi.</summary>
public sealed class LoggingClubRepository : IClubRepository
{
    private readonly IClubRepository _inner;
    private readonly IAppLogger _logger;

    public LoggingClubRepository(IClubRepository inner, IAppLogger logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public void Add(Club club)
    {
        _logger.Info($"[repo] Add club '{club.Name}'");
        _inner.Add(club);
    }

    public IReadOnlyList<Club> GetAll()
    {
        _logger.Info("[repo] GetAll clubs");
        return _inner.GetAll();
    }

    public Club? GetById(Guid id)
    {
        _logger.Info($"[repo] GetById {id}");
        return _inner.GetById(id);
    }
}
