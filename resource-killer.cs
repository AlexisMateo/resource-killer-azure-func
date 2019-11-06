using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;

namespace Company.Function
{
    public static class resource_killer
    {
        [FunctionName("resource_killer")]
        public static void Run([TimerTrigger("0 30 11 * * *")]TimerInfo myTimer, ILogger log)
        {
                var clientId = Environment.GetEnvironmentVariable("CLIENT_ID");
                var clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET");
                var tenantId = Environment.GetEnvironmentVariable("TENANT_ID");

                var credentials = SdkContext.AzureCredentialsFactory
                                .FromServicePrincipal(clientId,
                                clientSecret,
                                tenantId, 
                                AzureEnvironment.AzureGlobalCloud);
                var azure = Azure
                .Configure()
                .Authenticate(credentials)
                .WithDefaultSubscription();

                var resourceGroups = azure.ResourceGroups.ListByTag("deleteme", "true");

                log.LogInformation("Life is a death that comes ⚰️");
                
                foreach (var resourceGroup in resourceGroups)
                {
                    azure.ResourceGroups.DeleteByNameAsync(resourceGroup.Name);
                    log.LogInformation("death came for " + resourceGroup.Name+ "  ⚰️");

                }
        }
    }
}
