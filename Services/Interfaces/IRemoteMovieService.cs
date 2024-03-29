﻿using MoviePro.Enums;
using MoviePro.Models.TMDB;

namespace MoviePro.Services.Interfaces
{
    public interface IRemoteMovieService
    {
        Task<MovieDetail> MovieDetailAsync(int id);
        Task<MovieSearch> SearchMoviesAsync(MovieCategory category, int count);
        Task<MovieSearch> SearchMoviesAsync(string searchTerm);
        Task<ActorDetail> ActorDetailAsync(int id);
        string GenerateSearchRequestUri();
    }
}
