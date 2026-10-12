using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class AiLog
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Prompt { get; set; }

    public string? Response { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User? User { get; set; }
}
