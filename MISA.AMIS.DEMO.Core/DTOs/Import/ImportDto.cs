using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.DEMO.Core
{
    public class ImportDto<T>
    {
        // Danh sách đối tượng dùng Nhập khẩu
        public IEnumerable<T>? ListObject { get; set; }

        // Key dùng lưu vào Memory Cache
        public Guid ImportKey { get; set; }
    }
}
