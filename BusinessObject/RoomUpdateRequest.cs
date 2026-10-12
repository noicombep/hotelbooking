using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace BusinessObject
{
    public class RoomUpdateRequest
    {
        public int Id { get; set; }                 // Id của phòng cần update
        public string RoomNumber { get; set; }      // Số phòng
        public int TypeId { get; set; }             // Loại phòng
        public int Capacity { get; set; }           // Sức chứa
        public decimal Price { get; set; }          // Giá
        public string Status { get; set; }          // Available / Occupied
        public string Description { get; set; }     // Mô tả

        // List file ảnh mới upload
        public List<IFormFile> Images { get; set; }
    }
}