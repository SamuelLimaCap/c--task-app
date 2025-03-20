using System.ComponentModel.DataAnnotations.Schema;
using TaskStatus = TaskManagerApp.Data.TaskStatus;

namespace TaskManagerApp.dto;

public class TaskStatusDTO
{
    [Column("status")]
    public string Status { get; set; }
}