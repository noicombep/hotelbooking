using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class EmailLog
{
    public int Id { get; set; }

    public string? ToEmail { get; set; }

    public string? Subject { get; set; }

    public string? Body { get; set; }

    public string? Status { get; set; }

    public DateTime CreatedAt { get; set; }
}
