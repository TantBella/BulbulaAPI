using Bookings.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Bookings.Services
{
    public class BookingService
    {
        private readonly IMongoCollection<Booking> _bookings;

        public BookingService(IConfiguration config)
        {

            var connectionString = config["CosmosDbConnection"];
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("DbConnection saknas");
                throw new Exception("Saknad DbConnection");
            }

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("AirbnbDb");
            _bookings = database.GetCollection<Booking>("Bookings");

            //var client = new MongoClient(config["CosmosDbConnection"]);
            //var database = client.GetDatabase("AirbnbDb");
            //_bookings = database.GetCollection<Booking>("Bookings");
        }

        public async Task<List<Booking>> GetAllAsync() =>
            await _bookings.Find(_ => true).ToListAsync();

        public async Task CreateAsync(Booking booking) =>
            await _bookings.InsertOneAsync(booking);

        public async Task<bool> UpdateAsync(string id, Booking updatedBooking)
        {
            var filter = Builders<Booking>.Filter.Eq(b => b.Id, id);
            var updateResult = await _bookings.ReplaceOneAsync(filter, updatedBooking);

            return updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var filter = Builders<Booking>.Filter.Eq(b => b.Id, id);
            var deleteResult = await _bookings.DeleteOneAsync(filter);

            return deleteResult.DeletedCount > 0;
        }
    }
}
