using EduHub.Ch02.Abstractions;

namespace EduHub.Ch02.Infrastructure;

public sealed class ConsoleLogger : IAppLogger
{
    public void Info(string message) => Console.WriteLine(message);
}

/// <summary>Ghi log ra file. Đổi sang bản này chỉ cần sửa một dòng ở Program.cs.</summary>
public sealed class FileLogger : IAppLogger
{
    private readonly string _path;

    public FileLogger(string path) => _path = path;

    public void Info(string message) =>
        File.AppendAllText(_path, $"{DateTime.Now:s} {message}{Environment.NewLine}");
}
