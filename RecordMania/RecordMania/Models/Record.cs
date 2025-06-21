namespace RecordMania.Models;

public class Record
{
    public int Id { get; set; }
    public int LanguageId { get; set; }
    public int TaskId { get; set; }
    public int StudentId { get; set; }
    public Int64 ExecutionTime { get; set; }
    public DateTime CreatedAt { get; set; }
}