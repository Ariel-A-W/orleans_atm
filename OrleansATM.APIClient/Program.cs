using Orleans.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleansClient((context, client) =>
{
    client.UseAzureStorageClustering(configureOptions: options =>
    {
        options.TableServiceClient = new Azure.Data.Tables.TableServiceClient("UseDevelopmentStorage=true;");
    });

    client.Configure<ClusterOptions>(options =>
    {
        options.ClusterId = "ATMCluster"; 
        options.ServiceId = "ATMService";
    });   
}); 

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
