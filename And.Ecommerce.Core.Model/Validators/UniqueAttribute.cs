using And.Ecommerce.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace And.ECommerce.Core.Model.Validators
{
    public class UniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            using (AndDB db = new AndDB())
            {
                var email = value as string;
                if (db.Users.Any(u => u.Email == email))
                {
                    return new ValidationResult("This email has been taken.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
