using EmployeeApi.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.Services
{
    public class EmployeeService:IEmployeeService
    {
        private readonly AppDbContext _context;

        // DI注入 Service也需要DbContext才能工作
        public EmployeeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAll()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee; // 此時 ID 已經產生了
            // 因為 employee 是「參考型別」，它一直指向同一個記憶體位置。
            // 既然剛剛 EF Core 已經偷改了那個位置的 Id 值，
            // 所以這裡回傳出去時，它已經是： { Id: 101, Name: "Mike", ... }
        }

        public async Task<Employee?> GetEmployee(int id)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> UpdateEmployee(int id, Employee employee)
        {
            var existEmp = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);

            // 只回傳false或true，再讓Controller去決定要怎麼回應給使用者HTTP
            if (existEmp == null) return false; // 找不到人，更新失敗

            // 更新邏輯
            existEmp.Name = employee.Name;
            existEmp.BaseSalary = employee.BaseSalary;
            existEmp.Bonus = employee.Bonus;

            await _context.SaveChangesAsync();
            return true; // 更新成功
        }

        public async Task<bool> DeleteEmployee(int id)
        {
            var result = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (result == null) return false; // 找不到人，刪除失敗

            _context.Employees.Remove(result);
            await _context.SaveChangesAsync();
            return true; // 刪除成功
        }
    }
}
