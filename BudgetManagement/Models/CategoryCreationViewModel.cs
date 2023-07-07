using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetManagement.Models
{
    public class CategoryCreationViewModel : Category
    {
        public IEnumerable<SelectListItem> OperationTypes { get; set; }
    }
}
