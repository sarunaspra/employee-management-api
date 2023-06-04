using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SP.EmployeeManagement.DataAccess.Entities;

namespace SP.EmployeeManagement.DataAccess.Configs
{
    public class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable(nameof(Employee));

            builder.Property(x => x.FirstName).IsRequired();
            builder.Property(x => x.LastName).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.Salary).IsRequired();
            builder.Property(x => x.HireDate).IsRequired();

            builder.Property(b => b.Salary).HasPrecision(6, 2);
        }
    }
}
