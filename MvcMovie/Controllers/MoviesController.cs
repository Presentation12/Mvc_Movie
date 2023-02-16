﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public string Index(string search, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + search;
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id < 0 || !_movieRepository.Exists(id))
            {
                return NotFound();
            }

            var movie = _movieRepository.Get().Where(x => x.Id == id).Include(x => x.Genre).Select(x => new
            {
                x.Id,
                x.Title,
                x.ReleaseDate,
                GenreName = x.Genre.Name,
                x.Price,
                x.Rating
            }).SingleOrDefault();

            MovieViewModel movieDetails = new MovieViewModel();

            movieDetails.Id = movie.Id;
            movieDetails.Title = movie.Title;
            movieDetails.ReleaseDate = movie.ReleaseDate;
            movieDetails.Rating = movie.Rating;
            movieDetails.Price = movie.Price;
            movieDetails.Genre = movie.GenreName;

            return View(movieDetails);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            // Use LINQ to get list of genres.
            IQueryable<string> genreQuery = (from m in _genreRepository.Get()
                                             orderby m.Name
                                             select m.Name).AsQueryable();

            //Verificar quando estiver vazio

            var movieGenreVM = new MovieAllGenresViewModel
            (
                new SelectList(genreQuery)
            );

            return View(movieGenreVM);
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] MovieViewModel movie)
        {
            if (ModelState.IsValid)
            {
                Movie movieInsert = new Movie();

                movieInsert.Title = movie.Title;
                movieInsert.ReleaseDate = movie.ReleaseDate;
                movieInsert.Rating = movie.Rating;
                movieInsert.Price = movie.Price;

                Genre genre = _genreRepository.Get().Where(x => x.Name == movie.Genre).SingleOrDefault();
                movieInsert.GenreId = genre.Id;

                movieInsert.CreatedDate = DateTime.Now;
                movieInsert.UpdatedDate = DateTime.Now;
                movieInsert.Active = true;

                _movieRepository.Insert(movieInsert);
                _movieRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id < 0 || !_movieRepository.Exists(id))
            {
                return NotFound();
            }

            var movie = _movieRepository.Get().Where(x => x.Id == id).Include(x => x.Genre).Select(x => new
            {
                x.Id,
                x.Title,
                x.ReleaseDate,
                GenreName = x.Genre.Name,
                x.Price,
                x.Rating
            }).SingleOrDefault();

            // Use LINQ to get list of genres.
            IQueryable<string> genreQuery = (from m in _genreRepository.Get()
                                             orderby m.Name
                                             select m.Name).AsQueryable();

            //Verificar quando estiver vazio

            var movieGenreVM = new MovieAllGenresViewModel
            (
                new SelectList(genreQuery),
                movie.Id,
                movie.Title,
                movie.Price,
                movie.ReleaseDate,
                movie.Rating
            );

            movieGenreVM.Genre = movie.GenreName;

            return View(movieGenreVM);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] MovieViewModel movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Movie movieEdit = new Movie();

                    movieEdit.Id = movie.Id;
                    movieEdit.Title = movie.Title;
                    movieEdit.ReleaseDate = movie.ReleaseDate;
                    movieEdit.Rating = movie.Rating;
                    movieEdit.Price = movie.Price;

                    Genre genre = _genreRepository.Get().Where(x => x.Name == movie.Genre).SingleOrDefault();
                    movieEdit.GenreId = genre.Id;

                    movieEdit.UpdatedDate = DateTime.Now;

                    _movieRepository.Update(movieEdit);
                    _movieRepository.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Redirects to the list of movies (Movies/Index)
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 0 || !_movieRepository.Exists(id))
            {
                return NotFound();
            }

            var movie = _movieRepository.Get().Where(x => x.Id == id).Include(x => x.Genre).Select(x => new
            {
                x.Id,
                x.Title,
                x.ReleaseDate,
                GenreName = x.Genre.Name,
                x.Price,
                x.Rating
            }).SingleOrDefault();

            MovieViewModel movieView = new MovieViewModel();

            movieView.Id = movie.Id;
            movieView.Title = movie.Title;
            movieView.Price = movie.Price;
            movieView.ReleaseDate = movie.ReleaseDate;
            movieView.Rating = movie.Rating;
            movieView.Genre = movie.GenreName;

            return View(movieView);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_movieRepository.Get() == null)
            {
                return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
            }

            var movie = _movieRepository.Get().Where(x => x.Id == id).Include(x => x.Genre).SingleOrDefault();

            if (movie != null)
            {
                _movieRepository.Delete(id);
            }

            _movieRepository.Save();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<JsonResult> Paginate([FromBody] BootstrapModel model)
        {
            model.Limit = model.Limit.HasValue ? model.Limit.Value : int.MaxValue;

            var query = _movieRepository.Get();

            var genreName = model.Search.FirstOrDefault(x => x.Name == "Genre");
            if (genreName != null && !string.IsNullOrEmpty(genreName.Value))
            {
                query = query.Where(x => x.Genre.Name.ToUpper().StartsWith(genreName.Value.ToUpper()));
            }

            var title = model.Search.FirstOrDefault(x => x.Name == "Title");
            if (title != null && !string.IsNullOrEmpty(title.Value))
            {
                query = query.Where(x => x.Title.ToUpper().StartsWith(title.Value.ToUpper()));
            }

            query = query.Include(x => x.Genre);

            var result = await query.Skip(model.Offset).Take(model.Limit.Value).Select(x => new
            {
                x.Id,
                x.Title,
                x.ReleaseDate,
                GenreName = x.Genre.Name,
                x.Price,
                x.Rating
            }).ToListAsync();


            var _count = query.Count();

            return Json(new
            {
                rows = result,
                total = _count,
                totalNotFiltered = _count
            });
        }

        private bool MovieExists(int id)
        {
            var entity = _movieRepository.Get().Where(x => x.Id == id).Include(x => x.Genre).SingleOrDefault();

            if (entity != null)
                return true;

            return false;
        }
    }
}
