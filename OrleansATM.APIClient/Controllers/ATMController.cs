using Microsoft.AspNetCore.Mvc;
using OrleansATM.APIClient.Contracts;
using OrleansATM.Grains.Abstractions;

namespace OrleansATM.APIClient.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ATMController : ControllerBase
{
    [HttpPost("verificarcuenta")]
    public async Task<IActionResult> VerificarCuenta(
        CrearCuenta crearCuenta,
        IClusterClient clusterClient
    )
    {
        var verificarCuentatId = Guid.NewGuid();
        var verificarCuentaGrain = clusterClient.GetGrain<IVerificarCuentaGrain>(verificarCuentatId);
        await verificarCuentaGrain.Inicializar(crearCuenta.AbrirSaldo);
        return Created($"verificarcuenta: {verificarCuentatId}", verificarCuentatId);
    }


    [HttpGet("verificarcuenta/saldo/{verificarCuentatId}")]
    public async Task<IActionResult> GetSaldo(
        Guid verificarCuentaId,
        IClusterClient clusterClient
    )
    {
        var verificarCuentaGrain = clusterClient.GetGrain<IVerificarCuentaGrain>(verificarCuentaId);
        var saldo = await verificarCuentaGrain.GetSaldo();
        return Ok(saldo);
    }

    [HttpPost("verificarcuenta/ingresar/{verificarCuentatId}")]
    public async Task<IActionResult> GetIngresar(
        Guid verificarCuentaId, 
        Ingresar ingresar,
        IClusterClient clusterClient
    ) 
    { 
        var verificarCuentaGrain = clusterClient.GetGrain<IVerificarCuentaGrain>(verificarCuentaId);
        var estado = await verificarCuentaGrain.Ingresar(ingresar.Monto);
        if (estado == 0)
        {
            return BadRequest("El valor no es correcto.");
        }
        else
        {
            return Created("Ingreso exitoso.", 1);
        }
    }

    [HttpPost("verificarcuenta/extraer/{verificarCuentatId}")]
    public async Task<IActionResult> GetExtraer(
        Guid verificarCuentaId,
        Extraer extraer,
        IClusterClient clusterClient
    )
    {
        var verificarCuentaGrain = clusterClient.GetGrain<IVerificarCuentaGrain>(verificarCuentaId);
        var estado = await verificarCuentaGrain.Extraer(extraer.Monto); 
        if (estado == 0)
        {
            return BadRequest("No hay Saldo suficiente.");
        }
        else 
        {
            return Created("Egreso exitoso.", 1);
        }
    }
}
