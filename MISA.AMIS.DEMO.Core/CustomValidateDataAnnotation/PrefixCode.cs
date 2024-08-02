using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public class PrefixCode : ValidationAttribute
    {
        private readonly string _prefix;

        public PrefixCode(string prefix)
        {
            _prefix = prefix;
        }

        /// <summary>
        /// Hàm kiểm tra mã nhân viên có tiền tố hợp lệ không
        /// </summary>
        /// <param name="value">Mã nhân viên</param>
        /// <param name="validationContext">ValidationContext</param>
        /// <returns>ValidationResult</returns>
        /// CreatedBy: QuangHuy (20/03/2024)
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var errorMessage = "Error prefix code";
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
            if (value is string stringValue)
            {
                if (stringValue.StartsWith(_prefix))
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult(errorMessage, memberNames);
            }
            return new ValidationResult(errorMessage, memberNames);
        }
    }
}
