using Bookings.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Skapar och konfigurerar en standardvärd för Azure Functions
var builder = Host.CreateDefaultBuilder()

    // Konfigurerar Azure Functions-specifik middleware (t.ex. routing)
    .ConfigureFunctionsWebApplication()

    // Lägger till beroenden i DI-container (dependency injection)
    .ConfigureServices(services =>
    {
        // Registrerar BookingService som en singleton – dvs. en instans används under hela applikationens livstid
        services.AddSingleton<BookingService>();
    });

// Bygger värden (host)
var host = builder.Build();

// Startar Azure Functions-applikationen
host.Run();
