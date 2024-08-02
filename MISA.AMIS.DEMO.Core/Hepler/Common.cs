using Microsoft.Extensions.Localization;
using MISA.AMIS.DEMO.Core.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MISA.AMIS.DEMO.Core
{
    /// <summary>
    /// Lớp chứa các hàm bổ trợ
    /// </summary>
    public class Common
    {
        /// <summary>
        /// Hàm chuyển ENum sang chuỗi tương ứng
        /// </summary>
        /// <param name="code">Mã ngôn ngữ</param>
        /// <returns>Chuỗi tương ứng</returns>
        /// CreatedBy: QuangHuy (11/03/2024)
        public static string? ConvertEnumLangToString(LangCode code)
        {
            return code switch
            {
                LangCode.VN => "vi-VN",
                LangCode.US => "en-US",
                _ => "vi-VN",
            };
        }

        /// <summary>
        /// Hàm kiểm tra và format lại Trường dạng ngày tháng năm
        /// </summary>
        /// <param name="date">Chuỗi thời gian</param>
        /// <returns>Datetime: nếu hợp lệ || Null: Nếu không hợp lệ</returns>
        /// CreatedBy: QuangHuy (16/01/2024)
        public static DateTime? ValidateDateTime(string? date)
        {
            if (date == null)
            {
                return null;
            }
            var dateFormat = FormatDateTimeToEnum(date);
            switch (dateFormat)
            {
                case DateFormat.ddMMyyyy:
                    var listSub = date.Split(new char[] { '-', '/' });
                    var day = Int32.Parse(listSub[0]);
                    var month = Int32.Parse(listSub[1]);
                    var year = Int32.Parse(listSub[2]);
                    return new DateTime(year, month, day);
                case DateFormat.MMyyyy:
                    listSub = date.Split(new char[] { '-', '/' });
                    month = Int32.Parse(listSub[0]);
                    year = Int32.Parse(listSub[1]);
                    return new DateTime(year, month, 1);
                case DateFormat.yyyy:
                    listSub = date.Split(new char[] { '-', '/' });
                    year = Int32.Parse(listSub[0]);
                    return new DateTime(year, 1, 1);
                default:
                    if (DateTime.TryParse(date, out var dateTime))
                    {
                        day = dateTime.Day;
                        month = dateTime.Month;
                        year = dateTime.Year;
                        // Tạo ngày tháng ngowcj với mặc định để đúng với định dạng trên Excel
                        return new DateTime(year, day, month);
                    }
                    return null;
            }
        }

        /// <summary>
        /// So sách 2 giá chuỗi đã loại bỏ dấu và chứ hoa
        /// </summary>
        /// <param name="source">Giá trị so sánh</param>
        /// <param name="value">Giá trị so sánh</param>
        /// <returns>True: bằng nhau || False: không bằng nhau</returns>
        /// CreatedBy: QuangHuy (02/03/2024)
        public static bool CompareRawString(string source, string value)
        {
            var valueReduce = RemoveSpecialCharacters(RemoveVietnameseAccentsToLowerCase(value.Trim()));
            var sourceReduce = RemoveSpecialCharacters(RemoveVietnameseAccentsToLowerCase(source.Trim()));
            if (valueReduce.Equals(sourceReduce))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Hàm chuyển chuỗi ngày tháng chuyền vào về dạng Enum tương ứng
        /// </summary>
        /// <param name="date">Chuỗi ngày tháng</param>
        /// <returns>Dạng Enum ngày tháng tương ứng</returns>
        /// CreatedBy: QuangHuy (24/01/2024)
        public static DateFormat? FormatDateTimeToEnum(string date)
        {
            Regex regex;
            // "dd-mm-yyyy" và "dd/mm/yyyy"
            var pattern1 = @"^(0?[1-9]|[12][0-9]|3[01])[-/](0?[1-9]|1[0-2])[-/]\d{4}$";
            regex = new Regex(pattern1);
            if (regex.IsMatch(date)) 
            {
                return DateFormat.ddMMyyyy;
            }

            // "mm-yyyy" và "mm/yyyy"
            var pattern2 = @"^(0?[1-9]|1[0-2])[-/]\d{4}$";
            regex = new Regex(pattern2);
            if (regex.IsMatch(date))
            {
                return DateFormat.MMyyyy;
            }

            // "yyyy"
            var pattern3 = @"^[12]\d{3}$";
            regex = new Regex(pattern3);
            if (regex.IsMatch(date))
            {
                return DateFormat.yyyy;
            }
            return null;
        }


        /// <summary>
        /// Hàm chuyển giá trị số sang Alphabet tương ứng
        /// </summary>
        /// <param name="number"></param>
        /// <returns>Chữ cái tương ứng</returns>
        /// CreatedBy: QuangHuy (24/01/2024)
        public static string ConvertToLetter(int number)
        {
            int dividend = number;
            string alphabet = string.Empty;

            while (dividend > 0)
            {
                int modulo = (dividend - 1) % 26;
                alphabet = Convert.ToChar(65 + modulo) + alphabet;
                dividend = (dividend - modulo) / 26;
            }

            return alphabet;
        }

        /// <summary>
        /// Hàm chuyển từ Gender Enum sang chuỗi tương ứng
        /// </summary>
        /// <param name="gender">Enum</param>
        /// <returns>Chuỗi tương ứng</returns>
        /// CreatedBy: QuangHuy (24/01/2024)
        public static string? ConvertEnumGenderToString(Gender? gender)
        {
            if(gender == null)
            {
                return null;
            }
            return gender switch
            {
                Gender.MALE => MISAResources.Enum_Male,
                Gender.FEMALE => MISAResources.Enum_Female,
                Gender.OTHER => MISAResources.Enum_Other,
                _ => MISAResources.Enum_Other,
            };
        }

        /// <summary>
        /// Hàm chuyển từ Datetime sang chuỗi format theo dd/mm/yyyy
        /// </summary>
        /// <param name="date">Giá trị ngày tháng</param>
        /// <returns>Chuỗi ngày tháng: dd/mm/yyy</returns>
        /// CreatedBy: QuangHuy (24/01/2024)
        public static string? ConvertDateTimeToString(DateTime? date)
        {
            if(date == null)
            {
                return null;
            }
            object day = date.Value.Day;
            if((int)day < 10)
            {
                day = $"0{day}";
            }
            object month = date.Value.Month;
            if ((int)month < 10)
            {
                month = $"0{month}";
            }
            var year = date.Value.Year;
            return $"{day}/{month}/{year}";
        }

        /// <summary>
        /// Hàm bỏ dấu tiếng việt và chuyển tất cả về chữ thường
        /// </summary>
        /// <param name="text">Chuỗi cần thao tác</param>
        /// <returns>Chuỗi đã xử lý</returns>
        /// CreatedBy: QuangHuy (16/01/2024)
        public static string RemoveVietnameseAccentsToLowerCase(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            string normalizedText = text.Normalize(NormalizationForm.FormD);

            StringBuilder sb = new StringBuilder();
            foreach (char c in normalizedText)
            {
                if (char.IsLetterOrDigit(c))
                {
                    sb.Append(char.ToLowerInvariant(c));
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');

            return sb.ToString();
        }

        /// <summary>
        /// Hàm loại bỏ ký tự đặc biệt trong 1 chuỗi
        /// </summary>
        /// <param name="text">Chuỗi cần được xử lý</param>
        /// <returns>Giá trị của chuỗi sau khi xử lý</returns>
        /// CreatedBy: QuangHuy (16/01/2024)
        public static string RemoveSpecialCharacters(string text)
        {
            return Regex.Replace(text, "[^\\w\\s]", "");
        }

        /// <summary>
        /// Hàm chuyển đối từ chuỗi giới tính sang Enum
        /// </summary>
        /// <param name="gender">Chuỗi giới tính</param>
        /// <returns>Enum Gender tương ứng</returns>
        /// CreatedBy: QuangHuy (16/01/2024)
        public static Gender? ConvertEnumGender(string? gender)
        {
            if (gender == null)
            {
                return null;
            }
            gender = RemoveVietnameseAccentsToLowerCase(gender);
            if (gender.Equals(RemoveVietnameseAccentsToLowerCase(MISAResources.Enum_Male)))
            {
                return Gender.MALE;
            }
            else if (gender.Equals(RemoveVietnameseAccentsToLowerCase(MISAResources.Enum_Female)))
            {
                return Gender.FEMALE;
            }
            else if (gender.Equals(RemoveVietnameseAccentsToLowerCase(MISAResources.Enum_Other)))
            {
                return Gender.OTHER;
            }
            else
            {
                return ConvertEnumGenderFromOtherResource(gender);
            }
        }
        
        /// <summary>
        /// Hàm chuyển đổi từ chuỗi giới tính sanh Enum Gender tương ứng trong trường hợp ngôn ngữ khác trên hệ thống đang sử dụng
        /// </summary>
        /// <param name="gender">Chuỗi gender</param>
        /// <returns>Giá trị Enum tương ứng</returns>
        /// CreatedBy: QuangHuy (11/03/2024)
        public static Gender? ConvertEnumGenderFromOtherResource(string gender)
        {
            var currentCulture = CultureInfo.CurrentCulture.ToString();
            foreach(var culture in Variable.supportCulture)
            {
                if(currentCulture.Equals(culture.ToString()))
                {
                    continue;
                }
                else
                {
                    var resourceManager = new System.Resources.ResourceManager(typeof(MISAResources));
                    if (gender.Equals(RemoveVietnameseAccentsToLowerCase(resourceManager.GetString("Enum_Male", culture) ?? "")))
                    {
                        return Gender.MALE;
                    }
                    else if (gender.Equals(RemoveVietnameseAccentsToLowerCase(resourceManager.GetString("Enum_Female", culture) ?? "")))
                    {
                        return Gender.FEMALE;
                    }
                    else if (gender.Equals(RemoveVietnameseAccentsToLowerCase(resourceManager.GetString("Enum_Other", culture) ?? "")))
                    {
                        return Gender.OTHER;
                    }
                    else
                    {
                        return Gender.OTHER;
                    }
                }
            }
            return Gender.OTHER;
        }
    }
}
