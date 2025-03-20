using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using TaskManagerApp.generator;
using Task = TaskManagerApp.Data.database.Task;

namespace TaskManagerApp.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Task> Task { get; set; }

}