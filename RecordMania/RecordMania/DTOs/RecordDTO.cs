using RecordMania.Models;
using Task = RecordMania.Models.Task;

namespace RecordMania.DTOs;

public class RecordDTO
{
    public int Id { get; set; }
    public Language Language { get; set; }
    public Task Task { get; set; }
    public Student Student { get; set; }
    public Int64 ExecutionTime { get; set; }
    public DateTime CreatedAt { get; set; }
}