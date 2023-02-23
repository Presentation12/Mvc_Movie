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
        // GET: FavouriteController
        public async Task<IActionResult> Index()
        {
            return View();
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
