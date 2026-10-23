namespace EduHub.Ch02.Domain;

public class Club
{
    public const int MaxMembers = 50;
    public const int MaxNameLength = 100;

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    private Club() { }

    /// <summary>Domain tự kiểm tra luật: tên không rỗng, tối đa 100 ký tự.</summary>
    public static Club Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên CLB không được để trống.", nameof(name));
        if (name.Trim().Length > MaxNameLength)
            throw new ArgumentException($"Tên CLB tối đa {MaxNameLength} ký tự.", nameof(name));

        return new Club
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Description = description?.Trim() ?? string.Empty
        };
    }

    /// <summary>Còn chỗ khi số thành viên hiện tại nhỏ hơn giới hạn.</summary>
    public bool HasRoom(int currentMemberCount) => currentMemberCount < MaxMembers;
}
