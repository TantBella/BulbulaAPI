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

        public DeleteBooking(BookingService bookingService, ILogger<DeleteBooking> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        [Function("DeleteBooking")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "bookings/{id}")] HttpRequestData req, string id)
        {
            _logger.LogInformation($"Radera bokning med id: {id}");

            var success = await _bookingService.DeleteAsync(id);

            if (!success)
            {
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                await notFoundResponse.WriteStringAsync("Bokning hittades inte.");
                return notFoundResponse;
            }

            var response = req.CreateResponse(HttpStatusCode.NoContent);
            return response;
        }
    }
}
