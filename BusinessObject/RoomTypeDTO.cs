using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class RoomTypeDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        // 🔥 thêm để hiển thị admin
        public int RoomCount { get; set; }
    }
}
