using SP.EmployeeManagement.DataAccess.Entities;
using SP.EmployeeManagement.DataAccess.Interfaces;

namespace SP.EmployeeManagement.DataAccess.Repositories
{
    public class PositionRepository : Repository<Position>, IPositionRepository
    {
        public PositionRepository(EmployeeManagementContext context) : base(context)
        {
        }
    }
}
