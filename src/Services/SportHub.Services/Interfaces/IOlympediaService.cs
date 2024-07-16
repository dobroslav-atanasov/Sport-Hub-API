namespace SportHub.Services.Interfaces;

using SportHub.Data.Models.Converters;
using SportHub.Data.Models.Converters.OlympicGames;
using SportHub.Data.Models.Converters.OlympicGames.Base;
using SportHub.Data.Models.Enumerations.OlympicGames;

public interface IOlympediaService
{
    List<AthleteModel> FindAthletes(string text);

    IList<string> FindNOCCodes(string text);

    List<int> FindVenues(string text);

    AthleteModel FindAthlete(string text);

    List<int> FindClubs(string text);

    string FindNOCCode(string text);

    MedalEnum FindMedal(string text);

    MedalEnum FindMedal(string text, RoundEnum roundType);

    FinishStatusEnum FindFinishStatus(string text);

    int FindMatchNumber(string text);

    int FindResultNumber(string text);

    string FindLocation(string html);

    string FindMatchInfo(string text);

    RecordEnum FindRecord(string text);

    bool IsQualified(string text);

    IList<int> FindResults(string text);

    DecisionEnum FindDecision(string text);

    int FindSeedNumber(string text);

    void SetWinAndLose(MatchModel match);

    Horse FindHorse(string text);

    Dictionary<string, int> GetIndexes(List<string> headers);

    Dictionary<string, int> FindIndexes(List<string> headers);
}