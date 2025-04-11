using System.Runtime.Serialization;

namespace OrleansATM.APIClient.Contracts;

[DataContract]
public record Ingersar
{
    [DataMember]
    public decimal Monto { get; init; }
}
