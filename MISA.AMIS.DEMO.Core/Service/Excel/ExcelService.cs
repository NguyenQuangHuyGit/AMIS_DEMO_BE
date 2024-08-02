using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using MISA.AMIS.DEMO.Core.Resources;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Theme;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    /// <summary>
    /// Service đọc ghi file Excel
    /// </summary>
    public class ExcelService : IExcelService
    {
        #region Methods
        public async Task<IEnumerable<T>> ReadExcelFile<T>(IFormFile fileImport, Func<ExcelWorksheet, Task<List<T>>> mapFunc)
        {
            var results = new List<T>();
            using (var stream = new MemoryStream())
            {
                // Copy tệp và Stream
                await fileImport.CopyToAsync(stream);
                // Thực hiện đọc dữ liệu
                using (var package = new ExcelPackage(stream))
                {
                    // Lấy Worksheet đầu tiên
                    var worksheet = package.Workbook.Worksheets[0];
                    if (worksheet != null)
                    {
                        // Gọi callback trả về danh sách đối tượng tượng ứng
                        results = await mapFunc(worksheet);
                    }
                };
            }
            return results;
        }

        public byte[] WriteExcelFile<T>(List<string> headers, string? title, List<T>? listDatas, Action<ExcelWorksheet, T, int>? mapFunc)
        {
            using (var package = new ExcelPackage())
            {
                // Tạo mới 1 Worksheet
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("MISAExcel");

                // Tạo template tiêu đề cho file excel
                int row = InitTemplateFile(worksheet, headers, title);
                if (listDatas != null)
                {
                    for (var i = 0; i < listDatas.Count; i++)
                    {
                        // Đặt giá trị cho cột số thứ tự
                        worksheet.Cells[row + i + 1, 1].Value = i + 1;
                        // Gọi hàm map data
                        if (mapFunc != null)
                        {
                            mapFunc(worksheet, listDatas[i], row + i + 1);
                        }
                    }

                    using (var range = worksheet.Cells[row, 1, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column])
                    {
                        range.AutoFitColumns();
                    }

                    //worksheet.Column(worksheet.Dimension.End.Column).AutoFit();
                    worksheet.Column(worksheet.Dimension.End.Column).Style.WrapText = true;

                    // Căn giữa cột Số thứ tự
                    worksheet.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                else
                {
                    // Xóa cột STT nếu không có dữ liệu
                    worksheet.DeleteColumn(1);
                }

                // Xử lý dữ liệu trả về
                using var memoryStream = new MemoryStream(package.GetAsByteArray());
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Hàm tạo mới template tiếu đề cho file excel
        /// </summary>
        /// <param name="worksheet">Worksheet cần tạo</param>
        /// <param name="headers">Danh sách tiêu đề</param>
        /// <param name="title"></param>
        /// <returns>Số hàng của tiêu đề</returns>
        /// CreatedBy: QuangHuy (22/01/2024)
        private int InitTemplateFile(ExcelWorksheet worksheet, List<string> headers, string? title)
        {
            // Hàng bắt đầu đổ dữ liệu tiêu đề
            int startRow = 1;

            // Lấy Chữ cái tượng trưng cho sô cột
            var letter = Common.ConvertToLetter(headers.Count);

            // Đặt tiêu đề của Worksheet nếu có
            if (title != null && title != "")
            {
                startRow = 3;
                // Merge 2 dòng đầu
                var firstRowMerge = $"A1:{letter}1";
                worksheet.Cells[$"A1:{letter}1"].Merge = true;
                worksheet.Cells[$"A2:{letter}2"].Merge = true;

                // Set tiêu đề và các kiểu gia trị
                worksheet.Cells[firstRowMerge].Value = title.ToUpper();
                worksheet.Cells[firstRowMerge].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[firstRowMerge].Style.Font.Bold = true;
                worksheet.Cells[firstRowMerge].Style.Font.Size = 14;
            }

            // Đổ tiêu đề cột
            for (int i = 0; i < headers.Count; i++)
            {
                worksheet.Cells[startRow, i + 1].Value = headers[i];
                worksheet.Cells[startRow, i + 1].Style.Font.Bold = true;
                worksheet.Cells[startRow, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[startRow, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                worksheet.Column(i + 1).Width = worksheet.Column(i + 1).Width * 2;
            }
            // Căn chỉnh độ rộng, độ cao cột theo nội dung
            worksheet.Cells.AutoFitColumns();
            worksheet.Row(startRow).CustomHeight = true;
            worksheet.Row(startRow).Height = worksheet.Row(startRow).Height * 2;
            worksheet.Row(startRow).Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            using (var range = worksheet.Cells[startRow, 1, startRow, worksheet.Dimension.End.Column])
            {
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            }

            return startRow;
        }
        #endregion
    }
}
