using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Room
{
    public int Id { get; set; }

    public string RoomNumber { get; set; } = null!;

    public int Capacity { get; set; }

    public decimal Price { get; set; }

    public string Status { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? RoomTypeId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<RoomImage> RoomImages { get; set; } = new List<RoomImage>();

    public virtual RoomType? RoomType { get; set; }
}
