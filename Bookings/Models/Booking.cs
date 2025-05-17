using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bookings.Models
{
    public class Booking
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("GuestName")]

        public string GuestName { get; set; } 
        [BsonElement("CheckInDate")]
        public DateTime CheckInDate { get; set; }
        [BsonElement("CheckOutDate")]
        public DateTime CheckOutDate { get; set; }
        [BsonElement("Guests")]
        public int Guests { get; set; }
    }
}
