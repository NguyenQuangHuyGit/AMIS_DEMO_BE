using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public class BaseEntity : IGetPrimaryKey
    {
        #region Properties

        /// Ngày tạo
        public DateTime? CreatedDate { get; set; }

        /// Người tạo
        public string? CreatedBy { get; set; }

        /// Ngày chỉnh sửa
        public DateTime? ModifiedDate { get; set; }

        /// Người chỉnh sửa
        public string? ModifiedBy { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Hàm lấy tên Property Key của Entity
        /// </summary>
        /// <returns>Tên của Key prop || NULL</returns>
        /// CreatedBy: QuangHuy (28/12/2023)
        public string? GetKeyName()
        {
            var properties = GetType().GetProperties();
            foreach (var property in properties)
            {
                if (Attribute.GetCustomAttribute(property, typeof(KeyAttribute)) is KeyAttribute)
                {
                    return property.Name;
                }
            }
            return null;
        } 
        #endregion
    }
}
