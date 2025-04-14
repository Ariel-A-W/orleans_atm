using Microsoft.Extensions.Logging;
using Orleans.TestingHost;

namespace OrleansATM.Tests.Silos;

public class SiloConfigurator : ISiloConfigurator
{
    public void Configure(ISiloBuilder siloBuilder)
    {
        siloBuilder
            .AddMemoryGrainStorage("tableStorage")
            .AddMemoryGrainStorage("blobStorage")
            .ConfigureLogging(logging => logging.AddConsole());
    }
}
