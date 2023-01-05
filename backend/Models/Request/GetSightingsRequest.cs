namespace WhaleSpotting.Models.Request
{
    public class GetSightingsRequest 
    {
        public int? SpeciesId { get; set; }
        public int? LocationId { get; set; }
        public float? Lat { get; set; }
        public float? Long { get; set; }
        public int? Dist { get; set; }
    }
}