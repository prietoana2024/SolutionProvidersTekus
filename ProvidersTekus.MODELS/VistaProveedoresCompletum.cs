using System;
using System.Collections.Generic;

namespace ProvidersTekus.MODELS;

public partial class VistaProveedoresCompletum
{
    public int ProveedorId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public int CampoId { get; set; }

    public string NombreCampo { get; set; } = null!;

    public string Etiqueta { get; set; } = null!;

    public string TipoDato { get; set; } = null!;

    public string? Valor { get; set; }
}
