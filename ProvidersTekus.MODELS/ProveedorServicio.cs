using System;
using System.Collections.Generic;

namespace ProvidersTekus.MODELS;

public partial class ProveedorServicio
{
    public int Id { get; set; }

    public int ProveedorId { get; set; }

    public int ServicioId { get; set; }

    public virtual Proveedore Proveedor { get; set; } = null!;

    public virtual Servicio Servicio { get; set; } = null!;
}
