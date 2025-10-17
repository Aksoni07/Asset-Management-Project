using System.Linq.Expressions;

namespace AssetManagement.DataAccess.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id); // Task<T> ---> return type.    <T>: return an obj of type : T = Employee
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity); 
        void Delete(T entity);
        Task<int> SaveChangesAsync(); // to execute all changes till now in db : Update/ Delete ---> No. of rows affected
    }
}

// Defining abilities of any repository: must be able to do CRUD : Clean way to access data
// Any Table can do these things : CRUD
// Creating Intermediate Interface: Hide Underlying complexities of DbContext