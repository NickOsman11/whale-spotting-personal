namespace WhaleSpotting.Models.Request
{
    public class GetSightingsRequest 
    {
        public int? SpeciesId { get; set; }
        public int? LocationId { get; set; }
    }
}