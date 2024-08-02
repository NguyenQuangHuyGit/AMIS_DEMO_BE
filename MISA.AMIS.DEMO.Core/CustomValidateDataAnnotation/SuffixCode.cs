using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public class SuffixCode : ValidationAttribute
    {
        private readonly string _regex;

        public SuffixCode(string regex)
        {
            _regex = regex;
        }

        /// <summary>
        /// Hàm kiểm tra mã nhân viên có hậu tố hợp lệ không
        /// </summary>
        /// <param name="value">Mã nhân viên</param>
        /// <param name="validationContext">ValidationContext</param>
        /// <returns>ValidationResult</returns>
        /// CreatedBy: QuangHuy (20/03/2024)
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var errorMessage = "Error suffix code";
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
                string? suffix = stringValue.Substring(3);
                var regex = new Regex(_regex);
                bool isValid = regex.IsMatch(suffix);
                if (isValid)
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult(errorMessage, memberNames);
            }
            return new ValidationResult(errorMessage, memberNames);
        }
    }
}
