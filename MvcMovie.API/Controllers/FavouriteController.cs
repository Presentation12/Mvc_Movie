using Microsoft.AspNetCore.Mvc;
using MvcMovieDAL.Entities;
using MvcMovieDAL;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MvcMovie.API.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MvcMovie.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FavouriteController : Controller
    {
        private readonly IRepository<Favourite> _favouriteRepository;
        private readonly IRepository<Genre> _genreRepository;
        private readonly IRepository<Movie> _movieRepository;
        private readonly IRepository<User> _userRepository;

        public FavouriteController(IRepository<Movie> movieRepository, IRepository<Genre> genreRepository,
            IRepository<Favourite> favouriteRepository, IRepository<User> userRepository)
        {
            _movieRepository = movieRepository;
            _genreRepository = genreRepository;
            _favouriteRepository = favouriteRepository;
            _userRepository = userRepository;
        }

        #region GET

        // GET: FavouriteController/Details/5
        [HttpGet("Details")]
        public async Task<IActionResult> Details(int id)
        {
            if (id < 0 || _favouriteRepository.Get().Where(x => x.Movie.Id == id).SingleOrDefault() == null)
            {
                return NotFound("Favourite Movie not found.");
            }

            var movie = _favouriteRepository.Get().Where(x => x.Movie.Id == id).Include(x => x.Movie.Genre).Select(x => new
            {
                x.Movie.Id,
                x.Movie.Title,
                x.Movie.ReleaseDate,
                GenreName = x.Movie.Genre.Name,
                x.Movie.Price,
                x.Movie.Rating
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

        #endregion

        #region POST

        [HttpPost("Paginate")]
        public async Task<JsonResult> Paginate([FromBody] BootstrapModel model)
        {
            model.Limit = model.Limit.HasValue && model.Limit != 0 ? model.Limit.Value : int.MaxValue;

            // Get user loged
            //var tokenDecoded = new JwtSecurityTokenHandler().ReadJwtToken(HttpContext.Session.GetString("token"));
            var tokenDecoded = new JwtSecurityTokenHandler().ReadJwtToken(model.Search.FirstOrDefault(x => x.Name == "Token").Value);

            var claimMail = tokenDecoded.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            string userMail = claimMail?.Value;

            var user = GetUserByEmailAsync(userMail);

            if (user == null)
            {
                return new JsonResult(new { error = "User not found." }) { StatusCode = 404 };
            }

            var query = _favouriteRepository.Get().Where(x => x.User.Id == user.Id);

            var genreName = model.Search.FirstOrDefault(x => x.Name == "Genre");
            if (genreName != null && !string.IsNullOrEmpty(genreName.Value))
            {
                query = query.Where(x => x.Movie.Genre.Name.ToUpper().StartsWith(genreName.Value.ToUpper()));
            }

            var title = model.Search.FirstOrDefault(x => x.Name == "Title");
            if (title != null && !string.IsNullOrEmpty(title.Value))
            {
                query = query.Where(x => x.Movie.Title.ToUpper().StartsWith(title.Value.ToUpper()));
            }

            query = query.Include(x => x.Movie.Genre);

            var result = await query.Skip(model.Offset).Take(model.Limit.Value).Select(x => new
            {
                x.Movie.Id,
                x.Movie.Title,
                x.Movie.ReleaseDate,
                GenreName = x.Movie.Genre.Name,
                x.Movie.Price,
                x.Movie.Rating
            }).ToListAsync();


            var _count = query.Count();

            return Json(new
            {
                rows = result,
                total = _count,
                totalNotFiltered = _count
            });
        }

        // POST: FavouriteController/Update
        [HttpPost("Update")]
        public async Task<IActionResult> Update(int idMovie)
        {
            Movie movieEntitie = _movieRepository.Get().Where(x => x.Id == idMovie).Include(x => x.Genre).SingleOrDefault();

            // Get user loged
            var tokenDecoded = new JwtSecurityTokenHandler().ReadJwtToken(HttpContext.Session.GetString("token"));

            var claimMail = tokenDecoded.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            string userMail = claimMail?.Value;

            var user = GetUserByEmailAsync(userMail);

            if (user == null)
            {
                return new JsonResult(new { error = "User not found." }) { StatusCode = 404 };
            }

            Favourite favouriteMovie = new Favourite
            {
                Movie = movieEntitie,
                User = user,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Active = true
            };

            _favouriteRepository.Insert(favouriteMovie);
            _favouriteRepository.Save();

            string jsonObject = JsonSerializer.Serialize(_favouriteRepository.Get().OrderBy(x => x.Id).LastOrDefault(),
                    new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve
                    });

            if (!jsonObject.IsNullOrEmpty())
            {
                return new JsonResult(jsonObject);
            }
            return NotFound(new { error = "Favourite Movie not found." }); ;
        }

        // POST: FavouriteController/Delete/5
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        public IActionResult Delete(int id)
        {
            if (_favouriteRepository.Get().Select(x => x.Movie) == null)
            {
                return NotFound(new { error = "Favourite Movie list empty." });
            }

            var movie = _favouriteRepository.Get().Where(x => x.Movie.Id == id).Include(x => x.Movie.Genre).SingleOrDefault();

            if (movie != null)
            {
                _favouriteRepository.Delete(movie.Id);
                _favouriteRepository.Save();
                return new JsonResult(true) { StatusCode = 200 };
            }

            return NotFound("Favourite Movie not found");
        }

        #endregion

        #region SECONDARY METHODS

        private User GetUserByEmailAsync(string email)
        {
            return _userRepository.Get().Where(x => x.Email == email).SingleOrDefault();
        }

        #endregion

    }
}
