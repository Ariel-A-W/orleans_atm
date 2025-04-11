namespace OrleansATM.Grains.State;

[GenerateSerializer]
public record SaldoState
{
    [Id(0)]
    public decimal Saldo { get; set; }
}
