using System.ComponentModel.DataAnnotations;

namespace BudgetManagement.Models
{
    public class Deal
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Display(Name = "Fecha Transacción")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public DateTime DealDate { get; set; } = DateTime.Today;

        [Display(Name = "Monto")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int Price { get; set; }

        [Display(Name = "Cuenta")]
        [Range(1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una {0}")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int AccountId { get; set; }

        [Display(Name = "Categoría")]
        [Range(1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una {0}")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int CategoryId { get; set; }

        [Display(Name = "Nota")]
        [StringLength(maximumLength: 1000, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Note { get; set; }

        [Display(Name = "Tipo Operación")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int OperationTypeId { get; set; }
    }
}
