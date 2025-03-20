using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data;

namespace TaskManagerApp.ViewComponents;

[ViewComponent(Name = "KanbanCard")]
public class KanbanCardViewComponent(AppDbContext appDbContext) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(int? id = null)
    {
        if (id is null)
            return View();

        return await InvokeById((int)id);
    }

    public async Task<IViewComponentResult> InvokeById(int id)
    {
        var task = appDbContext.Task.FromSql($"SELECT * FROM task where id = {id}").ToList();

        ViewBag.task = task[0];

        return View();
    }
}