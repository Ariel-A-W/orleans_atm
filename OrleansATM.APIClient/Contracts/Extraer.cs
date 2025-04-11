using System.Runtime.Serialization;

namespace OrleansATM.APIClient.Contracts;

[DataContract]
public record Extraer
{
    [DataMember]
    public decimal Monto { get; init; }
}
