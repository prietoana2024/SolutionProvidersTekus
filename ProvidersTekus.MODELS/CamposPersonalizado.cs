using System;
using System.Collections.Generic;

namespace ProvidersTekus.MODELS;

public partial class CamposPersonalizado
{
    public int Id { get; set; }

    public string NombreCampo { get; set; } = null!;

    public string Etiqueta { get; set; } = null!;

    public string TipoDato { get; set; } = null!;

    public bool Requerido { get; set; }

    public int Orden { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual ICollection<ProveedorCamposValore> ProveedorCamposValores { get; set; } = new List<ProveedorCamposValore>();
}
