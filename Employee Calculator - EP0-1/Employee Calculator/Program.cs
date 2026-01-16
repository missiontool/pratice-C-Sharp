using Employee_Calculator;
using System.Text;

Console.WriteLine("員工薪資輸入系統\r");
Console.WriteLine("--------------\n");

// 關閉判斷
bool endApp = false;
// 存放員工資料集合
List<Employee> employees = new List<Employee>();

while (!endApp)
{
    // 輸入員工姓名
    Console.Write("輸入員工姓名,並按下Enter：");
    string name = Console.ReadLine();

    // 輸入底薪
    Console.Write("輸入員工底薪,並按下Enter：");
    string baseSalary = Console.ReadLine();

    if (!int.TryParse(baseSalary, out int salaryResult))
    {
        Console.WriteLine("錯誤：薪資必須是數字！請重新輸入這名員工的資料。");
        continue;
    }

    // 輸入獎薪
    Console.Write("輸入員工獎薪,並按下Enter：");
    string bonus = Console.ReadLine();

    if (!int.TryParse(bonus, out int bonusResult))
    {
        Console.WriteLine("錯誤：薪資必須是數字！請重新輸入這名員工的資料。");
        continue;
    }

    // 新增一筆list資料
    employees.Add(new Employee
    {
        Name = name,
        BaseSalary = salaryResult,
        Bonus = bonusResult
    });
    Console.WriteLine("新增成功");

    Console.WriteLine("--------------\n");
    Console.WriteLine("輸入exit即可退出，按下任意鍵即可繼續");
    if (Console.ReadLine() == "exit") endApp = true;
}

Console.WriteLine("以下為薪資超過50000員工");
Console.WriteLine("--------------\n");

// 使用LINQ篩選
var highEarners = employees.Where(e => e.TotalSalary > 50000);

// 使用StringBuilder串接
StringBuilder sbBuilder = new StringBuilder();
foreach (Employee item in highEarners)
{
    sbBuilder.AppendLine($"姓名：{item.Name}, 總薪資：{item.TotalSalary}");
}

Console.WriteLine(sbBuilder.ToString());