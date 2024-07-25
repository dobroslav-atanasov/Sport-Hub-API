namespace SportHub.Data.Models.Crawlers.Paris2024.PDFs;

using System.Text.Json.Serialization;

public class PDFList
{
    [JsonPropertyName("pdfs")]
    public List<Pdf> Pdfs { get; set; }
}

public class Event
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("isTeam")]
    public bool IsTeam { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }
}

public class EventUnit
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("phase")]
    public Phase Phase { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("scheduled")]
    public string Scheduled { get; set; }

    [JsonPropertyName("order")]
    public string Order { get; set; }

    [JsonPropertyName("medal")]
    public int Medal { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("shortDescription")]
    public string ShortDescription { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }

    [JsonPropertyName("schedule")]
    public Schedule Schedule { get; set; }
}

public class H1
{
    [JsonPropertyName("orisNo")]
    public string OrisNo { get; set; }

    [JsonPropertyName("disciplineCode")]
    public string DisciplineCode { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}

public class Pdf
{
    [JsonPropertyName("pdfCode")]
    public string PdfCode { get; set; }

    [JsonPropertyName("pdfTitle")]
    public string PdfTitle { get; set; }

    [JsonPropertyName("eventUnit")]
    public EventUnit EventUnit { get; set; }

    [JsonPropertyName("h1")]
    public H1 H1 { get; set; }

    [JsonPropertyName("pdfType")]
    public string PdfType { get; set; }

    [JsonPropertyName("fullPath")]
    public string FullPath { get; set; }

    [JsonPropertyName("pdfDate")]
    public string PdfDate { get; set; }

    [JsonPropertyName("pdfTime")]
    public string PdfTime { get; set; }

    [JsonPropertyName("pdfSubcode")]
    public string PdfSubcode { get; set; }
}

public class Phase
{
    [JsonPropertyName("event")]
    public Event Event { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("shortDescription")]
    public string ShortDescription { get; set; }

    [JsonPropertyName("longDescription")]
    public string LongDescription { get; set; }

    [JsonPropertyName("order")]
    public string Order { get; set; }
}

public class Schedule
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("endDate")]
    public DateTime EndDate { get; set; }

    [JsonPropertyName("medal")]
    public int Medal { get; set; }
}
