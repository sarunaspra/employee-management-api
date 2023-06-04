using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SP.EmployeeManagement.DataAccess.Entities;

namespace SP.EmployeeManagement.DataAccess.Configs
{
    public class DepartmentConfig : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable(nameof(Department));

            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(100);
        }
    }
}
