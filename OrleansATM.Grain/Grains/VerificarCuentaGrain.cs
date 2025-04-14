using Orleans.Runtime;
using OrleansATM.Grains.Abstractions;
using OrleansATM.Grains.State;

namespace OrleansATM.Grains.Grains;

public class VerificarCuentaGrain : Grain, IVerificarCuentaGrain
{
    private readonly IPersistentState<SaldoState> _saldoState; 
    private readonly IPersistentState<VerificarCuentaState> _verificarCuentaState;

    public VerificarCuentaGrain(
        [PersistentState("saldo", "tableStorage")] IPersistentState<SaldoState> saldoState,
        [PersistentState("verificarCuenta", "blobStorage")] IPersistentState<VerificarCuentaState> verificarCuentaState)
    {
        _saldoState = saldoState;
        _verificarCuentaState = verificarCuentaState;
    }

    public async Task<int> Extraer(decimal monto)
    {
        var saldoActual = _saldoState.State.Saldo;
        if ((monto > 0 && saldoActual > 0) && (saldoActual > monto))
        {               
            var saldoNuevo = saldoActual - monto;
            _saldoState.State.Saldo = saldoNuevo;
            await _saldoState.WriteStateAsync();
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public async Task<int> Ingresar(decimal monto)
    {
        if (monto > 0)
        {
            var saldoActual = _saldoState.State.Saldo;
            var saldoNuevo = saldoActual + monto;
            _saldoState.State.Saldo = saldoNuevo;
            await _saldoState.WriteStateAsync();
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public async Task<decimal> GetSaldo()
    {
        await Task.CompletedTask;
        return _saldoState.State.Saldo;
    }

    public async Task Inicializar(decimal abrirSaldo)
    {
        try
        {
            _verificarCuentaState.State.AperturaTiempo = DateTime.UtcNow;
            _verificarCuentaState.State.CuentaTipo = "Default";
            _verificarCuentaState.State.CuentaId = this.GetGrainId().GetGuidKey();
            _saldoState.State.Saldo = abrirSaldo;
            await _saldoState.WriteStateAsync();
            await _verificarCuentaState.WriteStateAsync();
        }
        catch
        (Exception ex)
        {
            throw new Exception("Error al inicializar la cuenta.", ex);
        }
    }
}
