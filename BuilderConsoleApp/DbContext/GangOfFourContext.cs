using BuilderConsoleApp.BuilderPattern;

using Microsoft.EntityFrameworkCore;

using System.Diagnostics;

namespace BuilderConsoleApp.Context
{
    public class GangOfFourContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Benefit> Benefits { get; set; }

        public GangOfFourContext(DbContextOptions<GangOfFourContext> options)
            : base(options)
        {
            Debug.WriteLine($"New context created with id #{ContextId}");
        }
    }
}
