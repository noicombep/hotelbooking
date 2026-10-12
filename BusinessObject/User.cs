using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool? EmailConfirmed { get; set; }

    public string? VerifyToken { get; set; }

    public virtual ICollection<AiLog> AiLogs { get; set; } = new List<AiLog>();

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
