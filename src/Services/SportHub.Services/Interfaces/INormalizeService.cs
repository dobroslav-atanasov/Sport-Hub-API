namespace SportHub.Services.Interfaces;

using SportHub.Data.Models.Converters.OlympicGames;
using SportHub.Data.Models.Enumerations.OlympicGames;

public interface INormalizeService
{
    string MapOlympicGamesCountriesAndWorldCountries(string code);

    string NormalizeHostCityName(string hostCity);

    string NormalizeEventName(ConverterModel model);

    string ReplaceNonEnglishLetters(string name);

    string MapCityNameAndYearToNOCCode(string cityName, int year);

    string CleanEventName(string text);

    RoundDataModel MapRoundData(string name);

    RoundEnum MapAdditionalRound(string name);

    string MapCityToCountry(string city);
}