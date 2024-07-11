namespace SportHub.Data.Models.DbEntities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using SportHub.Data.Common.Models;
using SportHub.Data.Models.Enumerations.OlympicGames;

[Table("Games", Schema = "dbo")]
public class Game : BaseDeletableEntity<int>
{
    [Required]
    public int Year { get; set; }

    [MaxLength(10)]
    public string Number { get; set; }

    [Required]
    public GameTypeEnum GameType { get; set; }

    [Required]
    [MaxLength(100)]
    public string OfficialName { get; set; }

    [Column(TypeName = "Date")]
    public DateTime? OpeningDate { get; set; }

    [Column(TypeName = "Date")]
    public DateTime? ClosingDate { get; set; }

    [Column(TypeName = "Date")]
    public DateTime? StartCompetitionDate { get; set; }

    [Column(TypeName = "Date")]
    public DateTime? EndCompetitionDate { get; set; }

    [MaxLength(500)]
    public string OpenBy { get; set; }

    [MaxLength(5000)]
    public string Torchbearers { get; set; }

    [MaxLength(500)]
    public string AthleteOathBy { get; set; }

    [MaxLength(500)]
    public string JudgeOathBy { get; set; }

    [MaxLength(500)]
    public string CoachOathBy { get; set; }

    [MaxLength(500)]
    public string OlympicFlagBearers { get; set; }

    [MaxLength(50000)]
    public string Description { get; set; }

    [MaxLength(10000)]
    public string BidProcess { get; set; }

    public virtual ICollection<City> Cities { get; set; } = [];
}