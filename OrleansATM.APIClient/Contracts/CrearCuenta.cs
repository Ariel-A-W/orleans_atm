using System.Runtime.Serialization;

namespace OrleansATM.APIClient.Contracts;

[DataContract]
public record CrearCuenta
{
    [DataMember]
    public decimal AbrirSaldo { get; init; }
}
