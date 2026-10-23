using EduHub.Ch02.Abstractions;
using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Services;

// SRP: chỉ điều phối nghiệp vụ CLB. DIP: nhận interface qua constructor.
public class ClubService : IClubService
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
        var club = Club.Create(name, description);      // Domain tự kiểm tra
        _clubs.Add(club);                               // repository lưu
        _logger.Info($"Club created: {club.Name}");     // logger ghi
        _notifier.Send(presidentEmail, $"Club '{club.Name}' created"); // notifier gửi
        return club;
    }

    public Member AddMember(Guid clubId, string name, string email)
    {
        var club = _clubs.GetById(clubId)
                   ?? throw new InvalidOperationException("Không tìm thấy CLB.");

        if (!club.HasRoom(_members.GetByClub(clubId).Count))
            throw new InvalidOperationException("CLB đã đủ thành viên.");

        var member = Member.Create(clubId, name, email);
        _members.Add(member);
        _logger.Info($"Member added: {member.Name} -> {club.Name}");
        return member;
    }
}
