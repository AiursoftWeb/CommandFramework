namespace Aiursoft.CommandFramework.Tests.CalendarTests;

public class CalendarRenderer
{
    private readonly Random _rand = new();

    public void Render()
    {
        var now = DateTime.Now;
        var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
        Console.WriteLine("| {0,-16} | {1,-16} | {2,-16} | {3,-16} | {4,-16} | {5,-16} | {6,-16} |", "Sunday", "Monday",
            "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday");
        Console.WriteLine("|{0}|{1}|{2}|{3}|{4}|{5}|{6}|", new string('-', 18), new string('-', 18),
            new string('-', 18), new string('-', 18), new string('-', 18), new string('-', 18), new string('-', 18));
        var firstWeekDay = firstDayOfMonth.AddDays(-(int)firstDayOfMonth.DayOfWeek);
        var lastWeekDay = lastDayOfMonth.AddDays(6 - (int)lastDayOfMonth.DayOfWeek);
        for (var date = firstWeekDay; date <= lastWeekDay; date = date.AddDays(1))
        {
            Console.Write("| {0,-4} ", date.Day);
            Console.Write(" {0,-10} ", $"({GetWeather()})");
            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                Console.WriteLine("|");
            }
        }
        Console.WriteLine();
    }

    private string GetWeather()
    {
        string[] weathers = ["Sunny", "Cloudy", "Rainy", "Snowy", "Foggy", "Windy", "Stormy"];

        return weathers[_rand.Next(0, weathers.Length)];
    }
}