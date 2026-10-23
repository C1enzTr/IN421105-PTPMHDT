using EduHub.Ch02.Abstractions;

namespace EduHub.Ch02.Infrastructure;

// OCP: thêm kênh mới = thêm một class, không sửa code cũ.
public class EmailNotificationService : INotificationService
{
    public void Send(string to, string message) => Console.WriteLine($"  [EMAIL] to {to}: {message}");
}

public class SmsNotificationService : INotificationService
{
    public void Send(string to, string message) => Console.WriteLine($"  [SMS] to {to}: {message}");
}

public class SlackNotificationService : INotificationService
{
    public void Send(string to, string message) => Console.WriteLine($"  [SLACK] to {to}: {message}");
}
