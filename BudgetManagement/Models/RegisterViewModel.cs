using System.ComponentModel.DataAnnotations;

namespace BudgetManagement.Models
{
    public class RegisterViewModel
    {
        [Display(Name = "Correo")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress(ErrorMessage = "El campo {0} debe ser un correo electrónico válido")]
        public string Email { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
