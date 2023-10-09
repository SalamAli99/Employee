using Employee.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Employee.Data
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Employe> Employees{ get; set; }
    }
}
