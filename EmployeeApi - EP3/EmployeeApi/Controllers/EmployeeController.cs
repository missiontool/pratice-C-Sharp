using EmployeeApi.Data;
using EmployeeApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        private readonly AppDbContext _context;

        // 2. 建立建構子 目的是為了強制Controller一定要在有資料庫連線的狀態下才能作用
        // 系統會自動把準備好的context傳進來
        public EmployeeController(AppDbContext context) { 
            _context = context;
        }

        // --- API 接口區 ---

        // 1. 取得所有員工
        // GET: api/Employee
        [HttpGet]
        public async Task<List<Employee>> GetAll()
        {
            // 原本
            // return _employees;

            // 透過EF Core去資料庫下SQL指令
            // SELECT * FROM Employees
            return await _context.Employees.ToListAsync();
        }

        // 2. 新增員工 (體驗 Model Binding 的威力)
        // POST: api/Employee
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateEmployeeDto employeeDto)
        {
            var newEmployee = new Employee
            {
                Name = employeeDto.Name,
                BaseSalary = employeeDto.BaseSalary,
                Bonus = employeeDto.Bonus,

                // 通常會在資料庫預設自動編號，所以不用寫
                // Id = _employees.Count > 0 ? _employees.Max(e => e.Id) + 1 : 1
            };

            // 加入待辦清單
            _context.Employees.Add(newEmployee);

            // 真的送出SQL指令
            // INSERT INTO
            _context.SaveChangesAsync();

            // 回傳 HTTP 200 OK，並附上成功訊息
            return Ok($"新增成功！員工 ID 為：{newEmployee.Id}, 姓名：{newEmployee.Name}");
        }

        // 任務開始
        // 3. 查詢單一員工
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            // 原本
            // var result = _employees.FirstOrDefault(e => e.Id == id);
            
            // 直接對資料庫搜尋
            var result = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);

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

            var existEmp = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (existEmp == null)
            {
                return NotFound($"找不到 ID 為 {id} 的員工"); // 404
            }

            existEmp.Name = employeeDto.Name;
            existEmp.BaseSalary = employeeDto.BaseSalary;
            existEmp.Bonus = employeeDto.Bonus;

            // 存檔送出後會自動比較有更新變動的欄位
            await _context.SaveChangesAsync();

            return NoContent(); // 204請求成功
        }

        // 5. 刪除員工資料
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {

            var result = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (result == null)
            {
                return NotFound($"找不到 ID 為 {id} 的員工");
            }

            _context.Employees.Remove(result);

            // 同樣存檔後會自動判斷執行 DELETE SQL
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}