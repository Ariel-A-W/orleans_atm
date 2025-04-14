using Orleans.TestingHost;
using OrleansATM.Grains.Abstractions;
using Xunit;
using Assert = Xunit.Assert;

namespace OrleansATM.Tests.Silos;

public class VerificarCuentaSiloTests : IAsyncLifetime
{
    private TestCluster _cluster;

    public async Task InitializeAsync()
    {
        var builder = new TestClusterBuilder(1);

        builder.AddSiloBuilderConfigurator<SiloConfigurator>(); 
        builder.AddClientBuilderConfigurator<ClientConfigurator>();

        _cluster = builder.Build();
        await _cluster.DeployAsync();
    }


    public async Task DisposeAsync()
    {
        await _cluster.StopAllSilosAsync();
    }

    [Fact]
    public async Task CrearCuenta_DeberiaInicializarSaldoCorrectamente()
    {
        // Arrange
        var grainId = Guid.NewGuid();
        var grain = _cluster.GrainFactory.GetGrain<IVerificarCuentaGrain>(grainId);
        await grain.Inicializar(500);

        // Act
        var saldo = await grain.GetSaldo();

        // Assert
        Assert.Equal(500, saldo);
    }

    [Fact]
    public async Task IngresarYExtraer_DeberiaActualizarSaldo()
    {
        // Arrange
        var grainId = Guid.NewGuid();
        var grain = _cluster.GrainFactory.GetGrain<IVerificarCuentaGrain>(grainId);
        await grain.Inicializar(100);

        // Act
        var r1 = await grain.Ingresar(50);
        var r2 = await grain.Extraer(30);
        var saldo = await grain.GetSaldo();

        // Assert
        Assert.Equal(1, r1);
        Assert.Equal(1, r2);
        Assert.Equal(120, saldo);
    }
}
