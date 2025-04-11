namespace OrleansATM.Grains.Abstractions;

public interface IVerificarCuentaGrain : IGrainWithGuidKey
{
    Task Inicializar(decimal abrirSaldo); 

    Task<decimal> GetSaldo();

    Task<int> Ingresar(decimal monto);

    Task<int> Extraer(decimal monto);
}
