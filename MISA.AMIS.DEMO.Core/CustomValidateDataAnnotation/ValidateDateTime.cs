using MISA.AMIS.DEMO.Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public class ValidateDateTime : ValidationAttribute
    {
        /// <summary>
        /// Hàm kiểm tra xem ngày tháng chọn có lớn hơn ngày tháng hiện tại không
        /// </summary>
        /// <param name="value">Giá trị ngày tháng truyền vào</param>
        /// <param name="validationContext">ValidationContext</param>
        /// <returns>Trả về lỗi nếu có</returns>
        /// CreatedBy: QuangHuy (24/01/2024)
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var errorMessage = "Error number field";
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
            // Nếu không truyền thì trả về true
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is DateTime dateValue)
            {
                // Lấy ngày tháng hiện tại
                DateTime currentDate = DateTime.Now;

                // Kiểm tra nếu ngày được cung cấp nhỏ hơn ngày hiện tại
                if (dateValue < currentDate)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    // Nếu không, trả về thông báo lỗi
                    return new ValidationResult(errorMessage, memberNames);
                }
            }

            // Validate có phải là kiểu date time hay không
            return new ValidationResult(MISAResources.InValidMsg_DateType);
        }
    }
}
