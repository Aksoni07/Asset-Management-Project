using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AssetManagement.DataAccess.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context; //  Once the repository is created, its connection to the database session cannot be changed.

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context; // hold our DbContext instance : To Interact with DB , readonly : only constructor can assigne it value
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}


// Implementation of IgenericRepository Rule : Methods