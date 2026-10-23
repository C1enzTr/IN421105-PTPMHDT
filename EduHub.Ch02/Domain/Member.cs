namespace EduHub.Ch02.Domain;

/// <summary>Thành viên của một câu lạc bộ.</summary>
public sealed class Member
{
    public Guid Id { get; private set; }
    public Guid ClubId { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }

    private Member(Guid clubId, string name, string email)
    {
        Id = Guid.NewGuid();
        ClubId = clubId;
        Name = name;
        Email = email;
    }

    public static Member Create(Guid clubId, string name, string email)
    {
        if (clubId == Guid.Empty)
            throw new ArgumentException("Thành viên phải thuộc một câu lạc bộ.", nameof(clubId));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên thành viên không được để trống.", nameof(name));
        if (!IsValidEmail(email))
            throw new ArgumentException("Email không hợp lệ.", nameof(email));

        return new Member(clubId, name.Trim(), email.Trim());
    }

    private static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        var at = email.Trim().IndexOf('@');
        return at > 0 && at < email.Trim().Length - 1;
    }
}
