using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
  public class BookingDTO
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string RoomName { get; set; } = null!;
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = null!;
    public string? Note { get; set; }
    public string PaymentStatus { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
}
