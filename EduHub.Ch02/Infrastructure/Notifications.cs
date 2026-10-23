using EduHub.Ch02.Abstractions;

namespace EduHub.Ch02.Infrastructure;

// Mỗi kênh một class cài INotificationService (OCP: thêm kênh = thêm class).
public sealed class EmailNotificationService : INotificationService
{
    private readonly TextWriter _out;
    public EmailNotificationService(TextWriter? output = null) => _out = output ?? Console.Out;
    public void Send(string to, string message) => _out.WriteLine($"[EMAIL] to {to}: {message}");
}

public sealed class SmsNotificationService : INotificationService
{
    private readonly TextWriter _out;
    public SmsNotificationService(TextWriter? output = null) => _out = output ?? Console.Out;
    public void Send(string to, string message) => _out.WriteLine($"[SMS] to {to}: {message}");
}

public sealed class SlackNotificationService : INotificationService
{
    private readonly TextWriter _out;
    public SlackNotificationService(TextWriter? output = null) => _out = output ?? Console.Out;
    public void Send(string to, string message) => _out.WriteLine($"[SLACK] to {to}: {message}");
}
