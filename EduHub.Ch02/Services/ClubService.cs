using EduHub.Ch02.Abstractions;
using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Services;

/// <summary>Chỉ điều phối nghiệp vụ CLB: kiểm tra giao Domain, lưu giao repository,
/// ghi log giao IAppLogger, báo tin giao INotificationService.</summary>
public sealed class ClubService : IClubService
{
    private readonly IClubRepository _clubs;
    private readonly IMemberRepository _members;
    private readonly INotificationService _notifier;
    private readonly IAppLogger _logger;

    public ClubService(IClubRepository clubs, IMemberRepository members,
                       INotificationService notifier, IAppLogger logger)
    {
        _clubs = clubs;
        _members = members;
        _notifier = notifier;
        _logger = logger;
    }

    public Club CreateClub(string name, string description, string presidentEmail)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(presidentEmail);

        var club = Club.Create(name, description);          // Domain tự kiểm tra tên
        _clubs.Add(club);                                   // repository lưu
        _logger.Info($"Club created: {club.Name}");         // logger ghi
        _notifier.Send(presidentEmail, $"Club '{club.Name}' created"); // notifier gửi
        return club;
    }

    public Member AddMember(Guid clubId, string name, string email)
    {
        var club = _clubs.GetById(clubId)
                   ?? throw new InvalidOperationException("Không tìm thấy câu lạc bộ.");

        if (!club.HasRoom(_members.GetByClub(clubId).Count))
            throw new InvalidOperationException($"Câu lạc bộ '{club.Name}' đã đủ {Club.MaxMembers} thành viên.");

        var member = Member.Create(clubId, name, email);
        _members.Add(member);
        _logger.Info($"Member added: {member.Name} -> {club.Name}");
        _notifier.Send(member.Email, $"Chào mừng bạn đến với {club.Name}!");
        return member;
    }
}
