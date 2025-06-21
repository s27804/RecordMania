using Microsoft.EntityFrameworkCore;
using RecordMania.DbContexts;
using RecordMania.DTOs;
using RecordMania.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<RecordManiaDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/records", (RecordManiaDbContext db) =>
{
    var records = db.Record.ToList();
    var recordList = new List<RecordDTO>();
    foreach (var record in records)
    {
        var recordLanguage = db.Language.FirstOrDefault(l => l.Id == record.LanguageId);
        if(recordLanguage == null)
            return Results.NotFound();
        var recordTask = db.Task.FirstOrDefault(t => t.Id == record.TaskId);
        if(recordTask == null)
            return Results.NotFound();
        var recordStudent = db.Student.FirstOrDefault(s => s.Id == record.StudentId);
        if(recordStudent == null)
            return Results.NotFound();
        
        recordList.Add(new RecordDTO()
        {
            Id = record.Id,
            Language = recordLanguage,
            Task = recordTask,
            Student = recordStudent,
            ExecutionTime = record.ExecutionTime,
            CreatedAt = record.CreatedAt
        });
    }
    
    return Results.Ok(recordList.OrderByDescending(r => r.ExecutionTime));
});

app.MapPost("/api/records", (RecordInsertDTO dto, RecordManiaDbContext db) =>
{
    var language = db.Language.FirstOrDefault(l => l.Id == dto.LanguageId);
    if(language == null)
        return Results.NotFound("Language not found");
    var student = db.Student.FirstOrDefault(s => s.Id == dto.StudentId);
    if (student == null)
        return Results.NotFound("Student not found");
    var task = db.Task.FirstOrDefault(t => t.Id == dto.Task.Id);
    if (task == null)
    {
        if (dto.Task.Name == null || dto.Task.Description == null)
        {
            return Results.NotFound("Task not found");
        }
        
        var newTask = new RecordMania.Models.Task
        { 
            Name = dto.Task.Name,
            Description = dto.Task.Description
        };
        
        db.Task.Add(newTask);
        var insertTask = db.Task.OrderByDescending(t => t.Id).FirstOrDefault();
        if(insertTask == null)
            return Results.NotFound();
        
        var newRecord1 = new Record
        {
            LanguageId = language.Id,
            TaskId = insertTask.Id,
            StudentId = student.Id,
            ExecutionTime = dto.ExecutionTime,
            CreatedAt = dto.CreatedAt
        };
        db.Record.Add(newRecord1);
        db.SaveChanges();
    
        return Results.Ok("Created a new record");
    }

    var newRecord2 = new Record
    {
        LanguageId = language.Id,
        TaskId = task.Id,
        StudentId = student.Id,
        ExecutionTime = dto.ExecutionTime,
        CreatedAt = dto.CreatedAt
    };
    db.Record.Add(newRecord2);
    db.SaveChanges();
    
    return Results.Ok("Created a new record");
});

app.Run();