namespace OrleansATM.Grains.State;

[GenerateSerializer]
public record VerificarCuentaState
{
    [Id(0)] 
    public Guid CuentaId { get; set; }

    [Id(1)]
    public DateTime AperturaTiempo { get; set; }

    [Id(2)]
    public string? CuentaTipo { get; set; }
}
