namespace EduHub.Ch02.Domain;

/// <summary>Số liệu thống kê của một câu lạc bộ, dùng để xuất báo cáo.</summary>
public sealed record ClubStat(string ClubName, int Members, int Events, int Registered, int CheckedIn);
