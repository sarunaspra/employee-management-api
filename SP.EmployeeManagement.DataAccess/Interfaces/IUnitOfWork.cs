namespace SP.EmployeeManagement.DataAccess.Interfaces
{
    public interface IUnitOfWork
    {
        IEmployeeRepository EmployeeRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        IPositionRepository PositionRepository { get; }

        Task Commit();
    }
}
