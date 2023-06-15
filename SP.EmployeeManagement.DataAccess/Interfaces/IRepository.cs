namespace SP.EmployeeManagement.DataAccess.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
