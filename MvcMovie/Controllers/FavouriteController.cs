using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcMovieDAL.Entities;
using MvcMovieDAL;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MvcMovie.Controllers
{
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

        public User GetUserByEmailAsync(string email)
        {
            return _userRepository.Get().Where(x => x.Email == email).SingleOrDefault();
        }

        // GET: FavouriteController
        public async Task<IActionResult> Index(string movieGenre, string searchString)
        {
            // Get user loged
            var tokenDecoded = new JwtSecurityTokenHandler().ReadJwtToken(HttpContext.Session.GetString("token"));

            var claimMail = tokenDecoded.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            string userMail = claimMail?.Value;

            var user = GetUserByEmailAsync(userMail);

            if (user == null)
            {
                return NotFound();
            }

            // Use LINQ to get list of genres.
            IQueryable<string> genreQuery = (from m in _genreRepository.Get()
                                             orderby m.Name
                                             select m.Name).AsQueryable();

            var favouriteMovies = (from m in _favouriteRepository.Get().Where(x => x.User.Id == user.Id) select m.Movie).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                favouriteMovies = favouriteMovies.Where(s => s.Title!.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(movieGenre))
            {
                favouriteMovies = favouriteMovies.Where(x => x.Genre.Name == movieGenre);
            }

            var movieGenreVM = new MovieGenreViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Movies = await favouriteMovies.Select(x => new MovieViewModel()
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

        // GET: FavouriteController/Details/5
        public async Task<IActionResult> Details()
        {
            return View();
        }

        //public async Task<IActionResult> Delete(int id)
        //{
        //    if (id < 0 || _favouriteRepository.Get().Where(x => x.Movie.Id == id).SingleOrDefault() == null)
        //    {
        //        return NotFound();
        //    }

        //    var movie = _favouriteRepository.Get().Where(x => x.Movie.Id == id).Include(x => x.Movie.Genre).Select(x => new
        //    {
        //        x.Movie.Id,
        //        x.Movie.Title,
        //        x.Movie.ReleaseDate,
        //        GenreName = x.Movie.Genre.Name,
        //        x.Movie.Price,
        //        x.Movie.Rating
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
