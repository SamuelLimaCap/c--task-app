using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskManagerApp.Data.database;

[Keyless]
public class Task
{
    [Column("id")]
    public int? Id { get; set; }
    [Column("title")]
    public string Name { get; set; }
    [Column("content")]
    public string Content { get; set; }
    [Column("creation_date")]
    public DateOnly CreationDate { get; set; }
    [Column("last_modification_date")]
    public DateTime LastContentModificationDate { get; set; }
    [Column("is_deleted")]
    public bool IsDeleted { get; set; } = false;
    
    [Column("status_id")]
    public TaskStatus Status { get; set; } = TaskStatus.Todo;
}