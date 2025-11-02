using System;
using System.Collections.Generic;

namespace ProvidersTekus.MODELS;

public partial class ProveedorCamposValore
{
    public int Id { get; set; }

    public int ProveedorId { get; set; }

    public int CampoPersonalizadoId { get; set; }

    public string? Valor { get; set; }

    public virtual CamposPersonalizado CampoPersonalizado { get; set; } = null!;

    public virtual Proveedore Proveedor { get; set; } = null!;
}
