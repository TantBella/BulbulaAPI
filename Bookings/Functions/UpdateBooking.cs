using Bookings.Models;
using Bookings.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Bookings.Functions
{
    public class UpdateBooking
    {
        private readonly BookingService _bookingService;
        private readonly ILogger<UpdateBooking> _logger;

        // Konstruktor för dependency injection av BookingService och Logger
        public UpdateBooking(BookingService bookingService, ILogger<UpdateBooking> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        // Azure Function som aktiveras vid HTTP PUT-anrop mot "api/bookings/{id}"
        [Function("UpdateBooking")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "bookings/{id}")] HttpRequestData req, string id)
        {
            _logger.LogInformation($"Uppdaterar bokningen med id: {id}");

            // Läser request body som JSON och deserialiserar till Booking-objekt
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var updatedBooking = JsonSerializer.Deserialize<Booking>(body);

            // Validerar att både id och deserialiserat objekt är giltiga
            if (updatedBooking == null || string.IsNullOrEmpty(id))
            {
                _logger.LogError("Felaktig bokningsinfo");
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badResponse.WriteStringAsync("Ogiltig bokning eller id.");
                return badResponse;
            }

            // Försöker uppdatera bokningen i databasen via tjänsten
            var success = await _bookingService.UpdateAsync(id, updatedBooking);

            // Om uppdateringen misslyckas (exempelvis för att id inte finns), returnera 404 Not Found
            if (!success)
            {
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                await notFoundResponse.WriteStringAsync("Bokning hittades inte.");
                return notFoundResponse;
            }

            // Om allt går bra, returnera 200 OK och den uppdaterade bokningen i JSON-format
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(updatedBooking);
            return response;
        }
    }
}
