using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BudgetManagement.Models
{
    public class OperationType
    {
        public int Id { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Remote(action: "OperationTypeExistsValidation", controller: "OperationType")]
        public string T_Description { get; set; }
    }
}
