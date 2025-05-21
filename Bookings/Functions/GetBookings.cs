using Bookings.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Bookings.Functions
{
    public class GetBookings
    {
        private readonly BookingService _bookingService;
        private readonly ILogger<GetBookings> _logger;

        // Konstruktor för dependency injection av BookingService och Logger
        public GetBookings(BookingService bookingService, ILogger<GetBookings> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        // Azure Function som aktiveras vid HTTP GET-anrop mot "api/bookings"
        [Function("GetBookings")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "bookings")] HttpRequestData req)
        {
            _logger.LogInformation("Hämtar bokningar");

            // Hämtar alla bokningar asynkront från tjänsten
            var bookings = await _bookingService.GetAllAsync();

            // Om inga bokningar finns, returnera 404 Not Found med ett meddelande
            if (bookings == null || bookings.Count == 0)
            {
                _logger.LogWarning("Det finns inga bokningar just nu.");
                var emptyResponse = req.CreateResponse(HttpStatusCode.NotFound);
                await emptyResponse.WriteAsJsonAsync(new { message = "Det finns inga bokningar just nu." });
                return emptyResponse;
            }

            // Skapar svar med status 200 OK och skickar med listan av bokningar i JSON-format
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(bookings);

            _logger.LogInformation($"Det finns {bookings.Count} st bokningar.");
            return response;
        }
    }
}
