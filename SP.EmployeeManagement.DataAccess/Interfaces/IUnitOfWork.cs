namespace SP.EmployeeManagement.DataAccess.Interfaces
{
    public interface IUnitOfWork
    {
        IEmployeeRepository EmployeeRepository { get; }

        Task Commit();
    }
}
