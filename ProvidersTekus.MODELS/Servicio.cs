using System;
using System.Collections.Generic;

namespace ProvidersTekus.MODELS;

public partial class Servicio
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal ValorHora { get; set; }

    public string Paises { get; set; } = null!;

    public virtual ICollection<ProveedorServicio> ProveedorServicios { get; set; } = new List<ProveedorServicio>();
}
