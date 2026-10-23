using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Tests.Domain;

public class ClubTests
{
    [Fact]
    public void Create_WithValidName_ReturnsClubWithNameAndNewId()
    {
        // Arrange
        const string name = "Tech Club";

        // Act
        var club = Club.Create(name, "Câu lạc bộ công nghệ");

        // Assert
        Assert.Equal(name, club.Name);
        Assert.NotEqual(Guid.Empty, club.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_WithEmptyName_ThrowsArgumentException(string? name)
    {
        // Arrange
        // (dữ liệu từ InlineData)

        // Act
        var act = () => Club.Create(name!, "mô tả");

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Create_WithNameOver100Chars_ThrowsArgumentException()
    {
        // Arrange
        var longName = new string('a', Club.MaxNameLength + 1);

        // Act
        var act = () => Club.Create(longName, "mô tả");

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(49, true)]
    [InlineData(50, false)]   // biên: đủ 50 thì hết chỗ
    [InlineData(51, false)]
    public void HasRoom_DependsOnCurrentMemberCount(int count, bool expected)
    {
        // Arrange
        var club = Club.Create("Tech Club", "mô tả");

        // Act
        var result = club.HasRoom(count);

        // Assert
        Assert.Equal(expected, result);
    }
}
