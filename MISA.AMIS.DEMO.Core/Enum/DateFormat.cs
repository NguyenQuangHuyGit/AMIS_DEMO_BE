using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    /// <summary>
    /// Các dạng chuỗi ngày tháng
    /// </summary>
    public enum DateFormat
    {
        /// <summary>
        /// dd-MM-yyyy và dd/MM/yyyy
        /// </summary>
        ddMMyyyy = 0,

        /// <summary>
        /// MM-yyyy và MM/yyyy
        /// </summary>
        MMyyyy = 1,

        /// <summary>
        /// yyyy
        /// </summary>
        yyyy = 2
    }
}
