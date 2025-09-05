﻿using Microsoft.AspNetCore.Mvc;
using MvcMovie.Models;
using MvcMovie.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcMovie.Features.Movies.Models;
using MvcMovie.Features.Movies.Services;

namespace MvcMovie.Features.Movies.Controllers
{
    [Route("movies")]
    public class MoviesController : Controller
    {
        private readonly iMovieService _movies;
        private readonly ILogger<MoviesController> _logger;
        public MoviesController(iMovieService movies, ILogger<MoviesController> logger)
        {
            _movies = movies;
            _logger = logger;
        }

        // GET: Movies
        [HttpGet("")]
        public async Task<IActionResult> Index(string movieGenre, string searchString)
        {


            IEnumerable<Movie> all = await _movies.GetAllAsync();
            IEnumerable<Movie> movies = all;
            IEnumerable<string?> genreQuery = all.Select(movie => movie.Genre).Distinct();

            if (!string.IsNullOrEmpty(searchString))
            {

                movies = movies.Where(movie => movie.Title != null && movie.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase));
                _logger.Info("Searching by string {searchstring}", searchString);
            }

            if (!string.IsNullOrEmpty(movieGenre))
            {

                movies = movies.Where(movie => movie.Genre == movieGenre);
                _logger.Info("Searching by genre {movieGenre}", movieGenre);
            }

            var movieGenreVM = new MovieGenreViewModel {
                Genres = new SelectList(genreQuery),
                Movies = movies.ToList(),
            };

            return View(movieGenreVM);
        }
        // GET: Movies/Details/5
        [HttpGet("details/{id:int}", Name = "MovieDetails")]
        public async Task<IActionResult> Details(int id)
        {


            var movie = await _movies.GetByIdAsync(id);
            _logger.Info("Displaying Details for movie {id}", id);
            return View(movie);
        }

        // GET: Movies/Create
        [HttpGet("create")]
        public IActionResult Create()
        {
            _logger.Info("Create Get");
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _logger.Warn("Create POST model Invalid");
                return View(movie);

            }
            await _movies.AddAsync(movie);
            return RedirectToAction(nameof(Index)); ;
        }

        // GET: Movies/Edit/5
        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {


            var movie = await _movies.GetByIdAsync(id);
            _logger.Info("Edit GET, movie  id {id}", id);
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {


            if (!ModelState.IsValid)
            {
                _logger.Warn("Edit POST is invalid)");
                return View(movie);
            }
            await _movies.UpdateAsync(movie);
            return RedirectToAction(nameof(Index));
        }

        // GET: Movies/Delete/5
        [HttpGet("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _movies.GetByIdAsync(id);
            _logger.Info("Delete GET, movie id {id)", id);
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        { await _movies.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Movies/ by Genre
        [HttpGet("bygenre/{genre}")]
        public async Task<IActionResult> ByGenre(string genre)
        {
            var all = await _movies.GetAllAsync();
            var movies = all.Where(movie => movie.Genre != null && string.Equals(movie.Genre, genre, StringComparison.OrdinalIgnoreCase));
            var ViewModel = new MovieGenreViewModel
            {
                Genres = new SelectList(all.Select(m => m.Genre).Distinct()),
                Movies = movies.ToList(),
                MovieGenre = genre
            };
            return View("Index", ViewModel);
        }

        // GET: Movies/ Released/2010/5
        [HttpGet("released/{year:int:min(1900)}/{month:int:range(1,12)?}")]
        public async Task<IActionResult> Released(int year, int month)
        {
            var all = await _movies.GetAllAsync();
            var movies = all.Where(movie => movie.ReleaseDate.Year == year && (month == 0 ? true :movie.ReleaseDate.Month == month));
            var ViewModel = new MovieGenreViewModel
            {
                Genres = new SelectList(all.Select(m => m.Genre).Distinct()),
                Movies = movies.ToList(),
            };
            return View("Index", ViewModel);
        }
    }
}
