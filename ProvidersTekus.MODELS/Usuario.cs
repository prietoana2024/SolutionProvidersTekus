using System;
using System.Collections.Generic;

namespace ProvidersTekus.MODELS;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Pwd { get; set; } = null!;
}
