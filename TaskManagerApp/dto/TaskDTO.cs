using TaskStatus = TaskManagerApp.Data.TaskStatus;

namespace TaskManagerApp.dto;

public class TaskDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
}