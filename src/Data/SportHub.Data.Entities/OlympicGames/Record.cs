namespace SportHub.Data.Entities.OlympicGames;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using global::SportHub.Data.Common.Models;

[Table("Records", Schema = "dbo")]
public class Record : BaseDeletableEntity<Guid>, IEquatable<Record>
{
    [MaxLength(30)]
    public string Code { get; set; }

    [MaxLength(50)]
    public string Type { get; set; }

    [MaxLength(50)]
    public string Name { get; set; }

    [MaxLength(100)]
    public string Description { get; set; }

    public int Order { get; set; }

    public bool Current { get; set; } = false;

    [DataType("DATETIME2")]
    public DateTime? Date { get; set; }

    [MaxLength(50)]
    public string Result { get; set; }

    [MaxLength(50)]
    public string ResultType { get; set; }

    [MaxLength(50)]
    public string Country { get; set; }

    [MaxLength(200)]
    public string Place { get; set; }

    [MaxLength(500)]
    public string Competition { get; set; }

    public Guid EventId { get; set; }
    public virtual Event Event { get; set; }

    public string Meta { get; set; }

    public bool Equals(Record other)
    {
        var equals = true;

        if (this.Name != other.Name)
        {
            other.Name = this.Name;
            equals = false;
        }

        if (this.Type != other.Type)
        {
            other.Type = this.Type;
            equals = false;
        }

        if (this.Description != other.Description)
        {
            other.Description = this.Description;
            equals = false;
        }

        if (this.Order != other.Order)
        {
            other.Order = this.Order;
            equals = false;
        }

        if (this.Current != other.Current)
        {
            other.Current = this.Current;
            equals = false;
        }

        if (this.Date != other.Date)
        {
            other.Date = this.Date;
            equals = false;
        }

        //if (this.Result != other.Result)
        //{
        //    other.Result = this.Result;
        //    equals = false;
        //}

        if (this.ResultType != other.ResultType)
        {
            other.ResultType = this.ResultType;
            equals = false;
        }

        if (this.Country != other.Country)
        {
            other.Country = this.Country;
            equals = false;
        }

        if (this.Place != other.Place)
        {
            other.Place = this.Place;
            equals = false;
        }

        if (this.Competition != other.Competition)
        {
            other.Competition = this.Competition;
            equals = false;
        }

        if (this.Meta != other.Meta)
        {
            other.Meta = this.Meta;
            equals = false;
        }

        return equals;
    }

    public override bool Equals(object obj)
    {
        return obj is Record && this.Equals((Record)obj);
    }
}