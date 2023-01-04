using System.ComponentModel.DataAnnotations;
using System;

namespace WhaleSpotting.Models.Request
{
    public class CreateSightingRequest
    {
        [Required]
        [StringLength(70)]
        public string SeenBy { get; set; }

        [Required]
        public DateOnly SeenOn { get; set; }

        [Required]
        public float Latitude { get; set; }

        [Required]
        public float Longitude { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public int WhaleCount { get; set; }

        public int SpeciesId { get; set; }
    }
}
