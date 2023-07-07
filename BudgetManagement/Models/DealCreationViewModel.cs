using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetManagement.Models
{
    public class DealCreationViewModel : Deal
    {
        public IEnumerable<SelectListItem> Accounts { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
