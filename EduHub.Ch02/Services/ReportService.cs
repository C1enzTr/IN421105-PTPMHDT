using EduHub.Ch02.Abstractions;
using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Services;

// Chỉ thống kê; định dạng đầu ra giao cho IReportStrategy.
public class ReportService : IReportService
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

    public string Generate(IReportStrategy strategy)
    {
        var stats = _clubs.GetAll().Select(c =>
        {
            var evs = _events.GetByClub(c.Id);
            return new ClubStat(c.Name, _members.GetByClub(c.Id).Count, evs.Count,
                                evs.Sum(e => e.RegisteredCount), evs.Sum(e => e.CheckedInCount));
        }).ToList();

        return strategy.Render(stats);
    }
}
