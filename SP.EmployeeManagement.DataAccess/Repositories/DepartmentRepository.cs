using SP.EmployeeManagement.DataAccess.Entities;
using SP.EmployeeManagement.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.EmployeeManagement.DataAccess.Repositories
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(EmployeeManagementContext context) : base(context)
        {
        }
    }
}
