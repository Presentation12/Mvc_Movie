using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MvcMovie.API.Models;
using MvcMovieDAL;
using MvcMovieDAL.Entities;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace MvcMovie.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : Controller
    {
        private readonly IRepository<Movie> _movieRepository;
        private readonly IRepository<Genre> _genreRepository;

        public MoviesController(IRepository<Movie> movieRepository, IRepository<Genre> genreRepository)
        {
            _movieRepository = movieRepository;
            _genreRepository = genreRepository;
        }

        #region GET 

        [HttpGet("Details")]
        public async Task<IActionResult> Details(int idMovie)
        {
            if (idMovie < 0 || !_movieRepository.Exists(idMovie))
            {
                return NotFound("Movie not found.");
            }

            var movie = _movieRepository.Get().Where(x => x.Id == idMovie).Include(x => x.Genre).Select(x => new
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

            return new JsonResult(movieDetails);
        }

        // GET: Movies/Create
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
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

            return new JsonResult(movieGenreVM);
        }

        // GET: Movies/Edit/5
        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(int idMovie)
        {
            if (idMovie < 0 || !_movieRepository.Exists(idMovie))
            {
                return NotFound("Movie not found.");
            }

            var movie = _movieRepository.Get().Where(x => x.Id == idMovie).Include(x => x.Genre).Select(x => new
            {
                x.Id,
                x.Title,
                x.ReleaseDate,
                GenreName = x.Genre.Name,
                x.Price,
                x.Rating
            }).SingleOrDefault();

            // Use LINQ to get list of genres.
            //IQueryable<string> genreQuery = (from m in _genreRepository.Get()
            //                                 orderby m.Name
            //                                 select m.Name).AsQueryable();

            //Verificar quando estiver vazio

            MovieViewModel movieDetails = new MovieViewModel();

            movieDetails.Id = movie.Id;
            movieDetails.Title = movie.Title;
            movieDetails.ReleaseDate = movie.ReleaseDate;
            movieDetails.Rating = movie.Rating;
            movieDetails.Price = movie.Price;
            movieDetails.Genre = movie.GenreName;

            return new JsonResult(movieDetails);
        }

        #endregion

        #region POST

        [HttpPost("Paginate")]
        public async Task<JsonResult> Paginate([FromBody] BootstrapModel model)
        {
            model.Limit = model.Limit.HasValue && model.Limit != 0 ? model.Limit.Value : int.MaxValue;

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

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost("Create")]
        public async Task<IActionResult> Create([Bind("Title,ReleaseDate,Genre,Price,Rating")] MovieViewModel movie)
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

                string jsonObject = JsonSerializer.Serialize(_genreRepository.Get().OrderBy(x => x.Id).LastOrDefault(),
                    new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve
                    });

                if (!jsonObject.IsNullOrEmpty())
                {
                    return new JsonResult(jsonObject);
                }

                return NotFound(new { error = "Movie not found." });

            }
            return BadRequest(new { error = "Movie was not updated." });
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] MovieViewModel movie)
        {
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

                    string jsonObject = JsonSerializer.Serialize(movieEdit,
                    new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve
                    });

                    // Redirects to the list of movies (Movies/Index)
                    return new JsonResult(jsonObject);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_movieRepository.Get().Where(x => x.Id == movie.Id).SingleOrDefault() == null)
                    {
                        return NotFound(new { error = "Movie not found." });
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return BadRequest(new { error = "Movie was not updated." });
        }

        // POST: Movies/Delete/5
        [HttpPost("Delete/{idMovie}"), ActionName("Delete")]
        public async Task<IActionResult> Delete(int idMovie)
        {
            if (idMovie < 0 || _movieRepository.Get() == null)
            {
                return NotFound(new { error = "Movie list empty." });
            }

            var movie = _movieRepository.Get().Where(x => x.Id == idMovie).Include(x => x.Genre).SingleOrDefault();

            if (movie != null)
            {
                _movieRepository.Delete(movie.Id);
            }

            _movieRepository.Save();
            return new JsonResult(true) { StatusCode = 200 };
        }

        #endregion

    }
}
