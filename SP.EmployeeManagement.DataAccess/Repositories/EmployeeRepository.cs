using SP.EmployeeManagement.DataAccess.Entities;
using SP.EmployeeManagement.DataAccess.Interfaces;

namespace SP.EmployeeManagement.DataAccess.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(EmployeeManagementContext context) : base(context)
        {
        }
    }
}
