using System.Runtime.Serialization;

namespace OrleansATM.APIClient.Contracts;

[DataContract]
public record Ingresar
{
    [DataMember]
    public decimal Monto { get; init; }
}
