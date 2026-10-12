using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Booking
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime CheckIn { get; set; }

    public DateTime CheckOut { get; set; }

    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = null!;

    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; }

    public string PaymentStatus { get; set; } = null!;

    public int RoomId { get; set; }

    public virtual Room Room { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
