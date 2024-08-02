using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public interface IExcelService
    {
        /// <summary>
        /// Hàm đọc file Excel
        /// </summary>
        /// <typeparam name="T">Kiểu đối tượng trả về</typeparam>
        /// <param name="fileImport">File Excel cần đọc</param>
        /// <returns>Danh sách kiểu dữ liệu muốn trả về</returns>
        /// CreatedBy: QuangHuy (16/01/2024)
        public Task<IEnumerable<T>> ReadExcelFile<T>(IFormFile fileImport, Func<ExcelWorksheet, Task<List<T>>> mapFunc);

        /// <summary>
        /// Hàm ghi dữ liệu vào file Excel
        /// </summary>
        /// <typeparam name="T">Kiểu đối tượng: là class</typeparam>
        /// <param name="headers">Danh sách tiêu đề của các cột</param>
        /// <param name="title">Tiêu đề của WorkSheet (Có thể không truyền)</param>
        /// <param name="listDatas">Danh sách đối tượng muốn ghi</param>
        /// <param name="mapFunc">Hàm callback xử lý việc mapping dữ liệu</param>
        /// <returns>File excel dữ liệu</returns>
        /// CreatedBy: QuangHuy (22/01/2024)
        public byte[] WriteExcelFile<T>(List<string> headers, string? title, List<T>? listDatas, Action<ExcelWorksheet, T, int>? mapFunc);
    }
}
