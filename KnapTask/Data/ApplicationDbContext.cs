using Microsoft.EntityFrameworkCore;
using KnapTask.Models;


namespace KnapTask.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<TaskItem> TaskItems { get; set; }

    }
}
