using EmployeeApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Controllers
{
    // [Route] 定義了這支 API 的網址路徑
    // [controller] 會自動換成你的類別名稱 (去掉 Controller)，所以網址是 /api/Employee
    [Route("api/[controller]")]
    [ApiController] // 這個標籤告訴系統：啟用自動 Model Binding 和驗證功能！
    public class EmployeeController : ControllerBase
    {
        // --- 模擬資料庫區 (In-Memory Database) ---
        // 使用 static 讓資料在記憶體中長存，不會因為 Request 結束就消失
        private static List<Employee> _employees = new List<Employee>
        {
            new Employee { Id = 1, Name = "Mike", BaseSalary = 50000, Bonus = 5000 },
            new Employee { Id = 2, Name = "Judy", BaseSalary = 60000, Bonus = 10000 }
        };

        // --- API 接口區 ---

        // 1. 取得所有員工
        // GET: api/Employee
        [HttpGet]
        public List<Employee> GetAll()
        {
            return _employees;
        }

        // 2. 新增員工 (體驗 Model Binding 的威力)
        // POST: api/Employee
        [HttpPost]
        public IActionResult Add([FromBody] CreateEmployeeDto employeeDto)
        {
            // [FromBody] 告訴系統：去讀 Request Body 裡的 JSON，把它變成 newEmp 物件

            //// 簡單的邏輯：給他一個新的 ID (最大 ID + 1)
            //int newId = _employees.Count > 0 ? _employees.Max(e => e.Id) + 1 : 1;
            //newEmp.Id = newId;

            //// 存入清單
            //_employees.Add(newEmp);

            // Mapping 把資料從DTO寫過去給Employee過程
            var newEmployee = new Employee
            {
                Name = employeeDto.Name,
                BaseSalary = employeeDto.BaseSalary,
                Bonus = employeeDto.Bonus,

                Id = _employees.Count > 0 ? _employees.Max(e => e.Id) + 1 : 1
            };

            _employees.Add(newEmployee);

            // 回傳 HTTP 200 OK，並附上成功訊息
            return Ok($"新增成功！員工 ID 為：{newEmployee.Id}, 姓名：{newEmployee.Name}");
        }

        // 任務開始
        // 3. 查詢單一員工
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _employees.FirstOrDefault(e => e.Id == id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // 4. 更新員工資料
        [HttpPut("{id}")]
        public IActionResult Edit(int id, [FromBody] UpdateEmployeeDto employeeDto)
        {

            var existEmp = _employees.FirstOrDefault(e => e.Id == id);
            if (existEmp == null)
            {
                return NotFound($"找不到 ID 為 {id} 的員工"); // 404
            }

            existEmp.Name = employeeDto.Name;
            existEmp.BaseSalary = employeeDto.BaseSalary;
            existEmp.Bonus = employeeDto.Bonus;

            return NoContent(); // 204請求成功
        }

        // 5. 刪除員工資料
        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {

            var result = _employees.FirstOrDefault(e => e.Id == id);
            if (result == null)
            {
                return NotFound($"找不到 ID 為 {id} 的員工");
            }
            _employees.Remove(result);

            return NoContent();
        }
    }
}