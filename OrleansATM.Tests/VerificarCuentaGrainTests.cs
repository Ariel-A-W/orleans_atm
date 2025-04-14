using Moq;
using Orleans.Runtime;
using OrleansATM.Grains.Grains;
using OrleansATM.Grains.State;
using Xunit;
using Assert = Xunit.Assert;

namespace OrleansATM.Tests;

public class VerificarCuentaGrainTests
{
    private readonly Mock<IPersistentState<SaldoState>> _mockSaldoState;
    private readonly Mock<IPersistentState<VerificarCuentaState>> _mockCuentaState;

    private readonly VerificarCuentaGrain _grain;

    public VerificarCuentaGrainTests()
    {
        _mockSaldoState = new Mock<IPersistentState<SaldoState>>();
        _mockCuentaState = new Mock<IPersistentState<VerificarCuentaState>>();

        // Configuración inicial de los states
        _mockSaldoState.SetupGet(x => x.State).Returns(new SaldoState());
        _mockCuentaState.SetupGet(x => x.State).Returns(new VerificarCuentaState());

        _grain = new VerificarCuentaGrain(_mockSaldoState.Object, _mockCuentaState.Object);
    }

    [Fact]
    public async Task Ingresar_DeberiaSumarSaldo_CuandoMontoEsValido()
    {
        // Arrange
        _mockSaldoState.Object.State.Saldo = 100;

        // Act
        var resultado = await _grain.Ingresar(50);

        // Assert
        Assert.Equal(1, resultado);
        Assert.Equal(150, _mockSaldoState.Object.State.Saldo);
        _mockSaldoState.Verify(x => x.WriteStateAsync(), Times.Once);
    }

    [Fact]
    public async Task Ingresar_DeberiaFallar_CuandoMontoEsNegativo()
    {
        // Act
        var resultado = await _grain.Ingresar(-10);

        // Assert
        Assert.Equal(0, resultado);
        _mockSaldoState.Verify(x => x.WriteStateAsync(), Times.Never);
    }

    [Fact]
    public async Task Extraer_DeberiaRestarSaldo_CuandoMontoValido()
    {
        // Arrange
        _mockSaldoState.Object.State.Saldo = 100;

        // Act
        var resultado = await _grain.Extraer(50);

        // Assert
        Assert.Equal(1, resultado);
        Assert.Equal(50, _mockSaldoState.Object.State.Saldo);
        _mockSaldoState.Verify(x => x.WriteStateAsync(), Times.Once);
    }

    [Fact]
    public async Task Extraer_DeberiaFallar_CuandoSaldoInsuficiente()
    {
        // Arrange
        _mockSaldoState.Object.State.Saldo = 20;

        // Act
        var resultado = await _grain.Extraer(50);

        // Assert
        Assert.Equal(0, resultado);
        _mockSaldoState.Verify(x => x.WriteStateAsync(), Times.Never);
    }

    [Fact]
    public async Task GetSaldo_DeberiaRetornarSaldoActual()
    {
        // Arrange
        _mockSaldoState.Object.State.Saldo = 75;

        // Act
        var saldo = await _grain.GetSaldo();

        // Assert
        Assert.Equal(75, saldo);
    }
}
