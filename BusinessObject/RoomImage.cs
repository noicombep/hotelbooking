using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class RoomImage
{
    public int Id { get; set; }

    public int RoomId { get; set; }

    public string? ImageUrl { get; set; }

    public virtual Room Room { get; set; } = null!;
}
