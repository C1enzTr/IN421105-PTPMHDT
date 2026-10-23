using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Tests.Domain;

public class MemberTests
{
    [Fact]
    public void Create_WithValidData_ReturnsMemberInClub()
    {
        // Arrange
        var clubId = Guid.NewGuid();

        // Act
        var m = Member.Create(clubId, "An", "a@student.dthu.edu.vn");

        // Assert
        Assert.Equal(clubId, m.ClubId);
        Assert.Equal("a@student.dthu.edu.vn", m.Email);
    }

    [Theory]
    [InlineData("khong-co-a-cong")]
    [InlineData("")]
    public void Create_WithInvalidEmail_ThrowsArgumentException(string email)
    {
        // Arrange
        var clubId = Guid.NewGuid();

        // Act
        var act = () => Member.Create(clubId, "An", email);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }
}
