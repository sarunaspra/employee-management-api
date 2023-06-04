using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SP.EmployeeManagement.DataAccess.Entities;

namespace SP.EmployeeManagement.DataAccess.Configs
{
    public class PositionConfig : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.ToTable(nameof(Position));

            builder.Property(x => x.Title).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(100);
        }
    }
}
