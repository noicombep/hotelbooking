using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }

        public int TypeId { get; set; }
        public string Type { get; set; }
        public int Capacity { get; set; }

        public decimal Price { get; set; }
        public string Status { get; set; }

        public string Description { get; set; }

        public List<string> Images { get; set; }
    }
}
