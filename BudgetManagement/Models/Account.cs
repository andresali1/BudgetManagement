using BudgetManagement.Validations;
using System.ComponentModel.DataAnnotations;

namespace BudgetManagement.Models
{
    public class Account
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50)]
        [FirstCapitalLetter]
        public string A_Name { get; set; }

        [Display(Name = "Tipo Cuenta")]
        public int AccountTypeId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int Balance { get; set; }

        [Display(Name = "Descripción")]
        [StringLength(maximumLength: 1000)]
        public string A_Description { get; set; }

        public string AccountType { get; set; }
    }
}
