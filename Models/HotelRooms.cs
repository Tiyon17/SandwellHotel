namespace SandwellHotel.Models
{
    public class HotelRooms
    {
        public int Id { get; set; }// The Unique Id of each hotel room 

        public string Name { get; set; }// The Name of hotel room

        public string Location { get; set; }//The location of the hotel room(floor,door number)

        public int PricePerNight { get; set; }// The price for each night in the hotel.

        public int StarRating { get; set; }//The rating of the room

        public int GuestRating { get; set; }//The rating of room from guests

        public int NumberOfBeds { get; set; }//The number of beds in the room

        public bool IsAvailable { get; set; }// Is the room availabl
    }
}
