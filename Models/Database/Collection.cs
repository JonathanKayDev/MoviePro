using System.ComponentModel.DataAnnotations;

namespace MoviePro.Models.Database
{
    public class Collection
    {
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and no more than {1} characters long.", MinimumLength = 2)]
        public string? Name { get; set; }

        public string? Description { get; set; }

        // Navigation property
        public ICollection<MovieCollection> MovieCollections { get; set; } = new HashSet<MovieCollection>();
    }
}
