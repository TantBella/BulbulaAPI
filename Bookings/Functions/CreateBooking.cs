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

        public CreateBooking(BookingService bookingService, ILogger<CreateBooking> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        [Function("CreateBooking")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "bookings")] HttpRequestData req)
        {
            _logger.LogInformation("Skapar ny bokning");

            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var booking = JsonSerializer.Deserialize<Booking>(body);

            if (booking is null)
            {
                _logger.LogError("Ogiltig bokning");
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badResponse.WriteAsJsonAsync(new { message = "Ogiltig bokning." });
                return badResponse;
            }

            await _bookingService.CreateAsync(booking);
            _logger.LogInformation($"Bokning skapad för {booking.GuestName}");

            var response = req.CreateResponse(HttpStatusCode.Created);
            await response.WriteAsJsonAsync(booking);
            return response;
        }
    }
}
