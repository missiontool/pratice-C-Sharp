namespace EmployeeApi
{
    public class Employee
    {
        // 為了模擬資料庫，我們通常會需要一個 ID 來識別每一筆資料
        public int Id { get; set; }
        public string Name { get; set; }
        public int BaseSalary { get; set; }
        public int Bonus { get; set; }

        // 只有 get 代表是唯讀屬性 (計算欄位)
        // 在 API 傳輸時，這個通常不會讓前端傳過來，而是我們算給他
        public int TotalSalary => BaseSalary + Bonus;
    }
}