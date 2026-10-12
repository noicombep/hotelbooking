using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class RoomType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
