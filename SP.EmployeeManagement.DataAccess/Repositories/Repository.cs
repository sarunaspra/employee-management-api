using Microsoft.EntityFrameworkCore;
using SP.EmployeeManagement.DataAccess.Interfaces;

namespace SP.EmployeeManagement.DataAccess.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbSet<T> DbSet;

        protected Repository(EmployeeManagementContext context) => DbSet = context.Set<T>();

        public async Task Add(T entity)
        {
            await DbSet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }
        
        public async Task<IEnumerable<T>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Update(T entity)
        {
            DbSet.Update(entity);
        }
    }
}
