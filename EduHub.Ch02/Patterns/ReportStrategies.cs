using EduHub.Ch02.Abstractions;
using EduHub.Ch02.Domain;

namespace EduHub.Ch02.Patterns;

/// <summary>STRATEGY: mỗi định dạng báo cáo là một class riêng, đổi được lúc chạy.</summary>
public sealed class ConsoleReportStrategy : IReportStrategy
{
    public string Name => "Console";

    public string Render(IReadOnlyList<ClubStat> stats) =>
        string.Join("\n", stats.Select(s =>
            $"Club: {s.ClubName} | Members: {s.Members} | Events: {s.Events} | Registered: {s.Registered} | CheckedIn: {s.CheckedIn}"));
}

public sealed class CsvReportStrategy : IReportStrategy
{
    public string Name => "CSV";

    public string Render(IReadOnlyList<ClubStat> stats) =>
        "Club,Members,Events,Registered,CheckedIn\n" +
        string.Join("\n", stats.Select(s =>
            $"\"{s.ClubName.Replace("\"", "\"\"")}\",{s.Members},{s.Events},{s.Registered},{s.CheckedIn}"));
}
