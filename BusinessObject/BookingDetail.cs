using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class BookingDetail
{
    public int Id { get; set; }

    public int BookingId { get; set; }

    public int RoomId { get; set; }

    public decimal Price { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;
}
