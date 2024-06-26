namespace SportHub.Services.Interfaces;

using SportHub.Data.Models.Dates;

public interface IDateService
{
    DateTimeModel ParseDate(string text, int year = 0);

    TimeSpan? ParseTime(string text);

    TimeSpan? ParseTimeFromSeconds(string text);
}