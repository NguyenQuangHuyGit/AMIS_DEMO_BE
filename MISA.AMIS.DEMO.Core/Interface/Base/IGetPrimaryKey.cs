using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public interface IGetPrimaryKey
    {
        /// <summary>
        /// Hàm lấy tên PrimaryKey của một đối tượng class cụ thể
        /// </summary>
        /// <returns>Tên của primary key</returns>
        /// CreatedBy: QuangHuy (08/01/2024)
        public string? GetKeyName();
    }
}
