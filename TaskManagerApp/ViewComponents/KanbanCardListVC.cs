using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data;
using TaskStatus = TaskManagerApp.Data.TaskStatus;

namespace TaskManagerApp.ViewComponents;

[ViewComponent(Name = "KanbanCardList")]
public class KanbanCardListVc(AppDbContext dbContext) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(TaskStatus status)
    {
        var tasks = dbContext.Task.FromSql($"""
                                            SELECT * FROM task 
                                            WHERE status_id = {(int) status}
                                            AND is_deleted = false;
                                            """
        ).ToList();
        
        Console.WriteLine($"{status}: {tasks.Count}");

        ViewBag.taskList = tasks;
        return View();
    }
}