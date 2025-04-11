using Microsoft.Extensions.Hosting;
using Orleans.Configuration;

await Host.CreateDefaultBuilder(args)
    .UseOrleans(siloBuilder => 
    {
        siloBuilder.UseAzureStorageClustering(configureOptions: options =>
        {
            options.TableServiceClient = new Azure.Data.Tables.TableServiceClient("UseDevelopmentStorage=true;");
        });

        siloBuilder.Configure<ClusterOptions>(options =>
        {
            options.ClusterId = "ATMCluster";
            options.ServiceId = "ATMService";
        });

        siloBuilder.AddAzureTableGrainStorage("tableStorage", configureOptions: options =>
        {
            options.TableServiceClient = new Azure.Data.Tables.TableServiceClient("UseDevelopmentStorage=true;");
        });

        siloBuilder.AddAzureBlobGrainStorage("blobStorage", configureOptions: options =>
        {
            options.BlobServiceClient = new Azure.Storage.Blobs.BlobServiceClient("UseDevelopmentStorage=true;");
        });
    }).RunConsoleAsync();
