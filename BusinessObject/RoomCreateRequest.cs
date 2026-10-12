using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BusinessObject
{

    public class RoomCreateRequest
    {
        public string RoomNumber { get; set; } = null!;

        public int Capacity { get; set; }

        public decimal Price { get; set; }

        public string Status { get; set; } = null!;

        public string? Description { get; set; }

        public int? RoomTypeId { get; set; }

        // 🔥 Upload nhiều ảnh
        public List<IFormFile>? Images { get; set; }
    }

}
