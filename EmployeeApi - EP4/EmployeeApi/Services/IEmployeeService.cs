using EmployeeApi;

namespace EmployeeApi.Services
{
    public interface IEmployeeService
    {
        // 1. 取得所有員工
        Task<List<Employee>> GetAll();

        // 2. 新增員工
        Task<Employee> AddEmployee(Employee employee);

        // 3. 查詢單一員工
        Task<Employee?> GetEmployee(int id);

        // 4. 更新員工資料
        Task<bool> UpdateEmployee(int id, Employee employee);

        // 5. 刪除員工資料
        Task<bool> DeleteEmployee(int id);

    }
}
