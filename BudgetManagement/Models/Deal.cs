using System.ComponentModel.DataAnnotations;

namespace BudgetManagement.Models
{
    public class Deal
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Display(Name = "Fecha Transacción")]
        [DataType(DataType.Date)]
        public DateTime DealDate { get; set; } = DateTime.Today;

        public int Price { get; set; }

        [Display(Name = "Cuenta")]
        [Range(1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una {0}")]
        public int AccountId { get; set; }

        [Display(Name = "Categoría")]
        [Range(1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una {0}")]
        public int CategoryId { get; set; }

        [Display(Name = "Nota")]
        [StringLength(maximumLength: 1000, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        public string Note { get; set; }
    }
}
