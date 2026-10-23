using EduHub.Ch02.Abstractions;
using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Services;

/// <summary>Thống kê rồi giao cho IReportStrategy vẽ ra định dạng mong muốn.
/// Thêm định dạng mới (PDF, JSON) không phải sửa class này.</summary>
public sealed class ReportService
{
    private readonly IClubReader _clubs;
    private readonly IMemberRepository _members;
    private readonly IEventRepository _events;

    public ReportService(IClubReader clubs, IMemberRepository members, IEventRepository events)
    {
        _clubs = clubs;
        _members = members;
        _events = events;
    }

    public IReadOnlyList<ClubStat> BuildStats() =>
        _clubs.GetAll().Select(club =>
        {
            var events = _events.GetByClub(club.Id);
            return new ClubStat(
                club.Name,
                _members.GetByClub(club.Id).Count,
                events.Count,
                events.Sum(e => e.RegisteredCount),
                events.Sum(e => e.CheckedInCount));
        }).ToList();

    public string Generate(IReportStrategy strategy) => strategy.Render(BuildStats());
}
