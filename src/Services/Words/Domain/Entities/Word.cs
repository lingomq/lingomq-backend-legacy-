namespace Words.Domain.Entities;

public class Word : EntityBase
{
    public string? Value { get; set; }
    public string? TranslatedValue { get; set; }
    public string? Transcription { get; set; }
    public string? Description { get; set; }
    public WordType? WordType { get; set; }
    public Language? Language { get; set; }
}
