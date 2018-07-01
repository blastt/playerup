using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Market.Web.Attributes
{
    public class UserName : ValidationAttribute
    {
        Regex regex = new Regex("^[0-9A-Za-z_]+$");
        public override bool IsValid(object value)
        {
            return regex.IsMatch(value.ToString());
        }
    }
    public class UserNameValidation
    {
        

        public static ValidationResult ValidateUserName(string value)
        {
            Regex regex = new Regex("^[0-9A-Za-z_]+$");
            bool isValid = regex.IsMatch(value); ;

            // Perform validation logic here and set isValid to true or false.

            if (isValid)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(
                    "The address for this customer does not match the required criteria.");
            }
        }
    }
}