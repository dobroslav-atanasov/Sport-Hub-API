namespace SportData.Services.Interfaces;

using SportData.Data.Models.Converters.OlympicGames;
using SportData.Data.Models.Entities.OlympicGames.Enumerations;

public interface INormalizeService
{
    string MapOlympicGamesCountriesAndWorldCountries(string code);

    string NormalizeHostCityName(string hostCity);

    string NormalizeEventName(string name, int gameYear, string disciplineName);

    string ReplaceNonEnglishLetters(string name);

    string MapCityNameAndYearToNOCCode(string cityName, int year);

    string CleanEventName(string text);

    RoundDataModel MapRoundData(string name);

    RoundTypeEnum MapAdditionalRound(string name);
}