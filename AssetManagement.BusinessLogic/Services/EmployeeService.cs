using AssetManagement.Core.Entities;
using AssetManagement.DataAccess.Repositories;

namespace AssetManagement.BusinessLogic.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IGenericRepository<Employee> _employeeRepository;

        public EmployeeService(IGenericRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _employeeRepository.GetAllAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _employeeRepository.GetByIdAsync(id);
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            await _employeeRepository.AddAsync(employee);
            await _employeeRepository.SaveChangesAsync();
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            _employeeRepository.Update(employee);
            await _employeeRepository.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee != null)
            {
                _employeeRepository.Delete(employee);
                await _employeeRepository.SaveChangesAsync();
            }
        }
    }
}