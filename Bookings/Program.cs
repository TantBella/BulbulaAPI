using Bookings.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder()
    .ConfigureFunctionsWebApplication() 
    .ConfigureServices(services =>
    {
        services.AddSingleton<BookingService>();
    });

var host = builder.Build();
host.Run();
