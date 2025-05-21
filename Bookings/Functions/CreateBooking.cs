using Bookings.Models;
using Bookings.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Bookings.Functions
{
    public class CreateBooking
    {
        private readonly BookingService _bookingService;
        private readonly ILogger<CreateBooking> _logger;

        // Konstruktor för dependency injection av BookingService och Logger
        public CreateBooking(BookingService bookingService, ILogger<CreateBooking> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        // Azure Function som aktiveras vid HTTP POST-anrop mot "api/bookings"
        [Function("CreateBooking")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "bookings")] HttpRequestData req)
        {
            _logger.LogInformation("Skapar ny bokning");

            // Läser in JSON-data från request body och deserialiserar till Booking-objekt
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var booking = JsonSerializer.Deserialize<Booking>(body);

            // Om deserialiseringen misslyckas, returnera 400 Bad Request
            if (booking is null)
            {
                _logger.LogError("Ogiltig bokning");
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badResponse.WriteAsJsonAsync(new { message = "Ogiltig bokning." });
                return badResponse;
            }

            // Anropar service för att spara bokningen i databasen
            await _bookingService.CreateAsync(booking);
            _logger.LogInformation($"Bokning skapad för {booking.GuestName}");

            // Skapar och returnerar svar med status 201 Created och den sparade bokningen i JSON
            var response = req.CreateResponse(HttpStatusCode.Created);
            await response.WriteAsJsonAsync(booking);
            return response;
        }
    }
}
