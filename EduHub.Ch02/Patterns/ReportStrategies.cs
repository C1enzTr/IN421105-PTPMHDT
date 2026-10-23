using EduHub.Ch02.Abstractions;
using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Patterns;

// STRATEGY: mỗi định dạng báo cáo là một class riêng.
public class ConsoleReportStrategy : IReportStrategy
{
    public string Name => "Console";

    public string Render(IReadOnlyList<ClubStat> stats) =>
        string.Join(Environment.NewLine, stats.Select(s =>
            $"Club: {s.ClubName} | Members: {s.Members} | Events: {s.Events} | " +
            $"Registered: {s.Registered} | CheckedIn: {s.CheckedIn}"));
}

public class CsvReportStrategy : IReportStrategy
{
    public string Name => "CSV";

    public string Render(IReadOnlyList<ClubStat> stats) =>
        "Club,Members,Events,Registered,CheckedIn" + Environment.NewLine +
        string.Join(Environment.NewLine, stats.Select(s =>
            $"\"{s.ClubName}\",{s.Members},{s.Events},{s.Registered},{s.CheckedIn}"));
}
