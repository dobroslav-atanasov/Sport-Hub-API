namespace SportData.Data.Models.Converters.OlympicGames.Disciplines;

using SportData.Data.Models.Converters.OlympicGames.Base;

public class Fencing : BaseModel
{
    public int? Points { get; set; }

    public int? Wins { get; set; }

    public int? Losses { get; set; }

    public int? Draws { get; set; }

    public int? BarrageWins { get; set; }

    public int? BarrageLosses { get; set; }

    public int? TouchesDelivered { get; set; }

    public int? TouchesReceived { get; set; }

    public int? YellowCards { get; set; }

    public int? RedCards { get; set; }

    public int? Period1Points { get; set; }

    public int? Period2Points { get; set; }

    public int? Period3Points { get; set; }

    public List<Fencing> Athletes { get; set; } = [];
}