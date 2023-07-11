namespace BudgetManagement.Models
{
    public class MonthlyReportViewModel
    {
        public IEnumerable<MonthlyGetResult> DealsByMonth { get; set; }
        public int Income => DealsByMonth.Sum(x => x.Income);
        public int Expense => DealsByMonth.Sum(x => x.Expense);
        public int Total => Income - Expense;
        public int Year { get; set; }
    }
}
