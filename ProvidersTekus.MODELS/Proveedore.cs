using System;
using System.Collections.Generic;

namespace ProvidersTekus.MODELS;

public partial class Proveedore
{
    public int Id { get; set; }

    public string Nit { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public virtual ICollection<ProveedorCamposValore> ProveedorCamposValores { get; set; } = new List<ProveedorCamposValore>();

    public virtual ICollection<ProveedorServicio> ProveedorServicios { get; set; } = new List<ProveedorServicio>();
}
