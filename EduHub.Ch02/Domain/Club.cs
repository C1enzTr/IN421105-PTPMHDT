namespace EduHub.Ch02.Domain;

/// <summary>Câu lạc bộ. Luật nghiệp vụ (tên hợp lệ, giới hạn thành viên) nằm ở đây.</summary>
public sealed class Club
{
    public const int MaxNameLength = 100;
    public const int MaxMembers = 50;

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }

    private Club(string name, string description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
    }

    public static Club Create(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên câu lạc bộ không được để trống.", nameof(name));

        name = name.Trim();
        if (name.Length > MaxNameLength)
            throw new ArgumentException($"Tên câu lạc bộ tối đa {MaxNameLength} ký tự.", nameof(name));

        return new Club(name, description?.Trim() ?? string.Empty);
    }

    /// <summary>Còn chỗ khi số thành viên hiện tại nhỏ hơn giới hạn (đủ 50 thì hết chỗ).</summary>
    public bool HasRoom(int currentMemberCount) => currentMemberCount < MaxMembers;
}
