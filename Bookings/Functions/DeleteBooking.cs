using Bookings.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Bookings.Functions
{
    public class DeleteBooking
    {
        private readonly BookingService _bookingService;
        private readonly ILogger<DeleteBooking> _logger;

        // Konstruktor som tar emot BookingService och Logger via dependency injection
        public DeleteBooking(BookingService bookingService, ILogger<DeleteBooking> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        // Azure Function som triggas vid HTTP DELETE mot "api/bookings/{id}"
        [Function("DeleteBooking")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "bookings/{id}")] HttpRequestData req, string id)
        {
            _logger.LogInformation($"Försöker radera bokning med id: {id}");

            // Anropar service för att radera bokningen med det angivna id:t
            var success = await _bookingService.DeleteAsync(id);

            // Om raderingen misslyckades (t.ex. bokningen finns inte), returnera 404 Not Found
            if (!success)
            {
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                await notFoundResponse.WriteStringAsync("Bokning hittades inte.");
                return notFoundResponse;
            }

            // Vid lyckad radering returnera 204 No Content (inget innehåll)
            var response = req.CreateResponse(HttpStatusCode.NoContent);
            return response;
        }
    }
}
