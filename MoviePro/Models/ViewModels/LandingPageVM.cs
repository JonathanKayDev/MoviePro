using MoviePro.Models.Database;
using MoviePro.Models.TMDB;

namespace MoviePro.Models.ViewModels
{
    // LandingPageVM is an aggregate of Collection Model and MovieSearch Model
    public class LandingPageVM
    {
        public List<Collection>? CustomCollections { get; set; }
        // Each property to hold it's own MovieSearch info, so they can be independently displayed on the landing page
        public MovieSearch? NowPlaying { get; set; }
        public MovieSearch? Popular { get; set; }
        public MovieSearch? TopRated { get; set; }
        public MovieSearch? Upcoming { get; set; }
    }
}
