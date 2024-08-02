using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public class Email : ValidationAttribute
    {
        /// <summary>
        /// Hàm kiểm tra tính hợp lệ của một email
        /// </summary>
        /// <param name="value">Giá trị của trường email</param>
        /// <param name="validationContext">Đối tượng ValidationContext</param>
        /// <returns>ValidationResult</returns>
        /// CreatedBy: QuangHuy (20/03/2024)
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var errorMessage = "Error email field";
            if (ErrorMessageResourceType != null)
            {
                var resourceManager = new System.Resources.ResourceManager(ErrorMessageResourceType);
                if (ErrorMessageResourceName != null)
                {
                    errorMessage = resourceManager.GetString(ErrorMessageResourceName);
                }
            }
            if (value == null)
            {
                return ValidationResult.Success;
            }
            string[]? memberNames = validationContext.MemberName is { } memberName
                    ? new[] { memberName }
                    : null;
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.[\w]{2,3})+)$");
            if (value is string stringValue)
            {
                bool isValidEmail = regex.IsMatch(stringValue);
                if (isValidEmail || stringValue.Equals(""))
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult(errorMessage, memberNames);
            }
            return new ValidationResult(errorMessage, memberNames);
        }
    }
}
