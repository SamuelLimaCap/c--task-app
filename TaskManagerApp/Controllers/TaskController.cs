using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TaskManagerApp.Data;
using TaskManagerApp.dto;
using TaskManagerApp.Services;
using TaskStatus = TaskManagerApp.Data.TaskStatus;

namespace TaskManagerApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TaskController(AppDbContext context, IRenderComponent renderComponent) : ControllerBase
{
    [Route("{id}")]
    [HttpGet]
    public async Task<IActionResult> GetTaskHTMLTemplate(int id)
    {
        var tasks = context.Task.FromSql($"SELECT * FROM task WHERE id = {id}").ToList();

        if (tasks.Count == 1)
        {
            var html = renderComponent.ToStringAsync("KanbanCard", id);
            return Ok(html);
        }

        return NoContent();
    }

    [Route("status/{id}")]
    [HttpPut]
    public async Task<ActionResult> ChangeStatus(int id, TaskStatusDTO statusDto)
    {
        var statuses = Enum.GetNames(typeof(TaskStatus)).ToList().Find(p => p == statusDto.Status);
        if (string.IsNullOrWhiteSpace(statuses))
            return BadRequest("Status doesn't exist");

        TaskStatus statusGivenByUser;
        Enum.TryParse(statuses, out statusGivenByUser);
        var rowsAffected =
            await context.Database.ExecuteSqlAsync(
                $"UPDATE task SET status_id = {(int)statusGivenByUser} WHERE id = {id}");


        if (rowsAffected == 1)
            return NoContent();

        return BadRequest("Cannot change status of this card");
    }

    [Route("{id}")]
    [HttpDelete]
    public async Task<ActionResult> DeleteTask(int id)
    {
        var rowsAffected = await context.Database.ExecuteSqlAsync(
            $"UPDATE task SET is_deleted = true WHERE id = {id}");

        if (rowsAffected == 1)
            return Ok();

        return BadRequest("Id not found");
    }

    [HttpPost]
    public async Task<ActionResult> CreateTask(TaskDTO taskDto)
    {
        var statuses = Enum.GetNames(typeof(TaskStatus)).ToList().Find(p => p == taskDto.Status);
        if (string.IsNullOrWhiteSpace(statuses))
            return BadRequest("Status doesn't exist");

        TaskStatus statusGivenByUser;
        Enum.TryParse(statuses, out statusGivenByUser);

        var task = context.Task.FromSqlRaw(
            "INSERT INTO task (title, content, status_id) VALUES (@p0, @p1, @p2) RETURNING *", taskDto.Title, taskDto.Description,
            (int)statusGivenByUser
        ).ToList().First();

        return CreatedAtAction(nameof(CreateTask), task);
    }
}