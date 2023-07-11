namespace BudgetManagement.Models
{
    public class TransactionByUserParameter
    {
        public int UserId { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
