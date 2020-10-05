using AzureFunctionSample;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Moosesoft.Azure.ServiceBus;
using System.Diagnostics.CodeAnalysis;

[assembly: FunctionsStartup(typeof(Startup))]
namespace AzureFunctionSample
{
    [ExcludeFromCodeCoverage]
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton(
                Builder.MessageContextProcessor
                    .Configure()
                    .WithMessageProcessor<SampleMessageProcessor>()
                    .WithCloneMessageFailurePolicy()
                    .WithExponentialBackOffDelayStrategy()
                    .Build());
        }
    }
}