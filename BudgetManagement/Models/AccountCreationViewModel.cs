using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetManagement.Models
{
    public class AccountCreationViewModel : Account
    {
        public IEnumerable<SelectListItem> AccountTypes { get; set; }
    }
}
