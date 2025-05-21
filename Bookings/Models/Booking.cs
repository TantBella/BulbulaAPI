using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bookings.Models
{
    // Modellklass som representerar en bokning i databasen
    public class Booking
    {
        // Markerar denna property som dokumentets ID i Databas-tabellen
        // Representation som ObjectId (sträng) i C#
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        // Mappas till fältet "GuestName" i Databas-tabellen
        // Namnet på gästen som gjort bokningen
        [BsonElement("GuestName")]
        public string GuestName { get; set; }

        // Mappas till fältet "CheckInDate"
        // Datum för när gästen checkar in
        [BsonElement("CheckInDate")]
        public DateTime CheckInDate { get; set; }

        // Mappas till fältet "CheckOutDate"
        // Datum för när gästen checkar ut
        [BsonElement("CheckOutDate")]
        public DateTime CheckOutDate { get; set; }

        // Mappas till fältet "Guests"
        // Antal gäster som ingår i bokningen
        [BsonElement("Guests")]
        public int Guests { get; set; }
    }
}
