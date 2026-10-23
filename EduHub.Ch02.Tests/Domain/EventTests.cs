using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Tests.Domain;

public class EventTests
{
    private static Event NewEvent() =>
        Event.Create(Guid.NewGuid(), "Workshop SOLID", new DateTime(2026, 11, 20));

    [Fact]
    public void CheckIn_UnregisteredMember_ThrowsInvalidOperationException()
    {
        // Arrange
        var ev = NewEvent();

        // Act
        var act = () => ev.CheckIn(Guid.NewGuid());   // chưa đăng ký mà check-in

        // Assert
        Assert.Throws<InvalidOperationException>(act);
    }

    [Fact]
    public void CheckIn_RegisteredMember_IncreasesCheckedInCount()
    {
        // Arrange
        var ev = NewEvent();
        var memberId = Guid.NewGuid();
        ev.Register(memberId);

        // Act
        ev.CheckIn(memberId);

        // Assert
        Assert.Equal(1, ev.CheckedInCount);
    }

    [Fact]
    public void Register_SameMemberTwice_ThrowsInvalidOperationException()
    {
        // Arrange
        var ev = NewEvent();
        var memberId = Guid.NewGuid();
        ev.Register(memberId);

        // Act
        var act = () => ev.Register(memberId);

        // Assert
        Assert.Throws<InvalidOperationException>(act);
    }

    [Fact]
    public void Create_WithEmptyTitle_ThrowsArgumentException()
    {
        // Arrange
        var clubId = Guid.NewGuid();

        // Act
        var act = () => Event.Create(clubId, " ", DateTime.Today);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }
}
