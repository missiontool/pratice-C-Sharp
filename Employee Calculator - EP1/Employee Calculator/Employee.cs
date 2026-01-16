namespace Employee_Calculator
{
    public class Employee
    {
        public string Name { get; set; }
        public int BaseSalary { get; set; }
        public int Bonus { get; set; }

        public int TotalSalary
        {
            get { return BaseSalary + Bonus; }
        }
    }
}
