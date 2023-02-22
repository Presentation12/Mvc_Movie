using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;
using MvcMovieDAL;
using MvcMovieDAL.Entities;
using System.Linq;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IRepository<Movie> _movieRepository;
        private readonly IRepository<Genre> _genreRepository;

        public MoviesController(IRepository<Movie> movieRepository, IRepository<Genre> genreRepository)
        {
            _movieRepository = movieRepository;
            _genreRepository = genreRepository;
        }

        // GET: Movies
        public async Task<IActionResult> Index(string movieGenre, string searchString)
        {
            // Use LINQ to get list of genres.
            IQueryable<string> genreQuery = (from m in _movieRepository.Get()
                                             orderby m.Genre.Name
                                             select m.Genre.Name).AsQueryable();

            var movies = (from m in _movieRepository.Get()
                          select m).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title!.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre.Name == movieGenre);
            }

            var movieGenreVM = new MovieGenreViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Movies = await movies.Select(x => new MovieViewModel()
                { 
                    Genre = x.Genre.Name,
                    Title = x.Title,
                    Id = x.Id,
                    Rating = x.Rating,
                    ReleaseDate = x.ReleaseDate,
                    Price = x.Price,
                }).ToListAsync()
            };

            return View(movieGenreVM);
        }

        //[HttpPost]
        //public string Index(string search, bool notUsed)
        //{
        //    return "From [HttpPost]Index: filter on " + search;
        //}

        // GET: Movies/Details/5

        public async Task<IActionResult> Details()
        {
            return View();
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit()
        {
            return View();
        }

        // GET: Movies/Delete/5
        //public async Task<IActionResult> Delete(int id)
        //{
        //    if (id < 0 || !_movieRepository.Exists(id))
        //    {
        //        return NotFound();
        //    }

        //    var movie = _movieRepository.Get().Where(x => x.Id == id).Include(x => x.Genre).Select(x => new
        //    {
        //        x.Id,
        //        x.Title,
        //        x.ReleaseDate,
        //        GenreName = x.Genre.Name,
        //        x.Price,
        //        x.Rating
        //    }).SingleOrDefault();

        //    MovieViewModel movieView = new MovieViewModel();

        //    movieView.Id = movie.Id;
        //    movieView.Title = movie.Title;
        //    movieView.Price = movie.Price;
        //    movieView.ReleaseDate = movie.ReleaseDate;
        //    movieView.Rating = movie.Rating;
        //    movieView.Genre = movie.GenreName;

        //    return View(movieView);
        //}
    }
}
