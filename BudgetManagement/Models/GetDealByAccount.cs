namespace BudgetManagement.Models
{
    public class GetDealByAccount
    {
        public int UserId { get; set; }
        public int AccountId { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
