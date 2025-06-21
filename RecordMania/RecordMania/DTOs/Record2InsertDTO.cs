namespace RecordMania.DTOs;

public class Record2InsertDTO
{
    public int LanguageId { get; set; }
    public int StudentId { get; set; }
    public TaskDTO Task { get; set; }
    public Int64 ExecutionTime { get; set; }
    public DateTime CreatedAt { get; set; }
}