namespace BudgetManagement.Models
{
    public class MonthlyGetResult
    {
        public int Month { get; set; }
        public DateTime ReferenceDate { get; set; }
        public int Price { get; set; }
        public int Income { get; set; }
        public int Expense { get; set; }
        public int OperationTypeId { get; set; }
    }
}
