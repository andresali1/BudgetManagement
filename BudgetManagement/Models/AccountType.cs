using BudgetManagement.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BudgetManagement.Models
{
    public class AccountType
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [FirstCapitalLetter]
        [Remote(action: "AccountTypeExistsValidation", controller: "AccountType")]
        public string AT_Name { get; set; }
        public int UserId { get; set; }
        public int AT_Order { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if(AT_Name != null  && AT_Name.Length > 0)
        //    {
        //        var firstLetter = AT_Name[0].ToString();

        //        if(firstLetter != firstLetter.ToUpper())
        //        {
        //            yield return new ValidationResult("La primera letra debe ser mayúscula");
        //        }
        //    }
        //}
    }
}
