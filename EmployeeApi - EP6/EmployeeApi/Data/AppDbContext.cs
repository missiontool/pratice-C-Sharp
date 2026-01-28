using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.Data
{
    // 繼承DbContext就能使用已經寫好的功能
    // 連線資料庫、SQL指令、追蹤資料變化等等
    public class AppDbContext : DbContext
    {
        // DI注入的核心
        // options裡面會放資料庫連線字串
        // 只負責操作資料庫，所以做一個建構子，讓Program.cs可以設定進來
        // :base(options)用來將收到的options轉交給上一層的DbContext去處理
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // 告訴EF CORE這張表的格式要與Employee一致
        public DbSet<Employee> Employees { get; set; }
    }
}
