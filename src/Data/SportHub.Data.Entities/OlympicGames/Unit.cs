namespace SportHub.Data.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("Units", Schema = "dbo")]
public class Unit : BaseDeletableEntity<Guid>, IEquatable<Unit>
{
    [Required]
    [MaxLength(50)]
    public string Code { get; set; }

    /// <summary>
    /// description
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    /// <summary>
    /// long description
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string LongName { get; set; }

    /// <summary>
    /// short description
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string ShortName { get; set; }

    /// <summary>
    /// seo description
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string SEOName { get; set; }

    /// <summary>
    /// code
    /// </summary>
    [MaxLength(50)]
    public string OriginalCode { get; set; }

    /// <summary>
    /// type
    /// </summary>
    [MaxLength(10)]
    public string Type { get; set; }

    /// <summary>
    /// scheduled
    /// </summary>
    [MaxLength(10)]
    public string Scheduled { get; set; }

    public bool IsResult { get; set; } = false;

    public DateTime? OlympicDay { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [MaxLength(50)]
    public string VenueCode { get; set; }

    [MaxLength(200)]
    public string Venue { get; set; }

    [MaxLength(50)]
    public string LocationCode { get; set; }

    [MaxLength(200)]
    public string Location { get; set; }

    public bool IsMedal { get; set; } = false;

    [MaxLength(200)]
    public string Session { get; set; }

    [MaxLength(200)]
    public string OrderText { get; set; }

    public int Order { get; set; }

    [MaxLength(200)]
    public string Status { get; set; }

    [MaxLength(200)]
    public string Number { get; set; }

    [MaxLength(200)]
    public string Group { get; set; }

    [MaxLength(200)]
    public string ResultCode { get; set; }

    public int Competitors { get; set; }

    public Guid PhaseId { get; set; }
    public virtual Phase Phase { get; set; }

    public bool Equals(Unit other)
    {
        var equal = true;

        if (this.Name != other.Name)
        {
            other.Name = this.Name;
            equal = false;
        }

        if (this.LongName != other.LongName)
        {
            other.LongName = this.LongName;
            equal = false;
        }

        if (this.ShortName != other.ShortName)
        {
            other.ShortName = this.ShortName;
            equal = false;
        }

        if (this.SEOName != other.SEOName)
        {
            other.SEOName = this.SEOName;
            equal = false;
        }

        if (this.Type != other.Type)
        {
            other.Type = this.Type;
            equal = false;
        }

        if (this.Scheduled != other.Scheduled)
        {
            other.Scheduled = this.Scheduled;
            equal = false;
        }

        if (this.IsResult != other.IsResult)
        {
            other.IsResult = this.IsResult;
            equal = false;
        }

        if (this.OlympicDay != other.OlympicDay)
        {
            other.OlympicDay = this.OlympicDay;
            equal = false;
        }

        if (this.StartDate != other.StartDate)
        {
            other.StartDate = this.StartDate;
            equal = false;
        }

        if (this.EndDate != other.EndDate)
        {
            other.EndDate = this.EndDate;
            equal = false;
        }

        if (this.VenueCode != other.VenueCode)
        {
            other.VenueCode = this.VenueCode;
            equal = false;
        }

        if (this.Venue != other.Venue)
        {
            other.Venue = this.Venue;
            equal = false;
        }

        if (this.IsMedal != other.IsMedal)
        {
            other.IsMedal = this.IsMedal;
            equal = false;
        }

        if (this.Session != other.Session)
        {
            other.Session = this.Session;
            equal = false;
        }

        if (this.OrderText != other.OrderText)
        {
            other.OrderText = this.OrderText;
            equal = false;
        }

        if (this.Order != other.Order)
        {
            other.Order = this.Order;
            equal = false;
        }

        if (this.Status != other.Status)
        {
            other.Status = this.Status;
            equal = false;
        }

        if (this.Number != other.Number)
        {
            other.Number = this.Number;
            equal = false;
        }

        if (this.Group != other.Group)
        {
            other.Group = this.Group;
            equal = false;
        }

        if (this.ResultCode != other.ResultCode)
        {
            other.ResultCode = this.ResultCode;
            equal = false;
        }

        if (this.Competitors != other.Competitors)
        {
            other.Competitors = this.Competitors;
            equal = false;
        }

        if (this.Location != other.Location)
        {
            other.Location = this.Location;
            equal = false;
        }

        if (this.LocationCode != other.LocationCode)
        {
            other.LocationCode = this.LocationCode;
            equal = false;
        }

        return equal;
    }

    public override bool Equals(object obj)
    {
        return obj is Unit && this.Equals((Unit)obj);
    }
}