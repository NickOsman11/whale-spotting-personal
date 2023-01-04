using System; 

namespace WhaleSpotting.Models.Database
{
    public class Sighting
    {
        public int Id { get; set; }
        public Species Species { get; set; }
        public string SeenBy { get; set; }
        public DateOnly SeenOn { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public int WhaleCount { get; set; }
        public ConfirmationStatus ConfirmationStatus { get; set; }
        public Location Location { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
