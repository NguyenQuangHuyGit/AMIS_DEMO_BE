using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public class OnlyNumber : ValidationAttribute
    {
        /// <summary>
        /// Kiểm tra xem trường chỉ chứa chữ số hay không
        /// </summary>
        /// <param name="value">Giá trị của trường</param>
        /// <param name="validationContext">ValidationContext</param>
        /// <returns>ValidationResult tương ứng</returns>
        /// CreatedBy: QuangHuy (03/03/2024)
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var errorMessage = "Error number field";
            if (ErrorMessageResourceType != null)
            {
                var resourceManager = new System.Resources.ResourceManager(ErrorMessageResourceType);
                if(ErrorMessageResourceName != null)
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
            if (value is string stringValue)
            {
                if (stringValue.All(char.IsDigit))
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult(errorMessage, memberNames);
            }
            return new ValidationResult(errorMessage, memberNames);
        }
    }
}
