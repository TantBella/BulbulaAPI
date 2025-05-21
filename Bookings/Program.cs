using Bookings.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Skapar och konfigurerar en standardv�rd f�r Azure Functions
var builder = Host.CreateDefaultBuilder()

    // Konfigurerar Azure Functions-specifik middleware (t.ex. routing)
    .ConfigureFunctionsWebApplication()

    // L�gger till beroenden i DI-container (dependency injection)
    .ConfigureServices(services =>
    {
        // Registrerar BookingService som en singleton � dvs. en instans anv�nds under hela applikationens livstid
        services.AddSingleton<BookingService>();
    });

// Bygger v�rden (host)
var host = builder.Build();

// Startar Azure Functions-applikationen
host.Run();
