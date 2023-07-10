namespace BudgetManagement.Models
{
    public class DealUpdateViewModel : DealCreationViewModel
    {
        public int PreviousAccountId { get; set; }
        public int PreviousPrice { get; set; }
        public string ReturnUrl { get; set; }
    }
}
