using Microsoft.EntityFrameworkCore;
using SP.EmployeeManagement.DataAccess.Configs;
using SP.EmployeeManagement.DataAccess.Entities;
using System.Reflection;

namespace SP.EmployeeManagement.DataAccess
{
    public class EmployeeManagementContext : DbContext
    {
        public EmployeeManagementContext(DbContextOptions options) : base(options) 
        {
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Position> Positions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmployeeConfig());
            modelBuilder.ApplyConfiguration(new PositionConfig());
            modelBuilder.ApplyConfiguration(new DepartmentConfig());
        }
    }
}
