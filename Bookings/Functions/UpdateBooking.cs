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

        public UpdateBooking(BookingService bookingService, ILogger<UpdateBooking> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        [Function("UpdateBooking")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "bookings/{id}")] HttpRequestData req, string id)
        {
            _logger.LogInformation($"Uppdaterar bkoningen med id: {id}");

            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var updatedBooking = JsonSerializer.Deserialize<Booking>(body);

            if (updatedBooking == null || string.IsNullOrEmpty(id))
            {
                _logger.LogError("Felaktig bokningsinfo");
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badResponse.WriteStringAsync("Ogiltig bokning eller id.");
                return badResponse;
            }

            var success = await _bookingService.UpdateAsync(id, updatedBooking);

            if (!success)
            {
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                await notFoundResponse.WriteStringAsync("Bokning hittades inte.");
                return notFoundResponse;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(updatedBooking);
            return response;
        }
    }
}
