using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SP.EmployeeManagement.DataAccess.Interfaces;
using SP.EmployeeManagement.DataAccess.Repositories;

namespace SP.EmployeeManagement.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContextOptions _options;
        private EmployeeManagementContext _context;

        private EmployeeRepository _employeeRepository;
        private DepartmentRepository _departmentRepository;
        private PositionRepository _positionRepository;

        private EmployeeManagementContext Context => _context ??= new EmployeeManagementContext(_options);

        public UnitOfWork(IOptions<UnitOfWorkOptions> accessor) : this(accessor.Value)
        {
        }

        public UnitOfWork(UnitOfWorkOptions options)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlServer(options.ConnectionString, x => x.CommandTimeout(options.CommandTimeout));
            _options = optionsBuilder.Options;
        }

        public IEmployeeRepository EmployeeRepository => 
            _employeeRepository ??= new EmployeeRepository(Context);
        public IDepartmentRepository DepartmentRepository =>
            _departmentRepository ??= new DepartmentRepository(Context);
        public IPositionRepository PositionRepository =>
            _positionRepository ??= new PositionRepository(Context);

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Dispose()
        {
            await _context.DisposeAsync();
        }
    }
}
