//讀取檔案： 
//不准用 File.ReadAllLines (因為假設檔案有 10G，會把記憶體撐爆)。請去查如何用 StreamReader 一行一行讀 (Line by Line)。

//資料清洗 (Data Cleaning)：
//金額不是數字的 -> 記錄到 error.log 檔案中 (例如：Eric 資料異常: 金額錯誤)。
//電話是空的 -> 記錄到 error.log。
//正常的資料 -> 存入 List<UserBill>。

//複雜邏輯 (GroupBy)：
//電信公司想知道「多少人的帳單超過 1000 元」以及「這些人的總金額是多少」。
//請用 LINQ 算出來並印在螢幕上。

//輸出結果：
//將清洗後「完全正確」的資料，轉成 JSON 格式 (你需要去查 System.Text.Json 怎麼用)，寫入 processed_data.json。

using System.IO.Enumeration;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using The_Dirty_Work;

internal class Program
{
    private static async Task Main(string[] args)
    {
        List<UserBill> userBills = new List<UserBill>();

        // 讀取檔案(印出測試)
        string path = @"C:\Users\fmx06\Desktop\C#\The Dirty Work\data.csv";
        try
        {
            using (StreamReader sr = new StreamReader(path))
            {
                // 標題不用處理可以先讀掉
                sr.ReadLine();

                while (sr.Peek() >= 0)
                {
                    string member = sr.ReadLine();

                    // 資料切割
                    string[] words = member.Split(',');

                    // 確保資料為3欄
                    if (words.Length != 3)
                    {
                        Console.WriteLine($"格式錯誤(欄位不足):{member}");
                        continue;
                    }

                    // 先把資料取出來，另外存到變數
                    string name = words[0];
                    string phone = words[1];
                    string billAmount = words[2];

                    // 驗證電話是否為空
                    if (string.IsNullOrWhiteSpace(phone))
                    {
                        Console.WriteLine($"[錯誤] {name} 的電話是空的，跳過此筆");
                        continue;
                    }

                    // 驗證金額是否為數字
                    if (!int.TryParse(billAmount, out int finalAmount))
                    {
                        Console.WriteLine($"[錯誤] {name} 的金額不是數字，跳過此筆");
                        continue;
                    }

                    // 資料正確沒有問題
                    userBills.Add(new UserBill
                    {
                        User = name,
                        Phone = phone,
                        BillAmount = finalAmount
                    });
                }

                // LINQ篩選
                // 「多少人的帳單超過 1000 元」並轉成 List 方便後續操作
                var overBill = userBills.Where(b => b.BillAmount > 1000).ToList();

                // 「這些人的總金額是多少」與「人數」
                int totalAmount = overBill.Sum(x => x.BillAmount);
                int count = overBill.Count();

                // 結果
                Console.WriteLine($"共有{count}位超過1000的客戶，總金額為{totalAmount}");

                StringBuilder sbBuilder = new StringBuilder();
                foreach (UserBill userBill in overBill)
                {
                    sbBuilder.AppendLine($"客戶 {userBill.User} | 金額 {userBill.BillAmount}");
                }

                Console.WriteLine(sbBuilder.ToString());

                // 輸出結果
                // 1.json格式
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                // 2.序列化
                string jsonString = JsonSerializer.Serialize(userBills, options);

                // 3.輸出位置
                string folderPath = Path.GetDirectoryName(path);
                string outputPath = Path.Combine(folderPath, "processed_data.json");

                // 4.非同步寫入
                Console.WriteLine("正在匯出json報表");
                await File.WriteAllTextAsync(outputPath, jsonString);
                Console.WriteLine($"匯出完成!檔案位置:{outputPath}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"錯誤：{e.Message}");
        }
    }
}