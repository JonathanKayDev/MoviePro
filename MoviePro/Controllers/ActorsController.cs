﻿using Microsoft.AspNetCore.Mvc;
using MoviePro.Services.Interfaces;

namespace MoviePro.Controllers
{
    public class ActorsController : Controller
    {
        private readonly IRemoteMovieService _tmdbMovieService;
        private readonly IDataMappingService _mappingService;

        public ActorsController(IRemoteMovieService tmdbMovieService, IDataMappingService mappingService)
        {
            _tmdbMovieService = tmdbMovieService;
            _mappingService = mappingService;
        }


        public async Task<IActionResult> Details(int id)
        {
            // local variable with raw data from API
            var actor = await _tmdbMovieService.ActorDetailAsync(id);
            // update actor info
            actor = _mappingService.MapActorDetail(actor);

            return View(actor);
        }
    }
}
