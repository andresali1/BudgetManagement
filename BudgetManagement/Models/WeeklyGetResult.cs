namespace BudgetManagement.Models
{
    public class WeeklyGetResult
    {
        public int Week { get; set; }
        public int Price { get; set; }
        public int OperationTypeId { get; set; }
        public int Income { get; set; }
        public int Expense { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
