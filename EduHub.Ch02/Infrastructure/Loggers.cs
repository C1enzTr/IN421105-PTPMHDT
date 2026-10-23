using EduHub.Ch02.Abstractions;

namespace EduHub.Ch02.Infrastructure;

public class ConsoleLogger : IAppLogger
{
    public void Info(string message) => Console.WriteLine($"  [LOG] {message}");
}

public class FileLogger : IAppLogger
{
    private readonly string _path;
    public FileLogger(string path) => _path = path;
    public void Info(string message) => File.AppendAllText(_path, message + Environment.NewLine);
}
