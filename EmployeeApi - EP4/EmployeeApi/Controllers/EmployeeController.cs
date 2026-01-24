using EmployeeApi.Data;
using EmployeeApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeApi.Services; // 引用Service

namespace EmployeeApi.Controllers
{
    // [Route] 定義了這支 API 的網址路徑
    // [controller] 會自動換成你的類別名稱 (去掉 Controller)，所以網址是 /api/Employee
    [Route("api/[controller]")]
    [ApiController] // 這個標籤告訴系統：啟用自動 Model Binding 和驗證功能！
    public class EmployeeController : ControllerBase
    {
        // 改為正式資料庫
        // 1. 宣告一個私有變數來存資料庫連線
        // readonly代表一旦建構子被設定後，就不能再被亂改，增加安全性
        // private readonly AppDbContext _context;
        // 改成依賴Service
        // Controller不用知道Service是誰，只要認得介面的規範就好
        private readonly IEmployeeService _employeeService;

        // 2. 建立建構子 目的是為了強制Controller一定要在有資料庫連線的狀態下才能作用
        // 系統會自動把準備好的context傳進來
        // public EmployeeController(AppDbContext context) { 
        //    _context = context;
        // }
        // 系統會去Program.cs查，註冊的service把它new出來
        public EmployeeController(IEmployeeService employeeService) {
            _employeeService = employeeService;
        }

        // --- API 接口區 ---

        // 1. 取得所有員工
        // GET: api/Employee
        [HttpGet]
        public async Task<List<Employee>> GetAll()
        {
            // 透過EF Core去資料庫下SQL指令
            // SELECT * FROM Employees
            return await _employeeService.GetAll();
        }

        // 2. 新增員工 (體驗 Model Binding 的威力)
        // POST: api/Employee
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateEmployeeDto employeeDto)
        {
            // DTO轉Entity過程
            var newEmployee = new Employee
            {
                Name = employeeDto.Name,
                BaseSalary = employeeDto.BaseSalary,
                Bonus = employeeDto.Bonus,
            };

            // 呼叫service執行新增
            var result = await _employeeService.AddEmployee(newEmployee);

            // 回傳 HTTP 200 OK，並附上成功訊息
            return Ok($"新增成功！員工 ID 為：{newEmployee.Id}, 姓名：{newEmployee.Name}");
        }

        // 3. 查詢單一員工
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            // 直接呼叫Service裡面的方法去呼叫執行
            var result = await _employeeService.GetEmployee(id);

            // service回傳null或是有資料的object
            // 再藉由HTTP回應(404或200)
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // 4. 更新員工資料
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] UpdateEmployeeDto employeeDto)
        {

            var updateEmp = new Employee
            {
                Name = employeeDto.Name,
                BaseSalary = employeeDto.BaseSalary,
                Bonus = employeeDto.Bonus
            };

            bool isSuccess = await _employeeService.UpdateEmployee(id, updateEmp);

            if (!isSuccess) return NotFound();

            return NoContent(); // 204請求成功
        }

        // 5. 刪除員工資料
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {

            bool isSuccess = await _employeeService.DeleteEmployee(id);

            if (!isSuccess) return NotFound();

            return NoContent();
        }
    }
}