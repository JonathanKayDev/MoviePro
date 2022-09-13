namespace MoviePro.Models.Database
{
    public class MovieCollection
    {
        // Primary Key
        public int Id { get; set; }
        // Forgeing Keys
        public int CollectionId { get; set; }
        public int MovieId { get; set; }

        public int Order { get; set; }

        // Navigational properties
        public Collection? Collection { get; set; }
        public Movie? Movie { get; set; }
    }
}
