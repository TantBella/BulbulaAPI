using Bookings.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Bookings.Services
{
    /// <summary>
    /// BookingService hanterar CRUD-operationer mot en MongoDB-databas
    /// som är hostad i Azure Cosmos DB.
    /// </summary>
    public class BookingService
    {
        // Referens till samlingen 'Bookings' i databasen
        private readonly IMongoCollection<Booking> _bookings;

        /// <summary>
        /// Konstruktor som initierar anslutningen till databasen via en connection string från appinställningar.
        /// </summary>
        /// <param name="config">Konfigurationsinställningar (IConfiguration)</param>
        public BookingService(IConfiguration config)
        {
            // Hämtar connection string från appsettings / miljövariabel
            var connectionString = config["CosmosDbConnection"];

            // Kontrollera att connection string är angiven
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("DbConnection saknas");
                throw new Exception("Saknad DbConnection");
            }

            // Initiera MongoDB-klient och koppla till rätt databas och samling
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("AirbnbDb");
            _bookings = database.GetCollection<Booking>("Bookings");
        }

        /// <summary>
        /// Hämtar alla bokningar från databasen.
        /// </summary>
        /// <returns>Lista med bokningar</returns>
        public async Task<List<Booking>> GetAllAsync() =>
            await _bookings.Find(_ => true).ToListAsync();

        /// <summary>
        /// Lägger till en ny bokning i databasen.
        /// </summary>
        /// <param name="booking">Bokningsobjekt som ska sparas</param>
        public async Task CreateAsync(Booking booking) =>
            await _bookings.InsertOneAsync(booking);

        /// <summary>
        /// Uppdaterar en befintlig bokning baserat på ID.
        /// </summary>
        /// <param name="id">ID för bokningen som ska uppdateras</param>
        /// <param name="updatedBooking">Nya bokningsdata</param>
        /// <returns>True om en post uppdaterades, annars false</returns>
        public async Task<bool> UpdateAsync(string id, Booking updatedBooking)
        {
            var filter = Builders<Booking>.Filter.Eq(b => b.Id, id);
            var updateResult = await _bookings.ReplaceOneAsync(filter, updatedBooking);

            return updateResult.ModifiedCount > 0;
        }

        /// <summary>
        /// Tar bort en bokning baserat på ID.
        /// </summary>
        /// <param name="id">ID för bokningen som ska raderas</param>
        /// <returns>True om en post raderades, annars false</returns>
        public async Task<bool> DeleteAsync(string id)
        {
            var filter = Builders<Booking>.Filter.Eq(b => b.Id, id);
            var deleteResult = await _bookings.DeleteOneAsync(filter);

            return deleteResult.DeletedCount > 0;
        }
    }
}
