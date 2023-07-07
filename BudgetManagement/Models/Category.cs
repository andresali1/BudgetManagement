using System.ComponentModel.DataAnnotations;

namespace BudgetManagement.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50, ErrorMessage = "El campo {0} no puede ser mayor a {1} caracteres")]
        public string C_Name { get; set; }

        [Display(Name = "Tipo Operación")]
        public int OperationTypeId { get; set; }

        public int UserId { get; set; }
    }
}
