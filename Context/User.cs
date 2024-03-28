using System;
using System.Collections.Generic;

namespace mygallery.Context;

public partial class User
{
    public int UserId { get; set; }

    public string LoginName { get; set; } = null!;

    public string? LoginPassword { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public bool IsActive { get; set; }
}
