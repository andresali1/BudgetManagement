namespace BudgetManagement.Models
{
    public class WeeklyReportViewModel
    {
        public int Income => DealsByWeek.Sum(x => x.Income);
        public int Expense => DealsByWeek.Sum(x => x.Expense);
        public int Total => Income - Expense;
        public DateTime ReferenceDate { get; set; }
        public IEnumerable<WeeklyGetResult> DealsByWeek { get; set; }
    }
}
