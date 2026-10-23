namespace EduHub.Ch02.Domain;

public class Member
{
    public Guid Id { get; private set; }
    public Guid ClubId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;

    private Member() { }

    public static Member Create(Guid clubId, string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên thành viên không được để trống.", nameof(name));
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new ArgumentException("Email không hợp lệ.", nameof(email));

        return new Member
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            Name = name.Trim(),
            Email = email.Trim()
        };
    }
}
